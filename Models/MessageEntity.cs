/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/17 14:54:11
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
    /// 消息实体
    /// </summary>
    /// <typeparam name="T">需返回的数据集合类型</typeparam>
    public class MessageEntity<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Response { get; set; }


    }//Class_end
}
