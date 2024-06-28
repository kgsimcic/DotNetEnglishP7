using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<User> GetUserByName(string userName)
        {
            return await _userRepository.FindByUserName(userName);
        }

        public async Task<User> CreateUser(User user)
        {
            return await _userRepository.Create(user);
        }
        public async Task<int> UpdateUser(int id, User user)
        {

            var existingUser = _userRepository.FindById(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return await _userRepository.Update(user);
        }

        public async Task<int> DeleteUser(int id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.FindById(id);
        }
    }
}
