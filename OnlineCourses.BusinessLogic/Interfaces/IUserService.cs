using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Model.ViewModels;

namespace OnlineCourses.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterViewModel model);
        Task<(bool Success, User User)> ValidateUserAsync(LoginViewModel model);
        Task<User> GetByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<ServiceResult<User>> UpdateUserAsync(User user);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);

    }
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public User User { get; set; } // For login results

        public static ServiceResult<T> SuccessResult(T data, string message = "Success")
        {
            return new ServiceResult<T> { Success = true, Data = data, Message = message };
        }

        public static ServiceResult<T> FailureResult(string message)
        {
            return new ServiceResult<T> { Success = false, Message = message };
        }

    }
}
