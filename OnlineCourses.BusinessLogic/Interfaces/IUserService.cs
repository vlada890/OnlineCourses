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
    }
}
