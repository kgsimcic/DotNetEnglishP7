using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {
        public LocalDbContext DbContext { get; }

        public UserRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<User> FindByUserName(string userName)
        {
            return await DbContext.Users.ToAsyncEnumerable()
                .Where(user => user.UserName == userName)
                                  .FirstOrDefault();
        }

        public User[] FindAll()
        {
            return DbContext.Users.ToArray();
        }

        public async Task<User> Create(User user)
        {
            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> FindById(int id)
        {
            return await DbContext.Users.ToAsyncEnumerable()
                .Where(user => id == user.Id).FirstOrDefault();
        }

        public async Task<int> Update(User user)
        {
            // check that the user exists before updating them.
            var userToUpdate = DbContext.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            if (userToUpdate != null)
            {
                DbContext.Users.Update(user);
            }

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) {

            var userToDelete = DbContext.Users.Where(user =>user.Id == id).FirstOrDefault();
            if (userToDelete != null)
            {
                DbContext.Users.Remove(userToDelete);
            }
            return await DbContext.SaveChangesAsync();
        }
    }
}