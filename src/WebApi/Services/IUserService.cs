using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface IUserService
    {
        User[] GetAllUsers();
        // Task<User> GetUserByName(string userName);

# nullable enable
        User? GetUserById(int id);
# nullable disable
        Task<int> CreateUser(User user);
        Task<int> UpdateUser(int id, User user);
        Task<int> DeleteUser(int id);
    }
}
