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
                //===========New Methods for Admin Functionality
        Task<ServiceResult<User>> RegisterUserAsync(RegisterViewModel model);
        Task<ServiceResult<User>> ValidateUserForLoginAsync(LoginViewModel model);
        Task<ServiceResult<List<User>>> GetAllUsersAsync();
        Task<ServiceResult<User>> GetUserByIdAsync(int id);
        Task<ServiceResult<User>> UpdateUserRoleAsync(int userId, UserRole newRole);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);
    }
}
