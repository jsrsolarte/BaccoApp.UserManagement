using AutoMapper;
using BaccoApp.UserManagement.Application.CreateUser;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Domain.Entities;

namespace BaccoApp.UserManagement.Application.Users
{
    public sealed class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, ListUserDto>();
            CreateMap<User, DetailUserDto>();
        }
    }
}