using TaskManagementSystem.TaskService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDbContext(builder.Configuration);

var app = builder.Build();

app.Run();
