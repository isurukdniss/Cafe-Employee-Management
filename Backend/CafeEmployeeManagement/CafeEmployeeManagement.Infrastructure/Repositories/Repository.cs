using CafeEmployeeManagement.Domain.Interfaces;
using CafeEmployeeManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CafeEmployeeManagement.Infrastructure.Repositories
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id)
            
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                dbContext.Remove(entity);
                await dbContext.SaveChangesAsync();
            } 
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null
            , params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbContext.Set<T>() ;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id));
        }

        public async Task UpdateAsync(T entity)
        {
            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
