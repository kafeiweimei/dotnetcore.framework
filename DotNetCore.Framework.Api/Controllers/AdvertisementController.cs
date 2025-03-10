using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Framework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementServices advertisementServices;

        public AdvertisementController(IAdvertisementServices advertisementServices)
        {
            this.advertisementServices = advertisementServices;
        }

        /// <summary>
        /// 广告的Sum方法
        /// </summary>
        /// <param name="i">参数i</param>
        /// <param name="j">参数j</param>
        /// <returns></returns>
        [HttpGet("{i},{j}")]
        public async Task<int> GetSum(int i, int j)
        {
            return await advertisementServices.Sum(i, j);
        }


        /// <summary>
        /// 添加广告(异步)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddInfo(Advertisement entity)
        {
            return await advertisementServices.Add(entity);
        }


        /// <summary>
        /// 根据ID查询信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Advertisement>> GetInfoById(int id)
        {
            return await advertisementServices.Query(d => d.Id == id);
        }

        ///<summary>
        /// 更新广告
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdateInfo(Advertisement entity)
        {
            return await advertisementServices.Update(entity);
        }

        /// <summary>
        /// 删除指定广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteInfo(int id)
        {
            return await advertisementServices.DeleteById(id);
        }

    }//Class_end
}
