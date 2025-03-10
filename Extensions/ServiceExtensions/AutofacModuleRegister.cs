/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/24 16:11:16
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using Autofac;
using Autofac.Extras.DynamicProxy;
using Common;
using Common.Helper;
using Extensions.AOP;
using IServices.Base;
using Services.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;

namespace Extensions.ServiceExtensions
{
    public class AutofacModuleRegister:Autofac.Module
    {
        //AOP参数
        List<Type> cacheType = new List<Type>();

        protected override void Load(ContainerBuilder builder)
        {
            /*单个内容的依赖注入*/
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
            //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();

            /*程序集依赖注入*/

            var basePath = AppContext.BaseDirectory;

            string servicesDLL = "Services.dll";
            string repositoryDLL = "Repository.dll";

            //var serviceDLLFile = Path.Combine(basePath, servicesDLL);
            //var repositoryDLLFile = Path.Combine(basePath, repositoryDLL);
            //if (!(File.Exists(serviceDLLFile) && File.Exists(repositoryDLLFile)))
            //{
            //    var msg = $"{servicesDLL}和{repositoryDLL}丢失";
            //    throw new Exception(msg);
            //}

            //var assemblysServices = Assembly.LoadFrom(serviceDLLFile);
            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            //var assemblysRepository = Assembly.LoadFrom(repositoryDLLFile);
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            //AOP
            if (AppSettingsHelper.App(new string[] { "AppSettings", "LogAop", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<LogAop>();
                cacheType.Add(typeof(LogAop));
            }

            //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();


            AssemblyIOC(builder, basePath, servicesDLL,true);
            AssemblyIOC(builder, basePath, repositoryDLL);

          

        }

        /// <summary>
        /// 程序集依赖注入
        /// </summary>
        /// <param name="builder">Aotofac的依赖注入容器</param>
        /// <param name="basePath">需要注入的程序集基础路径</param>
        /// <param name="assemblyName">需要注入的程序集名称</param>
        private void AssemblyIOC(ContainerBuilder builder, string basePath,string assemblyName,bool enableAop=false)
        {
            if (builder==null||string.IsNullOrEmpty(basePath) ||string.IsNullOrEmpty(assemblyName)) return;

            var assemblyDLLPathAndName = Path.Combine(basePath, assemblyName);
            if (!(File.Exists(assemblyDLLPathAndName)))
            {
                var msg = $" {assemblyDLLPathAndName} 丢失";
                throw new Exception(msg);
            }

            var assemblyFile = Assembly.LoadFrom(assemblyDLLPathAndName);
            if (enableAop)
            {
                builder.RegisterAssemblyTypes(assemblyFile)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(cacheType.ToArray());
            }
            else
            {
                builder.RegisterAssemblyTypes(assemblyFile)
                .AsImplementedInterfaces();
            }
            
        }

    }//Class_end
}
