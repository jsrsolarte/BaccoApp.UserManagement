using BaccoApp.UserManagement.Application.CreateUser;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Application.Users.GetUsers;
using BaccoApp.UserManagement.Application.Users.UpdateBaseUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BaccoApp.UserManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<DetailUserDto>> CreateUser(CreateUserCommand user)
        {
            var userdto = await _mediator.Send(user);
            return CreatedAtAction("CreateUser", userdto);
        }

        [HttpGet]
        public async Task<ActionResult<ListUserDto>> GetUsers([FromQuery] GetUsersQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPut("{idUser}")]
        public async Task<ActionResult<DetailUserDto>> UpdateUser(Guid idUser, EditableUserDto editableUser)
        {
            var response = await _mediator.Send(new UpdateEditableUserCommand
            {
                Id = idUser,
                User = editableUser
            });
            return Ok(response);
        }
    }
}