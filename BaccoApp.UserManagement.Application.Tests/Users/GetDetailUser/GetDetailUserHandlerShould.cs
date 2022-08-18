using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoMapper;
using BaccoApp.UserManagement.Application.Users;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Application.Users.GetDetailUser;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Exceptions;
using BaccoApp.UserManagement.Domain.Ports;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace BaccoApp.UserManagement.Application.Tests.Users.GetDetailUser
{
    public class GetDetailUserHandlerShould
    {
        private readonly IFixture _fixture;

        public GetDetailUserHandlerShould()
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
            assertion.Verify(typeof(GetDetailUserQuery).GetConstructors());
        }

        [Fact]
        public async void HandleShouldReturnUser_WhenExistById()
        {
            //Arrange
            var user = _fixture.Create<User>();

            var repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            IRequestHandler<GetDetailUserQuery, DetailUserDto> sut = _fixture.Create<GetDetailUserHandler>();

            //Act

            var response = await sut.Handle(new GetDetailUserQuery { Id = user.Id }, CancellationToken.None);

            //Assert
            repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            response.Id.Should().Be(user.Id);
            response.FirstName.Should().Be(user.FirstName);
            response.LastName.Should().Be(user.LastName);
            response.Email.Should().Be(user.Email);
        }

        [Fact]
        public async void HandleShouldThrowsException_WhenNotExistById()
        {
            //Arrange
            var user = _fixture.Create<User>();

            var repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as User);

            IRequestHandler<GetDetailUserQuery, DetailUserDto> sut = _fixture.Create<GetDetailUserHandler>();

            //Act

            var response = async () => await sut.Handle(new GetDetailUserQuery { Id = user.Id }, CancellationToken.None);

            //Assert
            await response.Should().ThrowExactlyAsync<EntityNotExistException>().WithMessage($"Entity User with Id = {user.Id} not exist");
        }
    }
}