using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;

namespace WebApi.Services
{
    public class UserService : IUserService
    {

        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository) {
            _userRepository = userRepository;
        }

        public User[] GetAllUsers()
        {
            return _userRepository.FindAll();
        }

        public User GetUserByName(string userName)
        {
            return _userRepository.FindByUserName(userName);
        }

        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }
        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }

        public User GetUserById(int id)
        {
            _userRepository.FindById(id);
        }
    }
}
