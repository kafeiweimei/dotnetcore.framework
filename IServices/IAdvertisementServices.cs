/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/18 21:27:21
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using IServices.Base;
using System.Threading.Tasks;

namespace IServices
{
    public interface IAdvertisementServices:IBaseServices<Advertisement>
    {
        Task<int> Sum(int i, int j);

        //long Add(Advertisement model);
        //List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);
        //bool Update(Advertisement model);
        //bool Delete(Advertisement model);


    }//Interface_end
}
