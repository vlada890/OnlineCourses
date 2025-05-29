using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.Data.Repositories;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.Common;
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

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _repo.GetByIdAsync(userId);
        }
                public async Task<ServiceResult<User>> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                if (await _repo.GetByEmailAsync(model.Email) != null)
                    return ServiceResult<User>.FailureResult("Email already in use");

                var user = _mapper.Map<User>(model);
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

                await _repo.AddAsync(user);
                var result = await _repo.SaveChangesAsync();

                if (!result)
                    return ServiceResult<User>.FailureResult("Failed to register user");

                return ServiceResult<User>.SuccessResult(user, "User registered successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.FailureResult($"Error registering user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> ValidateUserForLoginAsync(LoginViewModel model)
        {
            try
            {
                var user = await _repo.GetByEmailAsync(model.Email);
                if (user == null)
                    return ServiceResult<User>.FailureResult("Invalid email or password");

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    model.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                    return ServiceResult<User>.SuccessResult(user, "Login successful");

                return ServiceResult<User>.FailureResult("Invalid email or password");
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.FailureResult($"Error during login: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _repo.GetAllAsync();
                return ServiceResult<List<User>>.SuccessResult(users.ToList(), "Users retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<List<User>>.FailureResult($"Error retrieving users: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _repo.GetByIdAsync(id);
                if (user == null)
                    return ServiceResult<User>.FailureResult("User not found");

                return ServiceResult<User>.SuccessResult(user, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.FailureResult($"Error retrieving user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> UpdateUserRoleAsync(int userId, UserRole newRole)
        {
            try
            {
                var user = await _repo.GetByIdAsync(userId);
                if (user == null)
                    return ServiceResult<User>.FailureResult("User not found");

                user.Role = newRole;
                await _repo.UpdateAsync(user);
                var result = await _repo.SaveChangesAsync();

                if (!result)
                    return ServiceResult<User>.FailureResult("Failed to update user role");

                return ServiceResult<User>.SuccessResult(user, "User role updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.FailureResult($"Error updating user role: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _repo.GetByIdAsync(id);
                if (user == null)
                    return ServiceResult<bool>.FailureResult("User not found");

                // Check if user has enrollments
                if (user.Enrollments != null && user.Enrollments.Any())
                    return ServiceResult<bool>.FailureResult("Cannot delete user with active enrollments");

                await _repo.DeleteAsync(id);
                var result = await _repo.SaveChangesAsync();

                if (!result)
                    return ServiceResult<bool>.FailureResult("Failed to delete user");

                return ServiceResult<bool>.SuccessResult(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.FailureResult($"Error deleting user: {ex.Message}");
            }
        }
    }
}
