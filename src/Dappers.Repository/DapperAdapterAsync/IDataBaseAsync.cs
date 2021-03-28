using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dappers.Repository
{
    public interface IDataBaseAsync<T>
    {
        /// <summary>
        /// 通用增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string sql, object param, IDbTransaction trans = null);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity, string sql = null, IDbTransaction trans = null);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        Task<int> BulkInsertAsync(List<T> entityList, IDbTransaction trans = null);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string KeyValue, IDbTransaction trans = null);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        Task<int> DeleteAsync(T entity, string sql = null, IDbTransaction trans = null);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        Task<int> BulkDeleteAsync(List<T> entityList, IDbTransaction trans = null);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity, string sql = null, IDbTransaction trans = null);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        Task<int> BulkUpdateAsync(List<T> entityList, IDbTransaction trans = null);

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync(string sql, object param = null, IDbTransaction trans = null);

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
        Task<IEnumerable<T>> QueryListAsync(IDbTransaction trans = null);

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryListAsync(string sql, object param, IDbTransaction trans = null);

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="sqlArgs"></param>
        /// <returns></returns>
        Task<Page<T>> QueryPagedAsync(int pageIndex, int pageSize, string sql, object sqlArgs = null);
    }
}
