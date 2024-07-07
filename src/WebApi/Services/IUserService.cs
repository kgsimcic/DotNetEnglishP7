using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface IUserService
    {
        User[] GetAllUsers();
        Task<User> GetUserByName(string userName);

        Task<User> GetUserById(int id);
        Task<int> CreateUser(User user);
        Task<int> UpdateUser(int id, User user);
        Task<int> DeleteUser(int id);
    }
}
