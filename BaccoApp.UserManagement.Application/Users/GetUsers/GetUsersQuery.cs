using BaccoApp.UserManagement.Application.Users.Dtos;
using MediatR;

namespace BaccoApp.UserManagement.Application.Users.GetUsers
{
    public class GetUsersQuery : PaginationRequest, IRequest<PaginationResponse<ListUserDto>>
    {
    }
}