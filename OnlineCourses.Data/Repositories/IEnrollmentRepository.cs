using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Data.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<bool> IsUserEnrolledAsync(int userId, int courseId);
        Task AddAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(int userId);
        Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseId);
        Task<bool> SaveChangesAsync();
    }
}
