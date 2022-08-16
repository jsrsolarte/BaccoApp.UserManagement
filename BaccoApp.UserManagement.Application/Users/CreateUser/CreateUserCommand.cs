using BaccoApp.UserManagement.Application.Users.Dtos;
using MediatR;

namespace BaccoApp.UserManagement.Application.CreateUser;

public class CreateUserCommand : IRequest<UserDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}