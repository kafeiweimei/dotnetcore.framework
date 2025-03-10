using IRepository;
using Repository;
using Services;
using System;
using Xunit;

namespace Tests
{
    public class AdvertisementServiceShould
    {
        [Fact]
        public void TestSum()
        {
            // 开始测试用例
            ICaculateRepository caculateRepository=new CaculateRepository();
            CaculateService caculateService = new CaculateService(caculateRepository);
            //断言1 判断求2数之和是否与希望值相等
            Assert.Equal(6, caculateService.Sum(4,2));
            //断言2
            Assert.Equal(16, caculateService.Sum(11, 5));
            //断言3
            Assert.Equal(666, caculateService.Sum(333, 333));
        }




    }//Class_end
}
