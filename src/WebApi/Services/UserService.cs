using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Dot.Net.WebApi.Services
{
    public class UserService : IUserService
    {

        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository) {
            _userRepository = userRepository;
        }

        public bool HasNumber(string str)
        {
            return str.Any(ch => !char.IsNumber(ch));
        }
        public bool HasSpecialChar(string str) {
            return str.Any(ch => !char.IsLetterOrDigit(ch));
        }

        public (bool isValid, List<string> validationErrors) ValidateUser(User user) {

            var validationErrors = new List<string>();

            // Validate username
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                validationErrors.Add("Username is required.");
            }

            // Validate password (8 chars at least, one uppercase, number, symbol)
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                validationErrors.Add("Password is required.");
            }
            else if (user.Password.Length < 8)
            {
                validationErrors.Add("Password must be at least 8 characters long.");
            }
            else if (string.Equals(user.Password.ToLower(), user.Password))
            {
                validationErrors.Add("Password must contain at least one uppercase character.");
            }
            else if (!HasNumber(user.Password))
            {
                validationErrors.Add("Password must have at least one number.");
            }
            else if (!HasSpecialChar(user.Password))
            {
                validationErrors.Add("Password must contain at least one special character.");
            }

            return (validationErrors.Count == 0, validationErrors);
        }

        public User[] GetAllUsers()
        {
            return _userRepository.FindAll();
        }

        public async Task<User> GetUserByName(string userName)
        {
            return await _userRepository.FindByUserName(userName);
        }

        public async Task<int> CreateUser(User user)
        {
            var validationResult = ValidateUser(user);

            if (!validationResult.isValid)
            {
                return 0;
            }
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
            var existingUser = _userRepository.FindById(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return await _userRepository.Delete(id);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.FindById(id);
        }
    }
}
