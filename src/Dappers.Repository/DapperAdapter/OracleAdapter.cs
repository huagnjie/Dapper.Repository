using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dappers.Repository
{
    public class OracleAdapter<T> : BaseRepository<T>, IDataBase<T> where T : class, new()
    {
        public OracleAdapter(string connString)
        {
            sqlConn = connString;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int Insert(T entity, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var createSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetCreateSql<T>() : sql;
            return conn.Execute(createSql, entity, trans);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        public int BulkInsert(List<T> entityList, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string sqlnew = BaseMethodUtility.GetRoutineCreateSql(entityList);
            return conn.Execute(sqlnew, trans);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        public int Delete(string KeyValue, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var deleteSql = BaseMethodUtility.GetDeleteSql<T>(KeyValue);
            return conn.Execute(deleteSql, trans);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int Delete(T entity, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var deleteSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetDeleteSql<T>() : sql;
            return conn.Execute(deleteSql, entity, trans);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public int BulkDelete(List<T> entityList, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string deleteSql = BaseMethodUtility.GetDeleteSql<T>();
            return conn.Execute(deleteSql, entityList, trans);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int Update(T entity, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var updateSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetUpdateSql<T>() : sql;
            return conn.Execute(updateSql, entity, trans);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public int BulkUpdate(List<T> entityList, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string updateSql = BaseMethodUtility.GetUpdateSql<T>();
            return conn.Execute(updateSql, entityList, trans);
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        public T Query(string keyValue, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string querySql = BaseMethodUtility.GetQuerySql<T>(keyValue);
            return conn.Query<T>(querySql, trans).SingleOrDefault();
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        public T Query(T T, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string selectSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetQuerySql<T>() : sql;
            return conn.Query<T>(selectSql, T, trans).SingleOrDefault();
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> QueryList(IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string querySql = BaseMethodUtility.GetQueryListSql<T>();
            return conn.Query<T>(querySql, trans);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> QueryList(string sql, string param, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            return conn.Query<T>(sql, param, trans);
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
            if (pageSize < 1 || pageSize > 50000) throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex));
            var partedSql = PagingUtil.SplitSql(sql);
            sql = PagingBuild(ref partedSql, sqlArgs, (pageIndex - 1) * pageSize, pageSize);
            var sqlCount = PagingUtil.GetCountSql(partedSql);
            var conn = GetConnection();
            var totalCount = conn.ExecuteScalar<int>(sqlCount, sqlArgs);
            var items = conn.Query<T>(sql, sqlArgs);
            var pagedList = new Page<T>(items.ToList(), pageIndex - 1, pageSize, totalCount);
            return pagedList;
        }
    }
}
