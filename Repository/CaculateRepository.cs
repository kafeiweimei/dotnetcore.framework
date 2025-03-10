using IRepository;
using System;

namespace Repository
{
    public class CaculateRepository : ICaculateRepository
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}
