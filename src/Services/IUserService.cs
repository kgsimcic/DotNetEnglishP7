using Dot.Net.WebApi.Domain;
using System.Collections;

namespace WebApi.Services
{
    public interface IUserService
    {
        User[] GetAllUsers();
        User GetUserByName(string userName);

        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
