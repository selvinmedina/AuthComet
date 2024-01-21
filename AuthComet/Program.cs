using AuthComet.Auth.Common;
using AuthComet.Auth.Features.Auth;
using AuthComet.Auth.Features.Notification;
using AuthComet.Auth.Features.Queues;
using AuthComet.Auth.Features.Users;
using AuthComet.Auth.Infrastructure;
using AuthComet.Auth.Infrastructure.AuthCometDatabase;
using AuthComet.Domain.Validations;
using EntityFramework.Infrastructure.Core.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var jwtSettings = new JwtSettings()
{
    Audience = config["Security:Jwt:Audience"]!,
    Issuer = config["Security:Jwt:Issuer"]!,
    SecretKey = config["Security:Jwt:SecretKey"]!
};

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };
            });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthCometDbContext>(options =>
            options.UseSqlServer("name=AuthComet"));

builder.Services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();
builder.Services.AddTransient<UsersService>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddSingleton<UserDomain>();
builder.Services.AddSingleton(jwtSettings);

var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection("RabbitMQ").Bind(rabbitMqSettings);
builder.Services.AddSingleton(rabbitMqSettings);

builder.Services.AddTransient<RabbitMQProducer>();

builder.Services.AddTransient<INotificationService>(x =>
{
    var hubUrl = config["Apis:AuthCometNotificationHub"];

    return new NotificationService(hubUrl!);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
