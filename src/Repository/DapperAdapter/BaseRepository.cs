using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    /// <summary>
    /// 封装仓库
    /// </summary>
    internal class BaseRepository<T>
    {
        protected string sqlConn { get; set; }

        /// <summary>
        /// 通用增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual int Execute(string sql, object param, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            return conn.Execute(sql, param, trans);
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Query(string sql, object param = null, IDbTransaction trans = null)
        {
            var conn = GetConnection();
            return conn.Query<T>(sql, param, trans);
        }

        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="partedSql"></param>
        /// <param name="sqlArgs"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public virtual string PagingBuild(ref PartedSql partedSql, object sqlArgs, long skip, long take)
        {
            if (string.IsNullOrEmpty(partedSql.OrderBy))
                throw new InvalidOperationException("miss order by");
            var hasDistinct = partedSql.Select.IndexOf("DISTINCT", StringComparison.OrdinalIgnoreCase) == 0;
            var select = "SELECT";
            if (hasDistinct)
            {
                partedSql.Select = partedSql.Select.Substring("DISTINCT".Length);
                select = "SELECT DISTINCT";
            }
            if (skip <= 0)
            {
                var subSql = StringBuilderCache.Allocate().AppendFormat("{0} TOP {1} {2}", select, take, partedSql.Select).Append(" FROM ").Append(partedSql.Body).Append(" ORDER BY ").Append(partedSql.OrderBy);
                return StringBuilderCache.ReturnAndFree(subSql);
            }
            else
            {
                var subSql = StringBuilderCache.Allocate().AppendFormat("SELECT * FROM (SELECT {0}, ROW_NUMBER() OVER " + "(ORDER BY {1}) AS ROWNUM FROM {2}) AS ROWCONSTRAINEDRESULT " +
                    "WHERE ROWNUM > {3} AND ROWNUM <= {4}", partedSql.Select, partedSql.OrderBy, partedSql.Body, skip, skip + take);
                return StringBuilderCache.ReturnAndFree(subSql);
            }
        }

        /// <summary>
        /// 获取Conn
        /// </summary>
        /// <returns></returns>
        public virtual IDbConnection GetConnection()
        {
            return new SqlConnection(sqlConn);
        }
    }
}
