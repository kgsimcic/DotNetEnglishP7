using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository : Repository<User>
    {
 
        public UserRepository(LocalDbContext dbContext):
            base(dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<User> FindByUserName(string userName)
        {
            return await DbContext.Users.ToAsyncEnumerable()
                .Where(user => user.UserName == userName)
                                  .FirstOrDefaultAsync();
        }

    }
}