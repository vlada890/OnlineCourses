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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        var instructors = new List<User>
{
    new User { FullName = "John Smith", Email = "john.smith@example.com", PasswordHash = "123", Role = UserRole.Instructor },
    new User { FullName = "David Jerison", Email = "david.jerison@example.com", PasswordHash = "123", Role = UserRole.Instructor },
    new User { FullName = "Walter Lewin", Email = "walter@example.com", PasswordHash = "123", Role = UserRole.Instructor }
};

        // Only add if not exists
        foreach (var instructor in instructors)
        {
            if (!context.Users.Any(u => u.Email == instructor.Email))
            {
                context.Users.Add(instructor);
            }
        }
        context.SaveChanges(); // Needed to get Ids for new users

        // Now retrieve the instructors with their IDs populated
        var arthur = context.Users.First(u => u.Email == "arthur.mattuck@example.com");
        var dave = context.Users.First(u => u.Email == "david.jerison@example.com");
        var walter = context.Users.First(u => u.Email == "walter@example.com");

        if (!context.Courses.Any())
        {
            context.Courses.AddRange(
                new Course
                {
                    Title = "Differential Equations",
                    Description = "Model systems using ODEs",
                    InstructorId = arthur.Id
                },
                new Course
                {
                    Title = "Calculus 1",
                    Description = "Intro into calculus topics",
                    InstructorId = dave.Id
                },
                new Course
                {
                    Title = "Classical Mechanics",
                    Description = "Learn about the surrounding world.Build intuition with real-life examples.",
                    InstructorId = walter.Id
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
