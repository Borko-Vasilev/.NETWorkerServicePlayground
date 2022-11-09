using InitialWorkerService.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InitialWorkerService.Repository.EFCore
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;

        protected readonly AppDbContext DbContext;

        protected BaseRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAll(string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetById(int id)
            => await dbSet.FindAsync(id);

        public async Task<T> GetById(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            query = query.Where(filter);

            return await query.SingleOrDefaultAsync();
        }


        public async Task Add(T entity)
            => await dbSet.AddAsync(entity);

        public Task Update(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            DbContext.Entry(entityToUpdate).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);

            return Task.CompletedTask;
        }

        public async Task Delete(object id)
        {
            T entityToDelete = await dbSet.FindAsync(id);
            await Delete(entityToDelete);
        }
    }
}
