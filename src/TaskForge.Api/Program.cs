using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskForge.Api.Middleware;
using TaskForge.Application.Commands.Tasks.CompleteTask;
using TaskForge.Application.Commands.Tasks.CreateTask;
using TaskForge.Application.Commands.Tasks.DeleteTask;
using TaskForge.Application.Commands.Tasks.UpdateTask;
using TaskForge.Application.Common.Interfaces;
using TaskForge.Identity.Data;
using TaskForge.Identity.Jwt;
using TaskForge.Identity.Seed;
using TaskForge.Identity.Users;
using TaskForge.Infrastructure.Communication;
using TaskForge.Infrastructure.Messaging.Publishers;
using TaskForge.Infrastructure.Persistence;
using TaskForge.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 🔑 USER SECRETS
builder.Configuration.AddUserSecrets<Program>();

// CONTROLLERS + SWAGGER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB CONTEXT (APP)
builder.Services.AddDbContext<TaskForgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DB CONTEXT (IDENTITY)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// IDENTITY
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// APP SERVICES
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<CreateTaskHandler>();
builder.Services.AddScoped<UpdateTaskHandler>();
builder.Services.AddScoped<CompleteTaskHandler>();
builder.Services.AddScoped<DeleteTaskHandler>();
builder.Services.AddScoped<TaskForge.Application.Readers.Tasks.ITaskQueryReader,
    TaskForge.Infrastructure.Readers.Tasks.TaskQueryReader>();
builder.Services.AddSingleton<ISmsService>(sp =>
    new TwilioSmsService(
        builder.Configuration["Twilio:AccountSid"] ,
        builder.Configuration["Twilio:AuthToken"] ,
        builder.Configuration["Twilio:FromNumber"] 
        ));
builder.Services.AddSingleton<ServiceBusPublisher>();
builder.Services.AddHostedService<OutboxProcessor>();

// JWT SETTINGS
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// TOKEN SERVICE
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddSingleton<IEventPublisher, ServiceBusPublisher>();

// JWT AUTH
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration
        .GetSection("JwtSettings")
        .Get<JwtSettings>()!;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Key
            ))
    };
});

var app = builder.Build();

await IdentitySeeder.Seed(app.Services);

// PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

