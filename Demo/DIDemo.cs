using Newtonsoft.Json;
using System;

namespace Demo
{
    /*依赖注入的演示示例（了解.NETCore自带的轻量级IOC【控制反转】）
     要使用这个示例只用在【Startup.cs】中的【ConfigureServices(IServiceCollection services)】方法下添加如下的内容即可
       //测试依赖注入控制反转
            services
                .AddSingleton<ISingletonTest,SingletonTest>()
                .AddScoped<IScopedTest, ScopedTest>()
                .AddTransient<ITransientTest, TransientTest>()
                .AddScoped<IAService,AService>()
                ;
     */

    public class DIDemo
    {

    }//Class_end

    #region   单例
    public interface ISingletonTest
    {
        int Age { get; set; }
        string Name { get; set; }

    }//Interface_end

    public class SingletonTest : ISingletonTest
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
    #endregion

    #region   会话
    public interface IScopedTest
    {
        int Age { get; set; }
        string Name { get; set; }
    }

    public class ScopedTest : IScopedTest
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

    #endregion

    #region   瞬态

    public interface ITransientTest
    {
        int Age { get; set; }
        string Name { get; set; }
    }

    public class TransientTest : ITransientTest
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

    #endregion


    #region   服务

    public interface IAService
    {
        void MethodTest();
    }

    public class AService : IAService
    {
        private ISingletonTest singletonTest;IScopedTest scopedTest;ITransientTest transientTest;

        public AService(ISingletonTest singletonTest,IScopedTest scopedTest,ITransientTest transientTest)
        {
            this.singletonTest = singletonTest;
            this.scopedTest = scopedTest;
            this.transientTest = transientTest;

            Console.WriteLine("------第一阶段（构造函数）------");
            Console.WriteLine($"Singleton:{JsonConvert.SerializeObject(singletonTest)}");
            Console.WriteLine($"Scoped:{JsonConvert.SerializeObject(scopedTest)}");
            Console.WriteLine($"Transient:{JsonConvert.SerializeObject(transientTest)}");
        }

        public void MethodTest()
        {
            Console.WriteLine("------第三阶段（调用服务的方法）------");
            Console.WriteLine($"Singleton:{JsonConvert.SerializeObject(singletonTest)}");
            Console.WriteLine($"Scoped:{JsonConvert.SerializeObject(scopedTest)}");
            Console.WriteLine($"Transient:{JsonConvert.SerializeObject(transientTest)}");
        }
    }

    #endregion


}
