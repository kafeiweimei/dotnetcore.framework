using IRepository;
using IServices;
using System;

namespace Services
{
    public class CaculateService : ICaculateService
    {
        private readonly ICaculateRepository caculateRepository;

        public CaculateService(ICaculateRepository caculateRepository)
        {
            this.caculateRepository = caculateRepository;
        }

        public int Sum(int a, int b)
        {
            var result = caculateRepository.Sum(a,b);
            return result;
        }
    }
}
