using BaccoApp.UserManagement.Domain.Ports;
using BaccoApp.UserManagement.Infrastructure;
using BaccoApp.UserManagement.Infrastructure.Extensions;
using MediatR;
using System.Reflection;

var applicationAssembly = Assembly.Load("BaccoApp.UserManagement.Application");

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

if (builder.Environment.IsEnvironment("Development")) builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddMediatR(applicationAssembly, typeof(Program).Assembly);
builder.Services.AddAutoMapper(applicationAssembly);

builder.Services.AddControllers();

builder.Services.AddPersistence(config);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting().UseEndpoints(endpoints => { endpoints.MapHealthChecks("/health"); });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();