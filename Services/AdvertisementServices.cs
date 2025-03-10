/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/18 21:36:15
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IRepository;
using IRepository.Base;
using IServices;
using Models.Entities;
using Services.Base;

namespace Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        private readonly IAdvertisementRepository advertisementRepository;

        public AdvertisementServices(IAdvertisementRepository dal):base(dal)
        {
            this.advertisementRepository = dal;
        }


        public Task<int> Sum(int i, int j)
        {
           return advertisementRepository.Sum(i, j);
        }



        //public long Add(Advertisement model)
        //{
        //    return dal.Add(model);
        //}

        //public bool Delete(Advertisement model)
        //{
        //    return dal.Delete(model);
        //}

        //public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        //{
        //    return dal.Query(whereExpression);
        //}

        //public bool Update(Advertisement model)
        //{
        //    return dal.Update(model);
        //}

    }//Class_end
}
