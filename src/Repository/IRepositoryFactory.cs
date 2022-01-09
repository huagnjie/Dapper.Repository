namespace Repository
{
    /// <summary>
    /// 定义仓储模型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryFactory<T> where T : class, new()
    {
        /// <summary>
        /// 定义仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        IRepository<T> BaseRepository(string connString, DatabaseType dt);

        /// <summary>
        /// 定义仓储（基础库）
        /// </summary>
        /// <returns></returns>
        IRepository<T> BaseRepository();

        /// <summary>
        /// 定义异步仓储
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        IRepositoryAsync<T> BaseRepositoryAsync(string connString, DatabaseType dt);

        /// <summary>
        /// 定义异步仓储（基础库）
        /// </summary>
        /// <returns></returns>
        IRepositoryAsync<T> BaseRepositoryAsync();
    }
}
