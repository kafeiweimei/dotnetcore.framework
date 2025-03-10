using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.AOP
{
    //日志拦截类，继承IInterceptor接口
    public class LogAop : IInterceptor
    {
        private readonly IHttpContextAccessor _accessor;

        public LogAop(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }


        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            string UserName = _accessor.HttpContext?.User?.Identity?.Name;

            //记录被拦截方法信息的日志信息
            var dataIntercept = "\r\n" +
                $"【当前执行时间】：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} \r\n" +
                $"【当前操作用户】：{UserName} \r\n" +
                $"【当前执行方法】：{invocation.Method.Name} \r\n" +
                $"【携带的参数有】： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            try
            {
                MiniProfiler.Current.Step($"执行Service方法：{invocation.Method.Name}() -> ");
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();


                // 异步获取异常，先执行
                if (IsAsyncMethod(invocation.Method))
                {

                    #region 方案一
                    //Wait task execution and modify return value
                    if (invocation.Method.ReturnType == typeof(Task))
                    {
                        invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                            (Task)invocation.ReturnValue,
                            async () => await SuccessAction(invocation, dataIntercept),/*成功时执行*/
                            ex =>
                            {
                                LogEx(ex, dataIntercept);
                            });
                    }
                    //Task<TResult>
                    else
                    {
                        invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                         invocation.Method.ReturnType.GenericTypeArguments[0],
                         invocation.ReturnValue,
                          //async () => await SuccessAction(invocation, dataIntercept),/*成功时执行*/
                          async (o) => await SuccessAction(invocation, dataIntercept, o),/*成功时执行*/
                         ex =>
                         {
                             LogEx(ex, dataIntercept);
                         });
                    }
                    #endregion


                    // 如果方案一不行，试试这个方案
                    #region 方案二

                    //var type = invocation.Method.ReturnType;
                    //var resultProperty = type.GetProperty("Result");
                    //dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue))}");

                    //Parallel.For(0, 1, e =>
                    //{
                    //    LogLock.OutSql2Log("AOPLog", new string[] { dataIntercept });
                    //});

                    #endregion
                }
                else
                {// 同步1

                    dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
                    OutLog(dataIntercept);
                }
            }
            catch (Exception ex)// 同步2
            {
                LogEx(ex, dataIntercept);

            }

        }

        private async Task SuccessAction(IInvocation invocation, string dataIntercept, object o = null)
        {
            //invocation.ReturnValue = o;
            //var type = invocation.Method.ReturnType;
            //if (typeof(Task).IsAssignableFrom(type))
            //{
            //    //var resultProperty = type.GetProperty("Result");
            //    //类型错误 都可以不要invocation参数，直接将o系列化保存到日记中
            //    dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(invocation.ReturnValue)}");
            //}
            //else
            //{
            //    dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
            //}

            dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(o)}");


            await Task.Run(() =>
            {
                OutLog(dataIntercept);
            });
        }

        private void LogEx(Exception ex, string dataIntercept)
        {
            if (ex != null)
            {
                //执行的 service 中，收录异常
                MiniProfiler.Current.CustomTiming("Errors：", ex.Message);
                //执行的 service 中，捕获异常
                dataIntercept += ($"【执行完成结果】：方法中出现异常：{ex.Message + ex.InnerException}\r\n");

                // 异常日志里有详细的堆栈信息
                OutLog(dataIntercept);
            }
        }


        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

        private void OutLog(string info)
        {
            var path = AppContext.BaseDirectory + @"\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = path + $@"\InterceptLog-{DateTime.Now.ToString("yyyyMMddHH")}.log";

            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(info);
            sw.Close();
        }

    }


    internal static class InternalAsyncHelper
    {
        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await actualReturnValue;
                await postAction();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<object, Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;
            try
            {
                var result = await actualReturnValue;
                await postAction(result);
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<object, Task> action, Action<Exception> finalAction)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, action, finalAction });
        }



    }//Class_end

}
