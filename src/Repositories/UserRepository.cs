using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {
        public LocalDbContext DbContext { get; }

        public UserRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public User FindByUserName(string userName)
        {
            return DbContext.Users.Where(user => user.UserName == userName)
                                  .FirstOrDefault();
        }

        public User[] FindAll()
        {
            return DbContext.Users.ToArray();
        }

        public void Add(User user)
        {
            DbContext.Users.Add(user);
        }

        public User FindById(int id)
        {
            return DbContext.Users.Where(user => id == user.Id).FirstOrDefault();
        }

        public void Update(User user)
        {
            DbContext.Users.Update(user);
        }

        public void Delete(int id) {
            var userToDelete = DbContext.Users.Where(user =>user.Id == id).FirstOrDefault();
            if (userToDelete != null)
            {
                DbContext.Users.Remove(userToDelete);
            }
        }
    }
}