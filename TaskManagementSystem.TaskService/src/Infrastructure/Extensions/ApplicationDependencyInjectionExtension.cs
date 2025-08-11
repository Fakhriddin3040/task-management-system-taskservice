using TaskManagementSystem.GrpcLib.Configurations.AspNet;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Api.Grpc.Server.Interceptors;
using TaskManagementSystem.SharedLib.Providers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.SharedLib.Services;
using TaskManagementSystem.TaskService.Api.Grpc.Services;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies.Validations;
using TaskManagementSystem.TaskService.Core.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskService.Core.Services.NumeralRank;
using TaskManagementSystem.TaskService.Infrastructure.DataAccess.Repositories;
using TaskManagementSystem.TaskService.Infrastructure.Policies;

namespace TaskManagementSystem.TaskService.Infrastructure.Extensions;


public static class ApplicationDependencyInjectionExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<GetLatestTaskRankService>();
        services.AddScoped<NumeralRankGenerationService>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddSingleton<IValidTaskTitlePolicy, ValidTaskTitlePolicy>();
        services.AddSingleton<IValidTaskDescriptionPolicy, ValidTaskDescriptionPolicy>();
    }

    public static void AddApplicationGrpcLib(this IServiceCollection services)
    {
        services.AddGrpcLibServices();
        services.AddGrpc(op =>
            op.Interceptors.Add<ExecutionContextInitializerGrpcServerInterceptor>()
        );
    }


    public static void AddApplicationNumeralRankServices(this IServiceCollection services)
    {
        services.AddScoped<INumeralRankStrategy, FirstNumeralRankStrategy>();
        services.AddScoped<INumeralRankStrategy, TopNumeralRankStrategy>();
        services.AddScoped<INumeralRankStrategy, BetweenNumeralRankStrategy>();
        services.AddScoped<INumeralRankStrategy, EndNumeralRankStrategy>();

        services.AddScoped<INumeralRankStrategySelector, NumeralRankStrategySelector>();

        services.AddScoped<INumeralRankValidationStrategy, FirstNumeralRankValidationStrategy>();
        services.AddScoped<INumeralRankValidationStrategy, TopNumeralRankValidationStrategy>();
        services.AddScoped<INumeralRankValidationStrategy, BetweenNumeralRankValidationStrategy>();
        services.AddScoped<INumeralRankValidationStrategy, EndNumeralRankValidationStrategy>();

        services.AddScoped<INumeralRankValidationStrategySelector, NumeralRankValidationStrategySelector>();
    }

    public static void AddApplicationMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
    }

    public static void AddSharedLibServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<IExecutionContextProvider, GrpcExecutionContextProvider>();
    }

    public static void UseApplicationGrpcLib(this WebApplication app)
    {
        app.UseGrpcLib();
        app.MapGrpcService<TaskGrpcService>();
    }
}
