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
            // ��ʼ��������
            ICaculateRepository caculateRepository=new CaculateRepository();
            CaculateService caculateService = new CaculateService(caculateRepository);
            //����1 �ж���2��֮���Ƿ���ϣ��ֵ���
            Assert.Equal(6, caculateService.Sum(4,2));
            //����2
            Assert.Equal(16, caculateService.Sum(11, 5));
            //����3
            Assert.Equal(666, caculateService.Sum(333, 333));
        }




    }//Class_end
}
