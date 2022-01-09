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
            return new Repository<T>(connString, dt);
        }

        /// <summary>
        /// 定义仓储（基础库）
        /// </summary>
        /// <returns></returns>
        public IRepository<T> BaseRepository()
        {
            return new Repository<T>("Base");
        }

        /// <summary>
        /// 定义异步仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public IRepositoryAsync<T> BaseRepositoryAsync(string connString, DatabaseType dt)
        {
            return new RepositoryAsync<T>(connString, dt);
        }

        /// <summary>
        /// 定义异步仓储（基础库）
        /// </summary>
        /// <returns></returns>
        public IRepositoryAsync<T> BaseRepositoryAsync()
        {
            return new RepositoryAsync<T>("Base");
        }
    }
}
