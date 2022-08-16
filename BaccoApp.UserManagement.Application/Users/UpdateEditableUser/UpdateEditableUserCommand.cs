using BaccoApp.UserManagement.Application.Users.Dtos;
using MediatR;

namespace BaccoApp.UserManagement.Application.Users.UpdateBaseUser
{
    public class UpdateEditableUserCommand: IRequest<DetailUserDto>
    {
        public Guid Id { get; set; }
        public EditableUserDto? User { get; set; }
    }
}