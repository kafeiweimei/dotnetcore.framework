/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/17 17:40:29
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using IRepository;
using Models.Entities;
using Repository.Sugar;
using SqlSugar;
using Repository.Base;
using System.Threading.Tasks;

namespace Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>,IAdvertisementRepository
    {
        public AdvertisementRepository()
        {
            
        }

        async Task<int> IAdvertisementRepository.Sum(int i, int j)
        {
            int result = await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
                return i + j;
            });

            return result;

        }


        
    }//Class_end
}
