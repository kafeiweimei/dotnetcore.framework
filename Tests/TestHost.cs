using Autofac.Extensions.DependencyInjection;
using Common.Helper;
using DotNetCore.Framework.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Tests
{
    internal static class TestHost
    {
        /// <summary>
        /// 获取测试主机
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder GetTestHost()
        {
            return new HostBuilder()
                    //替换autofac作为DI容器
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder
                        .UseTestServer()
                        .UseStartup<Startup>();
                    })
                    .ConfigureAppConfiguration((host, builder) =>
                    {
                        builder.SetBasePath(Directory.GetCurrentDirectory());
                        builder.AddJsonFile("appsettings.json", optional: true);
                        builder.AddEnvironmentVariables();
                    });
        }

        /// <summary>
        /// 获取没有令牌的测试客户端
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static HttpClient GetTestClientNoToken(this IHost host)
        {
            var client = host.GetTestClient();
            return client;
        }

        /// <summary>
        /// 获取带有令牌的测试客户端
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static HttpClient GetTestClientWithToken(this IHost host)
        {
            // 获取令牌
            TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = "Admin" };
            var jwtStr = JwtHelper.IssueJwt(tokenModel);

            var client = host.GetTestClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtStr}");
            return client;
        }



    }//Class_end
}
