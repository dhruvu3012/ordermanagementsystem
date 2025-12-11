namespace OrderManagemenSystem.Data.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<IQueryable<T>> GetAllAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task InsertAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
