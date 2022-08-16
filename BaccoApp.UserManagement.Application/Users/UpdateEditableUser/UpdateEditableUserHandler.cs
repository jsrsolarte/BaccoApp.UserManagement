using AutoMapper;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Application.Users.UpdateBaseUser;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Exceptions;
using BaccoApp.UserManagement.Domain.Ports;
using MediatR;

namespace BaccoApp.UserManagement.Application.Users.UpdateEditableUser
{
    public class UpdateEditableUserHandler : IRequestHandler<UpdateEditableUserCommand, DetailUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateEditableUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<DetailUserDto> Handle(UpdateEditableUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (user == null) throw new EntityNotExistException(typeof(User), request.Id.ToString());

            if (request.User?.FirstName != null)
            {
                user.FirstName = request.User.FirstName;
            }

            if (request.User?.LastName != null)
            {
                user.LastName = request.User.LastName;
            }

            await _userRepository.UpdateAsync(user, cancellationToken);

            return _mapper.Map<DetailUserDto>(user);
        }
    }
}