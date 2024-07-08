using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        TEntity[] GetAll();
        TEntity GetbyId(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        DbSet<TEntity> Set();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
