/***
*	Title："WebApi" 项目
*		主题：数据库连接上下文
*	Description：
*		功能：XXX
*	Date：2021
*	Version：0.1版本
*	Author：Coffee
*	Modify Recoder：
*/

using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Repository.Sugar
{
    public class DbContext
    {
        //连接字符串
        private static string _connectionString;
        //数据库类型
        private static DbType _dbType;
        //数据库实例
        private SqlSugarClient _db;


        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public SqlSugarClient Db
        {
            get { return _db; }
            private set { _db = value; }
        }

        /// <summary>
        /// 数据库上下文实例
        /// </summary>
        private static DbContext Context
        {
            get { return new DbContext(); }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        private DbContext()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException("数据库连接字符串为空！");
            }

            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = true,

                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            });
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接</param>
        private DbContext(bool blnIsAutoCloseConnection)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException("数据库连接字符串为空");
            }

            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,

                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            });
        }


        #region 实例方法
        /// <summary>
        /// 获取数据库处理对象
        /// </summary>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(_db);
        }

        /// <summary>
        /// 获取数据库处理对象
        /// </summary>
        /// <param name="db">数据库实例</param>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db) where T : class, new()
        {
            return new SimpleClient<T>(db);
        }

        #region 根据数据库表生产实体类

        /// <summary>
        /// 根据数据库表生成对应实体类
        /// </summary>       
        /// <param name="strPath">实体类存放路径</param>
        public void CreateClassFileByDBTable(string strPath)
        {
            CreateClassFileByDBTable(strPath, "Km.PosZC");
        }

        /// <summary>
        /// 根据数据库表生成实体类
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        public void CreateClassFileByDBTable(string strPath, string strNameSpace)
        {
            CreateClassFileByDBTable(strPath, strNameSpace, null);
        }

        /// <summary>
        /// 根据数据库表生成实体类
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生成指定的表</param>
        public void CreateClassFileByDBTable(
            string strPath,
            string strNameSpace,
            string[] lstTableNames)
        {
            CreateClassFileByDBTable(strPath, strNameSpace, lstTableNames, string.Empty);
        }

        /// <summary>
        /// 根据数据库表生产实体类
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        public void CreateClassFileByDBTable(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool blnSerializable = false)
        {
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                _db.DbFirst.Where(lstTableNames).IsCreateDefaultValue().IsCreateAttribute()
                    .SettingClassTemplate(p => p = @"
                    {using} namespace {Namespace}
                    {
                        {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
                         public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
                            {
                                public {ClassName}()
                                {
                                    {Constructor}
                                }
                                    {PropertyName}
                                }
                            }
                        ")
                    .SettingPropertyTemplate(p => p = @" {SugarColumn}
                         public {PropertyType} {PropertyName}
                         {
                            get
                             {
                                return _{PropertyName};
                             }
                            set
                            {
                                if(_{PropertyName}!=value)
                                {
                                    base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                                }
                                _{PropertyName}=value;
                            }
                    }")
                    .SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
            else
            {
                _db.DbFirst.IsCreateAttribute().IsCreateDefaultValue()
                    .SettingClassTemplate(p => p = @"
                    {using} namespace {Namespace}
                    {
                        {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
                        public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
                        {
                            public {ClassName}()
                            {
                                {Constructor}
                            }
                            {PropertyName}
                        }
                    }
                    ")
                    .SettingPropertyTemplate(p => p = @"
                    {SugarColumn}
                     public {PropertyType} {PropertyName}
                    {
                        get
                        {
                            return _{PropertyName};
                        }
                        set
                        {
                            if(_{PropertyName}!=value)
                            {
                                base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                            }
                            _{PropertyName}=value;
                        }
                     }")
                    .SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
        }

        #endregion

        #region 根据实体类生成数据库表

        /// <summary>
        /// 根据实体类生成数据库表
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntitys) where T : class, new()
        {
            Type[] lstTypes = null;
            if (lstEntitys != null)
            {
                lstTypes = new Type[lstEntitys.Length];
                for (int i = 0; i < lstEntitys.Length; i++)
                {
                    T t = lstEntitys[i];
                    lstTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(blnBackupTable, lstTypes);
        }

        /// <summary>
        /// 根据实体类生成数据库表 
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                _db.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                _db.CodeFirst.InitTables(lstEntitys);
            }
        }

        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 获取一个数据库上下文
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接
        /// （如果为false，则使用接受时需要手动关闭Db）</param>
        /// <returns>返回数据库上下文</returns>
        public static DbContext GetDbContext(bool blnIsAutoCloseConnection = true)
        {
            return new DbContext(blnIsAutoCloseConnection);
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="strConnectionString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void Init(string strConnectionString, DbType dbType = SqlSugar.DbType.SqlServer)
        {
            _connectionString = strConnectionString;
            _dbType = dbType;
        }

        /// <summary>
        /// 创建一个链接配置
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接（true:表示自动）</param>
        /// <param name="blnIsShardSameThread">是否跨类事务（true:表示开启）</param>
        /// <returns>返回连接配置</returns>
        public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true, bool blnIsShardSameThread = false)
        {
            ConnectionConfig config = new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                //IsShardSameThread = blnIsShardSameThread
            };
            return config;
        }

        /// <summary>
        /// 获取一个自定义的DB
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SqlSugarClient GetCustomDB(ConnectionConfig config)
        {
            return new SqlSugarClient(config);
        }


        /// <summary>
        ///获取一个自定义的数据库处理对象
        /// </summary>
        /// <param name="sugarClient">sugarClient</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(SqlSugarClient sugarClient) where T : class, new()
        {
            return new SimpleClient<T>(sugarClient);
        }
        /// <summary>
        /// 获取一个自定义的数据库处理对象
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig config) where T : class, new()
        {
            SqlSugarClient sugarClient = GetCustomDB(config);
            return GetCustomEntityDB<T>(sugarClient);
        }

        #endregion

    }//Class_end

}