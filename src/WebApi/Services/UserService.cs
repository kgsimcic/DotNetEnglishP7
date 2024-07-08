using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using System;

namespace Dot.Net.WebApi.Services
{
    public class UserService : IUserService
    {

        protected IRepository<User> _userRepository { get; }
        public UserService(IRepository<User> userRepository) {
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
            return _userRepository.GetAll();
        }

        /*public async Task<User> GetUserByName(string userName)
        {
            return await _userRepository.FindByUserName(userName);
        }*/

        public async Task<int> CreateUser(User user)
        {
            var validationResult = ValidateUser(user);

            if (!validationResult.isValid)
            {
                return 0;
            }

            _userRepository.Add(user);
            return await _userRepository.SaveChangesAsync();
        }
        public async Task<int> UpdateUser(int id, User user)
        {

            var existingUser = _userRepository.GetById(id);
            // Console.WriteLine(existingUser);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _userRepository.Update(user);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteUser(int id)
        {
            var existingUser = _userRepository.GetById(id);
            // Console.WriteLine(existingUser);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            
            _userRepository.Delete(existingUser);
            return await _userRepository.SaveChangesAsync();
        }

# nullable enable
        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }
# nullable disable
    }
}
