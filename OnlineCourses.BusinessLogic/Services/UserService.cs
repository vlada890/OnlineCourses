using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Data.Repositories;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.BusinessLogic.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            IUserRepository repo,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
        {
            _repo = repo;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterViewModel model)
        {
            if (await _repo.GetByEmailAsync(model.Email) != null)
                return (false, "Email already in use");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            await _repo.AddAsync(user);
            var result = await _repo.SaveChangesAsync();

            if (!result)
                return (false, "Failed to register user");

            return (true, "Registration successful");
        }

        public async Task<(bool Success, User User)> ValidateUserAsync(LoginViewModel model)
        {
            var user = await _repo.GetByEmailAsync(model.Email);
            if (user == null)
                return (false, null);

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                model.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
                return (true, user);

            return (false, null);
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _repo.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _repo.GetByRoleAsync(role);
        }

        public async Task<ServiceResult<User>> UpdateUserAsync(User user)
        {
            try
            {
                var updatedUser = await _repo.UpdateAsync(user);
                return ServiceResult<User>.SuccessResult(updatedUser, "User updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.FailureResult($"Update failed: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var result = await _repo.DeleteAsync(id);
                return result ?
                    ServiceResult<bool>.SuccessResult(true, "User deleted successfully") :
                    ServiceResult<bool>.FailureResult("User not found");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.FailureResult($"Delete failed: {ex.Message}");
            }
        }
    }
}
