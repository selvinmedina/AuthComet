using AuthComet.Auth.Features.Users;
using AuthComet.Auth.Infrastructure;
using AuthComet.Auth.Infrastructure.AuthCometDatabase;
using AuthComet.Domain.Validations;
using EntityFramework.Infrastructure.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthCometDbContext>(options =>
            options.UseSqlServer("name=AuthComet"));

builder.Services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();
builder.Services.AddTransient<UsersService>();
builder.Services.AddSingleton<UserDomain>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
