using System;
using System.Collections.Generic;
using System.Data;

namespace Repository
{
    internal class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class, new()
    {
        private readonly IDataBase<T> db;

        public Repository(string connString, DatabaseType dt = DatabaseType.SqlServer)
        {
            this.db = GetDataAdapter(connString, dt);
        }

        /// <summary>
        /// 通用增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public int Execute(string sql, object param, IDbTransaction trans = null)
        {
            return this.db.Execute(sql, param, trans);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int Insert(T entity, string sql = null, IDbTransaction trans = null)
        {
            return this.db.Insert(entity, sql, trans);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        public int BulkInsert(List<T> entityList, IDbTransaction trans = null)
        {
            return this.db.BulkInsert(entityList, trans);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        public int Delete(string KeyValue, IDbTransaction trans = null)
        {
            return this.db.Delete(KeyValue, trans);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int Delete(T entity, string sql = null, IDbTransaction trans = null)
        {
            return this.db.Delete(entity, sql, trans);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public int BulkDelete(List<T> entityList, IDbTransaction trans = null)
        {
            return this.db.BulkDelete(entityList, trans);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int Update(T entity, string sql = null, IDbTransaction trans = null)
        {
            return this.db.Update(entity, sql, trans);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public int BulkUpdate(List<T> entityList, IDbTransaction trans = null)
        {
            return this.db.BulkUpdate(entityList, trans);
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(string sql, object param = null, IDbTransaction trans = null)
        {
            return this.db.Query(sql, param, trans);
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
        public IEnumerable<T> QueryList(IDbTransaction trans = null)
        {
            return this.db.QueryList(trans);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> QueryList(string sql, string param, IDbTransaction trans = null)
        {
            return this.db.QueryList(sql, param, trans);
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="sqlArgs"></param>
        /// <returns></returns>
        public Page<T> QueryPaged(int pageIndex, int pageSize, string sql, object sqlArgs = null)
        {
            return this.db.QueryPaged(pageIndex, pageSize, sql, sqlArgs);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
