/***
*	Title："XXX" 项目
*		主题：XXX
*	Description：
*		功能：XXX
*	Date：2022/7/18 23:37:54
*	Version：0.1版本
*	Author：XXX
*	Modify Recoder：
*/

using IRepository.Base;
using IServices.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Base
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> baseRepository;
        //public IBaseRepository<TEntity> baseRepository = new BaseRepository<TEntity>();//通过在子类的构造函数中注入，这里是基类，不用构造函数
        public BaseServices(IBaseRepository<TEntity> baseRepository)
        {
            this.baseRepository = baseRepository;
        }
        /// <summary>
        /// 根据Id查询一条数据
        /// </summary>
        /// <param name="objId">查询的Id</param>
        /// <returns></returns>
        public async Task<TEntity> QueryById(object objId)
        {
            return await baseRepository.QueryById(objId);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="IsUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool IsUseCache = false)
        {
            return await baseRepository.QueryById(objId, IsUseCache);
        }

        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIds(object[] listIds)
        {
            return await baseRepository.QueryByIds(listIds);
        }



        /// <summary>
        /// 添加实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回数据库受影响的行数（1表示添加成功）</returns>
        public async Task<int> Add(TEntity entity)
        {
            return await baseRepository.Add(entity);
        }

        /// <summary>
        /// 添加实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列内容</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            return await baseRepository.Add(entity,insertColumns);
        }

        /// <summary>
        /// 批量添加实体数据
        /// </summary>
        /// <param name="listEntity">实体数据集合</param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await baseRepository.Add(listEntity);
        }



        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            return await baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="strWhere">条件</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            return await baseRepository.Update(entity, strWhere);
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="strSql">更新的sql语句</param>
        /// <param name="parameters">更新使用的参数</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> Update<SugarParameter>(string strSql, SugarParameter[] parameters = null)
        {
            return await baseRepository.Update(strSql,parameters);
        }

        /// <summary>
        /// 更新实体数据
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
            return await baseRepository.Update(entity, listColumns, listIgnoreColumns, strWhere);
        }



        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> Delete(TEntity entity)
        {
            return await baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> DeleteById(object id)
        {
            return await baseRepository.DeleteById(id);
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await baseRepository.DeleteByIds(ids);
        }



        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>返回所有数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            return await baseRepository.Query();
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>返回条件对应的数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            return await baseRepository.Query(strWhere);
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>返回条件表达式对应的数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await baseRepository.Query(whereExpression);
        }

        /// <summary>
        /// 根据条件表达式查询数据，且指定字段排序
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await baseRepository.Query(whereExpression, strOrderByFileds);
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
            return await baseRepository.Query(whereExpression, orderByExpression, isAsc);
        }

        /// <summary>
        /// 根据条件查询数据，且指定字段排序
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await baseRepository.Query(strWhere, strOrderByFileds);
        }

        /// <summary>
        /// 查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段和关键字【排序字段 排序关键字】，如：age desc</param>
        /// <returns>返回对应的实体数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds)
        {
            return await baseRepository.Query(whereExpression, intTop, strOrderByFileds);
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
            return await baseRepository.Query(strWhere, intTop, strOrderByFileds);
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
            return await baseRepository.QuerySql(sql, parameters);
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
            return await baseRepository.QueryTable(sql, parameters);
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
            return await baseRepository.Query(
              whereExpression,
              intPageIndex,
              intPageSize,
              strOrderByFileds);
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
            return await baseRepository.Query(
            strWhere,
            intPageIndex,
            intPageSize,
            strOrderByFileds);
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
            return await baseRepository.QueryPage(whereExpression,
         intPageIndex = 0, intPageSize, strOrderByFileds);
        }



    }//Class_end
}
