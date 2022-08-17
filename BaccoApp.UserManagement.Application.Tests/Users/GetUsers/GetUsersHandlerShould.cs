using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoMapper;
using BaccoApp.UserManagement.Application.Users;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Application.Users.GetUsers;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Ports;
using BaccoApp.UserManagement.Domain.Specifications;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace BaccoApp.UserManagement.Application.Tests.Users.GetUsers
{
    public class GetUsersHandlerShould
    {
        private readonly IFixture _fixture;

        public GetUsersHandlerShould()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            var mapperConfiguration = new MapperConfiguration(mc => mc.AddProfile(new UsersProfile()));
            var mapper = new Mapper(mapperConfiguration);
            _fixture.Inject<IMapper>(mapper);
        }

        [Fact]
        public void Constructor_NullParams_ThrowsArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(_fixture);

            // Assert
            assertion.Verify(typeof(GetUsersHandler).GetConstructors());
        }

        [Theory]
        [InlineData(100, 1, 10, 10)]
        [InlineData(99, 10, 10, 10)]
        [InlineData(51, 50, 5, 11)]
        [InlineData(0, 1, 10, 0)]
        public async Task HandleShouldReturnUserListWithPaginationAsync(int usersNumber, int page, int usersPerPage, int totalPages)
        {
            //Arrange
            var users = _fixture.CreateMany<User>(usersNumber);
            var usersPag = users.Skip((page - 1) * usersPerPage).Take(usersPerPage);

            var repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            repositoryMock.Setup(x => x.ListAsync(It.IsAny<GetUsersSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(usersPag);
            repositoryMock.Setup(x => x.CountAsync(It.IsAny<GetUsersSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(users.Count);

            IRequestHandler<GetUsersQuery, PaginationResponse<ListUserDto>> sut = _fixture.Create<GetUsersHandler>();

            //Act
            var response = await sut.Handle(new GetUsersQuery { Page = page, RecordsPerPage = usersPerPage }, CancellationToken.None);

            //Assert
            repositoryMock.Verify(x => x.ListAsync(It.IsAny<GetUsersSpec>(), It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(x => x.CountAsync(It.IsAny<GetUsersSpec>(), It.IsAny<CancellationToken>()), Times.Once);

            response.Records.Should().BeEquivalentTo(usersPag);
            response.TotalRecords.Should().Be(usersNumber);
            response.TotalPages.Should().Be(totalPages);
        }
    }
}