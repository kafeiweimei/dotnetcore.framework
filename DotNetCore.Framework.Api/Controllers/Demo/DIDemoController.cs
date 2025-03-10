using Demo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Framework.Api.Controllers.Demo
{
    [Route("api/[controller]")]
    [ApiController]
    public class DIDemoController : ControllerBase
    {
        private ISingletonTest singletonTest; IScopedTest scopedTest; ITransientTest transientTest;
        private readonly IAService aService;

        public DIDemoController(ISingletonTest singletonTest, IScopedTest scopedTest, ITransientTest transientTest,IAService aService)
        {
            this.singletonTest = singletonTest;
            this.scopedTest = scopedTest;
            this.transientTest = transientTest;
            this.aService = aService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> DITest()
        {
            singletonTest.Age = 18;
            singletonTest.Name = "张劲松";


            scopedTest.Age = 19;
            scopedTest.Name = "李思雨";

            transientTest.Age = 20;
            transientTest.Name = "王帆";


            Console.WriteLine("------第二阶段（调用接口的方法）------");
            Console.WriteLine($"Singleton:{JsonConvert.SerializeObject(singletonTest)}");
            Console.WriteLine($"Scoped:{JsonConvert.SerializeObject(scopedTest)}");
            Console.WriteLine($"Transient:{JsonConvert.SerializeObject(transientTest)}");

            aService.MethodTest();

            Console.WriteLine($"------调用服务的方法结束------");
            Console.WriteLine();
            return new string[] {"测试依赖注入控制反转示例"};


        }

    }//Class_end
}
