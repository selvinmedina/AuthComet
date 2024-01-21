using AuthComet.Mails.Common;
using AuthComet.Mails.Features.Emails;
using AuthComet.Mails.Features.Notifications;
using AuthComet.Mails.Features.Queues;
using Hangfire;
using Hangfire.Console;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var smtpSettings = new SmtpSettings();
configuration.GetSection("SmtpSettings").Bind(smtpSettings);
builder.Services.AddSingleton(smtpSettings);
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddHangfire(config =>
{
    string connectionString = configuration.GetConnectionString("Mongo")!;
    string databaseName = configuration["HangFire:DatabaseName"]!;

    var storageOptions = new MongoStorageOptions
    {
        CheckConnection = true,

        MigrationOptions = new MongoMigrationOptions
        {
            MigrationStrategy = new MigrateMongoMigrationStrategy(),
            BackupStrategy = new CollectionMongoBackupStrategy()
        },

        JobExpirationCheckInterval = TimeSpan.FromMinutes(15)
    };

    config.UseMongoStorage(connectionString, databaseName, storageOptions);

    config.UseConsole();
});


builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 3;
    options.Queues = new[] { "sync_process", "default" };
});

var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection("RabbitMQ").Bind(rabbitMqSettings);
builder.Services.AddSingleton(rabbitMqSettings);
builder.Services.AddSingleton<RabbitMQConsumer>();

var hubUrl = configuration["Apis:AuthCometNotificationHub"];
builder.Services.AddSingleton(provider =>
{
    var emailService = provider.GetRequiredService<IEmailService>();
    return new NotificationService(hubUrl!, emailService);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = new[] { new MyAuthorizationFilter() },
    DashboardTitle = "Hangfire - Send Emails"
});

app.UseRabbitMQListener();

var notificationService = app.Services.GetRequiredService<NotificationService>();
await notificationService.Start();

app.Run();
