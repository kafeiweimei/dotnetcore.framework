using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Common
{
    public class AppSettingsHelper
    {
        private static IConfiguration Configuration { get; set; }
        private static string contentPath { get; set; }

        public AppSettingsHelper(string contentPath)
        {
            string path = "appsettings.json";
            //如果配置文件是根据环境变量来区分的，可以这样配置
            //path = $"appsettinsg.{Environment.GetEnvironmentVariable("ASPNETCORE_EN-VIRONMENT")}.json";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                //这样配置，可以直接读取目录中的json文件，而不是bin文件夹下的，所以不用修改复制属性
                .Add(new JsonConfigurationSource { Path = path, Optional = false, ReloadOnChange = true })
                .Build();

        }

        public AppSettingsHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 要操作的字符
        /// </summary>
        /// <param name="sections">节点配置字符串数组</param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":",sections)];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }


        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> App<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":",sections),list);
            return list;
        }

        ////在Startup.cs下注入该帮助类
        //public void ConfigureService(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    services.AddSingleton(new AppSettingsHelper(Configuration));
        //}

    }//Class_end
}
