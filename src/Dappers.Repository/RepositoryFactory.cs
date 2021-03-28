namespace Dappers.Repository
{
    /// <summary>
    /// 定义仓储模型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryFactory<T> where T : class, new()
    {
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public IRepository<T> BaseRepository(string connString,DatabaseType dt)
        {
            switch (dt)
            {
                case DatabaseType.SqlServer:
                    return new Repository<T>(new SqlAdapter<T>(connString));
                case DatabaseType.MySql:
                    return new Repository<T>(new MySqlAdapter<T>(connString));
                case DatabaseType.Oracle:
                    return new Repository<T>(new OracleAdapter<T>(connString));
                default:
                    return null;
            }
        }

        /// <summary>
        /// 定义仓储（基础库）
        /// </summary>
        /// <returns></returns>
        public IRepository<T> BaseRepository()
        {
            return new Repository<T>(new SqlAdapter<T>("Data Source=.;Initial Catalog=Jie_Data;Integrated Security=True;"));
        }

        /// <summary>
        /// 定义异步仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public IRepositoryAsync<T> BaseRepositoryAsync(string connString, DatabaseType dt)
        {
            switch (dt)
            {
                case DatabaseType.SqlServer:
                    return new RepositoryAsync<T>(new SqlAdapterAsync<T>(connString));
                case DatabaseType.MySql:
                    return new RepositoryAsync<T>(new MySqlAdapterAsync<T>(connString));
                case DatabaseType.Oracle:
                    return new RepositoryAsync<T>(new OracleAdapterAsync<T>(connString));
                default:
                    return null;
            }
        }

        /// <summary>
        /// 定义异步仓储（基础库）
        /// </summary>
        /// <returns></returns>
        public IRepositoryAsync<T> BaseRepositoryAsync()
        {
            return new RepositoryAsync<T>(new SqlAdapterAsync<T>("Data Source=.;Initial Catalog=Jie_Data;Integrated Security=True;"));
        }
    }
}
