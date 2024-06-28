using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IUserService
    {
        User[] GetAllUsers();
        Task<User> GetUserByName(string userName);

        Task<User> GetUserById(int id);
        Task<User> AddUser(User user);
        Task<int> UpdateUser(User user);
        Task<int> DeleteUser(int id);
    }
}
