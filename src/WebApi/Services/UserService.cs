using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using System;
using Microsoft.AspNetCore.Mvc;

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
            return str.Any(ch => char.IsNumber(ch));
        }
        public bool HasSpecialChar(string str) {
            return str.Any(ch => !char.IsLetterOrDigit(ch));
        }

        public Result ValidateUser(User user) {

            // Validate username
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                return Result.Failure(
                    new Error("User.UsernameRequired", "Username is required."));
            }

            // Validate password (8 chars at least, one uppercase, number, symbol)
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return Result.Failure(
                    new Error("User.PasswordRequired", "Password is required."));
            }
            else if (user.Password.Length < 8)
            {
                return Result.Failure(
                    new Error("User.PasswordTooShort", "Password must be at least 8 characters long."));
            }
            else if (string.Equals(user.Password.ToLower(), user.Password))
            {
                return Result.Failure(
                    new Error("User.PasswordNeedsUppercase", "Password must contain at least one uppercase character."));
            }
            else if (!HasNumber(user.Password))
            {
                return Result.Failure(
                    new Error("User.PasswordNeedsNumber", "Password must have at least one number."));
            }
            else if (!HasSpecialChar(user.Password))
            {
                return Result.Failure(
                    new Error("User.PasswordNeedsSpecial", "Password must contain at least one special character."));
            }

            return Result.Success();
        }

        public async Task<User[]> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        /*public async Task<User> GetUserByName(string userName)
        {
            return await _userRepository.FindByUserName(userName);
        }*/

        public async Task<Result> CreateUser(User user)
        {
            var validationResult = ValidateUser(user);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();
            return validationResult;
        }

        public async Task<Result> UpdateUser(int id, User user)
        {

            var existingUser = _userRepository.GetById(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var validationResult = ValidateUser(user);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            return validationResult;
        }

        public async Task<int> DeleteUser(int id)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            
            _userRepository.Delete(existingUser);
            return await _userRepository.SaveChangesAsync();
        }

# nullable enable
        public async Task<User?> GetUserById(int id)
        {
            return await _userRepository.GetById(id);
        }
# nullable disable
    }
}
