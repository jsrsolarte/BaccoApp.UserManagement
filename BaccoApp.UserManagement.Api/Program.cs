using BaccoApp.UserManagement.Domain.Ports;
using BaccoApp.UserManagement.Infrastructure;
using BaccoApp.UserManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

if (builder.Environment.IsEnvironment("Development"))
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddControllers();

builder.Services.AddPersistence(config);
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();