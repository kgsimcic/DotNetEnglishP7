using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Threading;

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

        /*public async Task<User> FindByUserName(string userName)
        {
            return await DbContext.Users.ToAsyncEnumerable()
                .Where(user => user.UserName == userName)
                                  .FirstOrDefaultAsync();
        }*/

        public TEntity[] GetAll()
        {
            return Set().ToArray();
        }

        public TEntity? GetById(int id)
        {
            return Set().Find(id);
        }

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

        protected DbSet<TEntity> Set()
        {
            return DbContext.Set<TEntity>()
                ?? throw new InvalidOperationException($"DbSet of type {typeof(TEntity).Name} is not registered");
        }
    }
}