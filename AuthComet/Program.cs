using AuthComet.Auth.Features.Users;
using AuthComet.Auth.Infrastructure;
using AuthComet.Auth.Infrastructure.AuthCometDatabase;
using EntityFramework.Infrastructure.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthCometDbContext>(options =>
            options.UseSqlServer("name=AuthComet"));

builder.Services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();
builder.Services.AddTransient<UsersService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
