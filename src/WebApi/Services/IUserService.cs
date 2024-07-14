using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface IUserService
    {
        Task<User[]> GetAllUsers();
        // Task<User> GetUserByName(string userName);

# nullable enable
        Task<User?> GetUserById(int id);
# nullable disable
        Task<Result> CreateUser(User user);
        Task<Result> UpdateUser(int id, User user);
        Task<int> DeleteUser(int id);
    }
}
