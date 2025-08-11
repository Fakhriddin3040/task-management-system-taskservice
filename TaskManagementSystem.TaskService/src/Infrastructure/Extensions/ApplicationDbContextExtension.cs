using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.TaskService.Infrastructure.DataAccess.ORM;

namespace TaskManagementSystem.TaskService.Infrastructure.Extensions;


public static class ApplicationDbContextExtension
{
    public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection("Database");

        var connectionString =
            $"Host={dbSettings["Host"]};" +
            $"Port={dbSettings["Port"]};" +
            $"Database={dbSettings["Database"]};" +
            $"Username={dbSettings["User"]};" +
            $"Password={dbSettings["Password"]};" +
            $"Include Error Detail=true";

        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });
    }
}
