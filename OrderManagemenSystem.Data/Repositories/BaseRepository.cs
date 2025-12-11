using Microsoft.EntityFrameworkCore;
using OrderManagemenSystem.Data.Contracts;
using OrderManagemenSystem.Data.Entities;

namespace OrderManagemenSystem.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly OMSContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(OMSContext oMSContext)
        {
            _context = oMSContext;
            _dbSet = _context.Set<T>();
        }

        public async Task DeleteAsync(int id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
            }
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            await Task.Delay(0);
            return _dbSet;
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Delay(0);
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
