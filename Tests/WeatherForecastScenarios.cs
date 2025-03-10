using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Tests
{
    public class WeatherForecastScenarios
    {

        [Fact]
        private async void Get_WeatherForecast_response_ok_status()
        {
            string needRequstUrl = "/api/WeatherForecast/Get";

            // Arrange 获取服务
            using var server = await TestHost.GetTestHost().StartAsync();

            // Action 发起接口请求
            var response = server.GetTestClientNoToken().GetAsync(needRequstUrl);

            //Assert 接口状态码是401
            Assert.Equal(HttpStatusCode.Unauthorized, response.Result.StatusCode);


            var responseWithToken = server.GetTestClientWithToken()
               .GetAsync(needRequstUrl);
            // Assert 确保接口状态码是200
            Assert.Equal(HttpStatusCode.OK, responseWithToken.Result.StatusCode);

            
        }

    }//Class_end
}
