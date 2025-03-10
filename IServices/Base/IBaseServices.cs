using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Base
{
    public interface IBaseServices<TEntity> where TEntity : class
    {
        // 根据ID查询一条实体数据
        Task<TEntity> QueryById(object objId);
        // 根据ID查询一条实体数据(是否缓存)
        Task<TEntity> QueryById(object objId, bool IsUseCache = false);
        //根据ID列表查询对应的所有数据
        Task<List<TEntity>> QueryByIds(object[] listIds);


        //添加实体数据
        Task<int> Add(TEntity entity);
        //只插入实体数据的指定列数据
        Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        //批量添加实体数据
        Task<int> Add(List<TEntity> listEntity);



        //更新一条实体数据
        Task<bool> Update(TEntity entity);
        //根据条件更新一条实体数据
        Task<bool> Update(TEntity entity, string strWhere);
        //根据条件更新一条实体数据
        Task<bool> Update<TParameter>(string strSql, TParameter[] parameters = null);
        //更新实体列表数据（根据显示列和忽略列）
        Task<bool> Update(TEntity entity, List<string> listColumns = null,
            List<string> listIgnoreColumns = null, string strWhere = null);


        //根据ID删除一条数据
        Task<bool> DeleteById(object id);
        //根据实体删除一条数据
        Task<bool> Delete(TEntity entity);
        // 删除指定ID集合的数据(批量删除)
        Task<bool> DeleteByIds(object[] ids);


        //查询所有数据
        Task<List<TEntity>> Query();
        //根据条件查询数据
        Task<List<TEntity>> Query(string strWhere);
        //根据条件表达式查询数据
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);
        //根据条件表达式查询数据，且指定字段排序
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
        //根据条件表达式查询数据，且指定字段排序
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        //根据条件查询数据，且指定字段排序
        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);
        //查询前N条数据
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);
        //查询前N条数据
        Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);
        //根据sql语句和参数查询数据
        Task<List<TEntity>> QuerySql<TParameter>(string sql, TParameter[] parameters = null);
        //根据sql语句和参数查询数据
        Task<DataTable> QueryTable<TParameter>(string sql, TParameter[] parameters = null);

        //分页查询
        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds);
        //分页查询
        Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);
        //分页查询
        Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 0, int intPageSize = 20, string strOrderByFileds = null);




    }//Interface_end
}
