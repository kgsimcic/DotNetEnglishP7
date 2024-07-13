using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Threading;
using Dot.Net.WebApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public LocalDbContext DbContext { get; }

        public Repository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

/*        public async Task<TEntity> FindByUserName(string userName)
        {
            return await Set().ToAsyncEnumerable()
                .Where(entity => user.UserName == userName)
                                  .FirstOrDefaultAsync();
        }*/

        public TEntity[] GetAll()
        {
            return Set().ToArray();
        }

# nullable enable
        public TEntity? GetById(int id)
        {
            return Set().Find(id);
        }
# nullable disable
        public void Add(TEntity entity)
        {
            Set().Add(entity);
        }

        public void Update(TEntity entity)
        {
            Set().Update(entity);
        }

        public void Delete(TEntity entity) {

            Set().Remove(entity);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public DbSet<TEntity> Set()
        {
            return DbContext.Set<TEntity>();
        }
    }
}