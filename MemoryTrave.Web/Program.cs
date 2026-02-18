using MemoryTrave.Infrastructure;
using MemoryTrave.Web.Extensions;
using MemoryTrave.Web.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<MemoryTraveDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString(nameof(MemoryTraveDbContext)));
});

builder.Services.AddApplicationLayer();
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();