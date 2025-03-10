using IRepository.Base;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IAdvertisementRepository: IBaseRepository<Advertisement>
    {
        Task<int> Sum(int i, int j);

        //long Add(Advertisement model);
        //List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);
        //bool Update(Advertisement model);
        //bool Delete(Advertisement model);

    }//Interface_end
}
