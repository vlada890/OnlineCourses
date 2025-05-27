using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.BusinessLogic.Interfaces;
using OnlineCourses.BusinessLogic.Services;
using OnlineCourses.Data;
using OnlineCourses.Data.Repositories;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("OnlineCourses.Data")));

// Configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*

async Task SeedData(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher<User>>();
        var userRepo = services.GetRequiredService<IUserRepository>();
        var courseRepo = services.GetRequiredService<ICourseRepository>();

        // Ensure database is created
        await context.Database.MigrateAsync();

        // 1. Create an Instructor User if they don't exist
        var instructorUser = await userRepo.GetByEmailAsync("john.smith@example.com");
        if (instructorUser == null)
        {
            instructorUser = new User
            {
                FullName = "John Smith",
                Email = "john.smith@example.com",
                Role = UserRole.Instructor, // Assign the Instructor role
                CreatedAt = DateTime.UtcNow
            };
            instructorUser.PasswordHash = passwordHasher.HashPassword(instructorUser, "Password123!"); // Set a strong password
            await userRepo.AddAsync(instructorUser);
            await userRepo.SaveChangesAsync(); // Save the instructor user
            Console.WriteLine("Created John Smith (Instructor) user.");
        }

        // 2. Now, create the Course, referencing the InstructorId
        var introToCCourse = await courseRepo.GetCourseByTitleAsync("Introduction to C"); //add method to CourseRepository/ICourseRepository
        if (introToCCourse == null)
        {
            var course = new Course
            {
                Title = "Introduction to C",
                Description = "Learn the basics of C programming language: syntax, data types and control structures.",
                Price = 49.99M, // Always provide a price for decimal types
                InstructorId = instructorUser.Id, // CORRECT: Assign the ID of the created/found instructor
                CreatedAt = DateTime.UtcNow
            };
            await courseRepo.AddAsync(course);
            await courseRepo.SaveChangesAsync(); // Save the course
            Console.WriteLine("Created 'Introduction to C' course.");
        }
        else
        {
            Console.WriteLine("'Introduction to C' course already exists.");
        }

        // Add other courses/users as needed for seeding...
    }
}
async Task SeedData(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher<User>>();
        var userRepo = services.GetRequiredService<IUserRepository>();
        var courseRepo = services.GetRequiredService<ICourseRepository>();

        Console.WriteLine("Applying database migrations...");
        await context.Database.MigrateAsync();
        Console.WriteLine("Database migrations applied.");

        // 1. Create an Instructor User if they don't exist
        Console.WriteLine("Checking for John Smith instructor...");
        var instructorUser = await userRepo.GetByEmailAsync("john.smith@example.com");
        if (instructorUser == null)
        {
            Console.WriteLine("John Smith not found, creating...");
            instructorUser = new User
            {
                FullName = "John Smith",
                Email = "john.smith@example.com",
                Role = UserRole.Instructor,
                CreatedAt = DateTime.UtcNow
            };
            instructorUser.PasswordHash = passwordHasher.HashPassword(instructorUser, "Password123!");
            await userRepo.AddAsync(instructorUser);
            await userRepo.SaveChangesAsync();
            Console.WriteLine("Created John Smith (Instructor) user with ID: " + instructorUser.Id);
        }
        else
        {
            Console.WriteLine("John Smith (Instructor) user already exists with ID: " + instructorUser.Id);
        }

        // 2. Now, create the Course, referencing the InstructorId
        Console.WriteLine("Checking for 'Introduction to C' course...");
        var introToCCourse = await courseRepo.GetCourseByTitleAsync("Introduction to C");
        if (introToCCourse == null)
        {
            Console.WriteLine("'Introduction to C' course not found, creating...");
            var course = new Course
            {
                Title = "Introduction to C",
                Description = "Learn the basics of C programming language: syntax, data types and control structures.",
                Price = 49.99M,
                ImageUrl = "https://via.placeholder.com/150/0000FF/FFFFFF?text=C+Intro",
                InstructorId = instructorUser.Id,
                CreatedAt = DateTime.UtcNow
            };
            await courseRepo.AddAsync(course);
            await courseRepo.SaveChangesAsync();
            Console.WriteLine("Created 'Introduction to C' course.");
        }
        else
        {
            Console.WriteLine("'Introduction to C' course already exists.");
        }
        // Repeat for Calculus and Mechanics1
        Console.WriteLine("SeedData method finished.");
    }
}*/

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        if (!context.Courses.Any())
        {
            // First, create some instructor users directly in the context
            var johnSmith = new User
            {
                FullName = "John Smith",
                Email = "john.smith@example.com",
                Role = UserRole.Instructor,
                PasswordHash = "TempHash1", // You might want to hash this properly
                CreatedAt = DateTime.UtcNow
            };

            var davidJerison = new User
            {
                FullName = "David Jerison",
                Email = "david.jerison@example.com",
                Role = UserRole.Instructor,
                PasswordHash = "TempHash2",
                CreatedAt = DateTime.UtcNow
            };

            var walterLewin = new User
            {
                FullName = "Walter Lewin",
                Email = "walter.lewin@example.com",
                Role = UserRole.Instructor,
                PasswordHash = "TempHash3",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(johnSmith, davidJerison, walterLewin);
            context.SaveChanges(); // Save users first to get their IDs

            // Now create courses with the instructor IDs
            context.Courses.AddRange(
                new Course
                {
                    Title = "Introduction to C",
                    Description = "Learn the basics of C programming language:syntax,data types and control structures.",
                    InstructorId = johnSmith.Id, // Use InstructorId instead of Instructor
                    Price = 49.99M
                },
                new Course
                {
                    Title = "Calculus",
                    Description = "Learn the basics of calculus:differentiation,integrals and series.",
                    InstructorId = davidJerison.Id, // Use InstructorId instead of Instructor
                    Price = 79.99M
                },
                new Course
                {
                    Title = "Mechanics1",
                    Description = "Learn about conservation of energy,thermodynamics and some quantum mechanics.",
                    InstructorId = walterLewin.Id, // Use InstructorId instead of Instructor
                    Price = 89.99M
                }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while creating/seeding the database.");
    }
}

    app.Run();
