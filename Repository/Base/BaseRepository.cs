/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/18 22:33:23
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using IRepository.Base;
using Repository.Sugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private DbContext context;
        private SqlSugarClient db;
        private SimpleClient<TEntity> entityDB;

        public DbContext Context
        {
            get { return context; }
            set { context = value; }
        }
        internal SqlSugarClient Db
        {
            get { return db; }
            private set { db = value; }
        }
        internal SimpleClient<TEntity> EntityDB
        {
            get { return entityDB; }
            private set { entityDB = value; }
        }
        public BaseRepository()
        {
            DbContext.Init(BaseDBConfig.connectionString);
            context = DbContext.GetDbContext();
            db = context.Db;
            entityDB = context.GetEntityDB<TEntity>(db);
        }


        /// <summary>
        /// 根据ID查询一条实体数据
        /// </summary>
        /// <param name="objId">Id内容</param>
        /// <returns>返回数据实体</returns>
        public async Task<TEntity> QueryById(object objId)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().InSingle(objId));
            return await db.Queryable<TEntity>().In(objId).SingleAsync();
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="IsUseCache">是否使用缓存</param>
        /// <returns>返回数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool IsUseCache = false)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().WithCacheIF(IsUseCache).InSingle(objId));
            return await db.Queryable<TEntity>().WithCacheIF(IsUseCache, 10).In(objId).SingleAsync();
        }

        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="listIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>返回数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIds(object[] listIds)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().In(listIds).ToList());
            return await db.Queryable<TEntity>().In(listIds).ToListAsync();
        }

        /// <summary>
        /// 添加实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回数据库受影响的行数（1表示添加成功）</returns>
        public async Task<int> Add(TEntity entity)
        {
            //var i = await Task.Run(() => db.Insertable(entity).ExecuteCommand());
            ////返回的i是long类型,这里你可以根据你的业务需要进行处理（比如我这里转为Int）
            //return (int)i;

            var insert = db.Insertable(entity);

            //这里你可以返回TEntity，这样的话就可以获取id值，无论主键是什么类型
            //var return3 = await insert.ExecuteReturnEntityAsync();

            return await insert.ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列内容</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = db.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnIdentityAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            }
        }

        /// <summary>
        /// 批量添加实体数据
        /// </summary>
        /// <param name="listEntity">实体数据集合</param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await db.Insertable(listEntity.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回结果（true：表示更新成功）</returns>
        public async Task<bool> Update(TEntity entity)
        {
            ////这种方式会以主键为条件
            //var i = await Task.Run(() => _db.Updateable(entity).ExecuteCommand());
            //return i > 0;
            //这种方式会以主键为条件
            return await db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="strWhere">条件</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            // return await Task.Run(() => db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
            return await db.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="strSql">更新的sql语句</param>
        /// <param name="parameters">更新使用的参数</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> Update<SugarParameter>(string strSql, SugarParameter[] parameters = null)
        {
            //return await Task.Run(() => db.Ado.ExecuteCommand(strSql, parameters) > 0);
            return await db.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
        }

        /// <summary>
        /// 更新实体列表数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="listColumns">查询的列表列</param>
        /// <param name="listIgnoreColumns">需要忽略的列表列</param>
        /// <param name="strWhere">条件</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> Update(
          TEntity entity,
          List<string> listColumns = null,
          List<string> listIgnoreColumns = null,
          string strWhere = ""
            )
        {
            IUpdateable<TEntity> up = db.Updateable(entity);
            if (listIgnoreColumns != null && listIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(listIgnoreColumns.ToArray());
            }
            if (listColumns != null && listColumns.Count > 0)
            {
                up = up.UpdateColumns(listColumns.ToArray());
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = up.Where(strWhere);
            }
            return await up.ExecuteCommandHasChangeAsync();
        }


        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> DeleteById(object id)
        {
            //var i = await Task.Run(() => db.Deleteable<TEntity>(id).ExecuteCommand());
            //return i > 0;
            return await db.Deleteable<TEntity>().In(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> Delete(TEntity entity)
        {
            //var i = await Task.Run(() => db.Deleteable(entity).ExecuteCommand());
            //return i > 0;
            return await db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            //var i = await Task.Run(() => db.Deleteable<TEntity>().In(ids).ExecuteCommand());
            //return i > 0;
            return await db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }



        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>返回所有数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            //return await Task.Run(() => entityDB.GetList());
            return await db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>返回条件对应的数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 根据条件表达式查询数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>返回条件表达式对应的数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            //return await Task.Run(() => entityDB.GetList(whereExpression));
            return await db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 根据条件表达式查询数据，且指定字段排序
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList());
            return await db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(strOrderByFileds != null, strOrderByFileds).ToListAsync();
        }


        /// <summary>
        /// 根据条件表达式查询数据，且指定字段排序
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否升序（true表示升序）</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
            return await db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 根据条件查询数据，且指定字段排序
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            // return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }


        /// <summary>
        /// 查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList());
            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 查询前N条数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(
            string strWhere,
            int intTop,
            string strOrderByFileds)
        {
            // return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList());
            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// //根据sql语句和参数查询数据
        /// </summary>
        /// <typeparam name="SugarParameter"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async Task<List<TEntity>> QuerySql<SugarParameter>(string sql, SugarParameter[] parameters = null)
        {
            return await db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 根据sql语句和参数查询数据
        /// </summary>
        /// <typeparam name="SugarParameter"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async Task<DataTable> QueryTable<SugarParameter>(string sql, SugarParameter[] parameters = null)
        {
            return await db.Ado.GetDataTableAsync(sql, parameters);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageList(intPageIndex, intPageSize));
            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                     .WhereIF(whereExpression != null, whereExpression).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(
          string strWhere,
          int intPageIndex,
          int intPageSize,
          string strOrderByFileds)
        {
            //return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageList(intPageIndex, intPageSize));

            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                   .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小（默认20）</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc </param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression,
        int intPageIndex = 0, int intPageSize = 20, string strOrderByFileds = null)
        {
            return await Task.Run(() => db.Queryable<TEntity>()
            .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageList(intPageIndex, intPageSize));

            //RefAsync<int> totalCount = 0;
            //var list = await db.Queryable<TEntity>()
            // .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
            // .WhereIF(whereExpression != null, whereExpression)
            // .ToPageListAsync(intPageIndex, intPageSize, totalCount);

            //return new PageModel<TEntity>(intPageIndex, totalCount, intPageSize, list);
        }



    }//Class_end
}
