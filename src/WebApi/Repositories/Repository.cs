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
        public LocalDbContext DbContext { get; set; }

        public Repository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<TEntity[]> GetAll()
        {
            return await Set().ToArrayAsync();
        }

# nullable enable
        public async Task<TEntity?> GetById(int id)
        {
            var findResult = await Set().FindAsync(id);
            if (findResult != null)
            {
                DbContext.Entry(findResult).State = EntityState.Detached;
            }
            return findResult;
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