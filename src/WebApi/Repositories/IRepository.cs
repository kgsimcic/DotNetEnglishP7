using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        TEntity[] GetAll();
# nullable enable
        TEntity? GetById(int id);
# nullable disable
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        DbSet<TEntity> Set();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
