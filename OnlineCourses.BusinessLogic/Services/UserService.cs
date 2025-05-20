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

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _repo.GetByIdAsync(userId);
        }
    }
}
