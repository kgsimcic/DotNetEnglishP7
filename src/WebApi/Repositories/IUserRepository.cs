using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public interface IUserRepository
    {
        Task<User[]> GetAll();
# nullable enable
        Task<User?> GetById(int id);
        Task<User?> GetByUserName(string userName);
#nullable disable
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        DbSet<User> Set();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
