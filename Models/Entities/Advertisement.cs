/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/17 17:23:14
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class Advertisement
    {
        /// <summary>
        /// ID
        /// </summary>
        //[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }

        /// <summary>
        /// 广告图片
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 广告标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 广告链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createdate { get; set; } = DateTime.Now;

    }
}
