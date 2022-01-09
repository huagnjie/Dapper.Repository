using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Repository
{
    internal class RepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T> where T : class, new()
    {
        private readonly IDataBaseAsync<T> db;

        public RepositoryAsync(string connString, DatabaseType dt = DatabaseType.SqlServer)
        {
            this.db = GetDataAdapterAsync(connString, dt);
        }
        
        /// <summary>
        /// 通用增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, object param, IDbTransaction trans = null)
        {
            return await this.db.ExecuteAsync(sql, param, trans);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(T entity, string sql = null, IDbTransaction trans = null)
        {
            return await this.db.InsertAsync(entity, sql, trans);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        public async Task<int> BulkInsertAsync(List<T> entityList, IDbTransaction trans = null)
        {
            return await this.db.BulkInsertAsync(entityList, trans);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string KeyValue, IDbTransaction trans = null)
        {
            return await this.db.DeleteAsync(KeyValue, trans);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(T entity, string sql = null, IDbTransaction trans = null)
        {
            return await this.db.DeleteAsync(entity, sql, trans);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public async Task<int> BulkDeleteAsync(List<T> entityList, IDbTransaction trans = null)
        {
            return await this.db.BulkDeleteAsync(entityList, trans);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T entity, string sql = null, IDbTransaction trans = null)
        {
            return await this.db.UpdateAsync(entity, sql, trans);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public async Task<int> BulkUpdateAsync(List<T> entityList, IDbTransaction trans = null)
        {
            return await this.db.BulkUpdateAsync(entityList, trans);
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync(string sql, object param = null, IDbTransaction trans = null)
        {
            return await this.db.QueryAsync(sql, param, trans);
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        public T Query(string keyValue, IDbTransaction trans = null)
        {
            return this.db.Query(keyValue, trans);
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        public T Query(T T, string sql = null, IDbTransaction trans = null)
        {
            return this.db.Query(T, sql, trans);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryListAsync(IDbTransaction trans = null)
        {
            return await this.db.QueryListAsync(trans);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryListAsync(string sql, object param, IDbTransaction trans = null)
        {
            return await this.db.QueryListAsync(sql, param, trans);
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="sqlArgs"></param>
        /// <returns></returns>
        public async Task<Page<T>> QueryPagedAsync(int pageIndex, int pageSize, string sql, object sqlArgs = null)
        {
            return await this.db.QueryPagedAsync(pageIndex, pageSize, sql, sqlArgs);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
