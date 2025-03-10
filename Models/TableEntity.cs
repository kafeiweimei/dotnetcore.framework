/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/17 14:58:19
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// 表实体
    /// </summary>
    /// <typeparam name="T">需返回的数据集合类型</typeparam>
    public class TableEntity<T>
    {

        /// <summary>
        /// 返回编号
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> Datas { get; set; }


    }//Class_end
}
