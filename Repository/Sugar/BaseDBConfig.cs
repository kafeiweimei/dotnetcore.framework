/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/17 18:01:43
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Sugar
{
    public class BaseDBConfig
    {
        //public static string connectionString = "server=127.0.0.1;uid=test;pwd=123456;database=WebApiTest";
        public static string connectionString = AppSettingsHelper.App(new string[] { "DBSettings", "SqlServerConStr" });


    }//Class_end
}
