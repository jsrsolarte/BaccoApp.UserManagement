using BaccoApp.UserManagement.Application.Users.Dtos;
using MediatR;

namespace BaccoApp.UserManagement.Application.CreateUser;

public class CreateUserCommand : IRequest<DetailUserDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}