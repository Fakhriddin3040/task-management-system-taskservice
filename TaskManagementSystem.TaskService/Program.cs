using TaskManagementSystem.GrpcLib.Configurations.AspNet;
using TaskManagementSystem.TaskService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.TaskService.Infrastructure.DataAccess.ORM;

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();   // ⚡ вот оно
}

app.UseApplicationGrpcLib();

app.Run();
