using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface IUserService
    {
        Task<User[]> GetAllUsers();

# nullable enable
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByName(string userName);
# nullable disable
        Task<Result> CreateUser(User user);
        Task<Result> UpdateUser(User user);
        Task<int> DeleteUser(int id);
        bool CheckPassword(User user, string password);
    }
}
