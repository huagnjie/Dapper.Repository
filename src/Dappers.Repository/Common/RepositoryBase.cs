using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dappers.Repository
{
    public class RepositoryBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取对应数据库类型
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public IDataBase<T> GetDataAdapter(string connString, DatabaseType dt)
        {
            switch (dt)
            {
                case DatabaseType.SqlServer:
                    return new SqlAdapter<T>(connString);
                case DatabaseType.MySql:
                    return new MySqlAdapter<T>(connString);
                case DatabaseType.Oracle:
                    return new OracleAdapter<T>(connString);
                default:
                    return new SqlAdapter<T>(connString);
            }
        }

        /// <summary>
        /// 获取对应数据库类型 - Async
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public IDataBaseAsync<T> GetDataAdapterAsync(string connString, DatabaseType dt)
        {
            switch (dt)
            {
                case DatabaseType.SqlServer:
                    return new SqlAdapterAsync<T>(connString);
                case DatabaseType.MySql:
                    return new MySqlAdapterAsync<T>(connString);
                case DatabaseType.Oracle:
                    return new OracleAdapterAsync<T>(connString);
                default:
                    return new SqlAdapterAsync<T>(connString);
            }
        }
    }
}
