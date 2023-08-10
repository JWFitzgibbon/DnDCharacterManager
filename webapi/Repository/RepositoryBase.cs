using DnDAPI.Contracts;
using DnDAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DnDAPI.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly RepositoryContext _context;
        internal DbSet<T> dbSet;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public async Task Create(T entity)
        {
            await _context.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.Where(filter);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
