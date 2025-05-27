using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions opts) : base(opts) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasConversion<int>();

                // Configuring relationship with courses as instructor
                entity.HasMany(u => u.CoursesCreated)
                      .WithOne(c => c.Instructor)
                      .HasForeignKey(c => c.InstructorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure relationships
    modelBuilder.Entity<Course>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
    });

    // Configure Enrollment entity
    modelBuilder.Entity<Enrollment>(entity =>
    {
        entity.HasKey(e => e.Id);
        
        entity.HasOne(e => e.User)
              .WithMany(u => u.Enrollments)
              .HasForeignKey(e => e.UserId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Course)
              .WithMany(c => c.Enrollments)
              .HasForeignKey(e => e.CourseId)
              .OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate enrollments
        entity.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();
    });
        }
    }
}

