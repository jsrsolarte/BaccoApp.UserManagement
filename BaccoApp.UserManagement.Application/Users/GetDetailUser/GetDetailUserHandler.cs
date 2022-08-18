using AutoMapper;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Exceptions;
using BaccoApp.UserManagement.Domain.Ports;
using MediatR;

namespace BaccoApp.UserManagement.Application.Users.GetDetailUser
{
    public class GetDetailUserHandler : IRequestHandler<GetDetailUserQuery, DetailUserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public GetDetailUserHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DetailUserDto> Handle(GetDetailUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (user == null) throw new EntityNotExistException(typeof(User), request.Id);

            return _mapper.Map<DetailUserDto>(user);
        }
    }
}