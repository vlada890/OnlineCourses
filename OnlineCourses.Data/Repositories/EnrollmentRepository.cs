using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Data.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _ctx;

        public EnrollmentRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId) =>
            await _ctx.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId);

        public async Task AddAsync(Enrollment enrollment)
        {
            await _ctx.Enrollments.AddAsync(enrollment);
        }

        public async Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(int userId) =>
            await _ctx.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == userId)
                .ToListAsync();

        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

    }
}
