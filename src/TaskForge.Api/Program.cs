using Microsoft.EntityFrameworkCore;
using TaskForge.Api.Middleware;
using TaskForge.Application.Commands.Tasks.CompleteTask;
using TaskForge.Application.Commands.Tasks.CreateTask;
using TaskForge.Application.Commands.Tasks.DeleteTask;
using TaskForge.Application.Commands.Tasks.UpdateTask;
using TaskForge.Application.Interfaces;
using TaskForge.Infrastructure.Messaging.Publishers;
using TaskForge.Infrastructure.Persistence;
using TaskForge.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskForgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<OutboxProcessor>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<CreateTaskHandler>();
builder.Services.AddScoped<UpdateTaskHandler>();
builder.Services.AddScoped<CompleteTaskHandler>();
builder.Services.AddScoped<DeleteTaskHandler>();
builder.Services.AddScoped<TaskForge.Application.Readers.Tasks.ITaskQueryReader, TaskForge.Infrastructure.Readers.Tasks.TaskQueryReader>();

builder.Services.AddSingleton<ServiceBusPublisher>();

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<CorrelationIdMiddleware>();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}    

app.Run();
