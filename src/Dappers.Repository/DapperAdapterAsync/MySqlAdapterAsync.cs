﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Dappers.Repository
{
    internal class MySqlAdapterAsync<T> : BaseRepositoryAsync<T>, IDataBaseAsync<T> where T : class, new()
    {
        public MySqlAdapterAsync(string connString)
        {
            sqlConn = connString;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(T entity, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var createSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetCreateSql<T>() : sql;
            return await conn.ExecuteAsync(createSql, entity, trans);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        public async Task<int> BulkInsertAsync(List<T> entityList, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string sqlnew = BaseMethodUtility.GetRoutineCreateSql(entityList);
            return await conn.ExecuteAsync(sqlnew, trans);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string KeyValue, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var deleteSql = BaseMethodUtility.GetDeleteSql<T>(KeyValue);
            return await conn.ExecuteAsync(deleteSql, trans);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(T entity, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var deleteSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetDeleteSql<T>() : sql;
            return await conn.ExecuteAsync(deleteSql, entity, trans);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public async Task<int> BulkDeleteAsync(List<T> entityList, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string deleteSql = BaseMethodUtility.GetDeleteSql<T>();
            return await conn.ExecuteAsync(deleteSql, entityList, trans);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T entity, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            var updateSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetUpdateSql<T>() : sql;
            return await conn.ExecuteAsync(updateSql, entity, trans);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public async Task<int> BulkUpdateAsync(List<T> entityList, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string updateSql = BaseMethodUtility.GetUpdateSql<T>();
            return await conn.ExecuteAsync(updateSql, entityList, trans);
        }


        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        public T Query(string keyValue, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string querySql = BaseMethodUtility.GetQuerySql<T>(keyValue);
            var entity = conn.Query<T>(querySql, trans).SingleOrDefault();
            return entity;
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <returns></returns>
        public T Query(T T, string sql = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string selectSql = string.IsNullOrWhiteSpace(sql) ? BaseMethodUtility.GetQuerySql<T>() : sql;
            var entity = conn.Query<T>(selectSql, T, trans).SingleOrDefault();
            return entity;
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryListAsync(IDbTransaction trans = null)
        {
            var conn = GetConnection();
            string querySql = BaseMethodUtility.GetQueryListSql<T>();
            return await conn.QueryAsync<T>(querySql, trans);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryListAsync(string sql, object param, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            return await conn.QueryAsync<T>(sql, param, trans);
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
            if (pageSize < 1 || pageSize > 50000) throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex));
            var partedSql = PagingUtil.SplitSql(sql);
            sql = PagingBuild(ref partedSql, sqlArgs, (pageIndex - 1) * pageSize, pageSize);
            var sqlCount = PagingUtil.GetCountSql(partedSql);
            var conn = GetConnection();
            var totalCount = await conn.ExecuteScalarAsync<int>(sqlCount, sqlArgs);
            var items = await conn.QueryAsync<T>(sql, sqlArgs);
            var pagedList = new Page<T>(items.ToList(), pageIndex - 1, pageSize, totalCount);
            return pagedList;
        }

        /// <summary>
        /// 返回分页Sql
        /// </summary>
        /// <param name="partedSql"></param>
        /// <param name="sqlArgs"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public override string PagingBuild(ref PartedSql partedSql, object sqlArgs, long skip, long take)
        {
            var pageSql = $"{partedSql.Raw} LIMIT {take} OFFSET {skip}";
            return pageSql;
        }
    }
}
