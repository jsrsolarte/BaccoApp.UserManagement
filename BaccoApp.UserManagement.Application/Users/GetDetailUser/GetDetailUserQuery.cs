using BaccoApp.UserManagement.Application.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaccoApp.UserManagement.Application.Users.GetDetailUser
{
    public class GetDetailUserQuery: IRequest<DetailUserDto>
    {
        public Guid Id { get; set; }
    }
}
