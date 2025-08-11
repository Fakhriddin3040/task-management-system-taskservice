using TaskManagementSystem.GrpcLib.Configurations.AspNet;
using TaskManagementSystem.TaskService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationGrpcLib();
builder.Services.AddApplicationServices();
builder.Services.AddApplicationNumeralRankServices();
builder.Services.AddApplicationMediatRServices();
builder.Services.AddSharedLibServices();


builder.Services.AddLogging(l =>
{
    l.AddConsole();
    l.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();

app.UseApplicationGrpcLib();

app.Run();
