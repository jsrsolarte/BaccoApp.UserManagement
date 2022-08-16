using BaccoApp.UserManagement.Application.Dtos;
using BaccoApp.UserManagement.Application.Users.Dtos;
using MediatR;

namespace BaccoApp.UserManagement.Application.Users.GetUsers
{
    public class GetUsersQuery : PaginationRequestDto, IRequest<PaginationResponseDto<ListUserDto>>
    {
    }
}