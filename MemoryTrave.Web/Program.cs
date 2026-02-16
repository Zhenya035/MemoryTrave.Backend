using MemoryTrave.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<MemoryTraveDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString(nameof(MemoryTraveDbContext)));
});

var app = builder.Build();

app.Run();