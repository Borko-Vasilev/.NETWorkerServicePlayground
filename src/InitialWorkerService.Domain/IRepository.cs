using System.Linq.Expressions;

namespace InitialWorkerService.Domain
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");

        Task<IEnumerable<T>> GetAll(string includeProperties = "");

        Task<T> GetById(int id);

        Task<T> GetById(Expression<Func<T, bool>> filter, string includeProperties = "");

        Task Update(T entityToUpdate);

        Task Add(T entity);

        Task Delete(T entity);

        Task Delete(object id);
    }
}
