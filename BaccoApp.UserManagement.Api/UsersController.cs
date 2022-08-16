using BaccoApp.UserManagement.Application.CreateUser;
using BaccoApp.UserManagement.Application.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BaccoApp.UserManagement.Api
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
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserCommand user)
        {
            var userdto = await _mediator.Send(user);
            return CreatedAtAction("CreateUser", userdto);
        }
    }
}