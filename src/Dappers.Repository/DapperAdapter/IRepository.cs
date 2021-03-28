using System.Collections.Generic;
using System.Data;

namespace Dappers.Repository
{
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// 通用增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        int Execute(string sql, object param, IDbTransaction trans = null);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        int Insert(T entity, string sql = null, IDbTransaction trans = null);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        int BulkInsert(List<T> entityList, IDbTransaction trans = null);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        int Delete(string KeyValue, IDbTransaction trans = null);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        int Delete(T entity, string sql = null, IDbTransaction trans = null);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        int BulkDelete(List<T> entityList, IDbTransaction trans = null);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        int Update(T entity, string sql = null, IDbTransaction trans = null);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        int BulkUpdate(List<T> entityList, IDbTransaction trans = null);

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> Query(string sql, object param = null, IDbTransaction trans = null);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        T Query(string keyValue, IDbTransaction trans = null);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        T Query(T T, string sql = null, IDbTransaction trans = null);

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> QueryList(IDbTransaction trans = null);

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> QueryList(string sql, string param, IDbTransaction trans = null);

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="sqlArgs"></param>
        /// <returns></returns>
        Page<T> QueryPaged(int pageIndex, int pageSize, string sql, object sqlArgs = null);

    }
}
