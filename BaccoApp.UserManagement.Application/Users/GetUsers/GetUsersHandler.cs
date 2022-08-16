using AutoMapper;
using BaccoApp.UserManagement.Application.Dtos;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Domain.Ports;
using BaccoApp.UserManagement.Domain.Specifications;
using MediatR;

namespace BaccoApp.UserManagement.Application.Users.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginationResponseDto<ListUserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ListUserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var getUserSpec = new GetUsersSpec(request.Search, request.RecordsPerPage, request.Page);

            var records = await _userRepository.ListAsync(getUserSpec, cancellationToken);
            var totalRecords = await _userRepository.CountAsync(getUserSpec, cancellationToken);

            var recordsDto = _mapper.Map<IEnumerable<ListUserDto>>(records);

            return new PaginationResponseDto<ListUserDto>(request, recordsDto, totalRecords);
        }
    }
}