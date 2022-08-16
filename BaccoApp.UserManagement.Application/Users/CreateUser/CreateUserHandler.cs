using AutoMapper;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Exceptions;
using BaccoApp.UserManagement.Domain.Ports;
using BaccoApp.UserManagement.Domain.Specifications;
using MediatR;

namespace BaccoApp.UserManagement.Application.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, DetailUserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IMapper mapper, IUserRepository repository)
        {
            ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<DetailUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var spec = new FindUserByEmailSpec(request.Email);
            var userCreated = await _repository.GetBySpecAsync(spec, cancellationToken);

            if (userCreated == null)
            {
                var user = _mapper.Map<User>(request);
                await _repository.AddAsync(user, cancellationToken);
                return _mapper.Map<DetailUserDto>(user);
            }
            else
            {
                throw new EntityAlreadyExistException(typeof(User), "Email", request.Email);
            }
        }
    }
}