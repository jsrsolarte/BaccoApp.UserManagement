using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoMapper;
using BaccoApp.UserManagement.Application.CreateUser;
using BaccoApp.UserManagement.Application.Users;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Exceptions;
using BaccoApp.UserManagement.Domain.Ports;
using BaccoApp.UserManagement.Domain.Specifications;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace BaccoApp.UserManagement.Application.Tests.Users.CreateUser
{
    public class CreateUserHandlerShould
    {
        private readonly IFixture _fixture;

        public CreateUserHandlerShould()
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
            assertion.Verify(typeof(CreateUserHandler).GetConstructors());
        }

        [Fact]
        public async void HandleShouldCreateUserIfNotExist_ReturnNewUser()
        {
            //Arrange
            var command = _fixture.Create<CreateUserCommand>();

            var repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            repositoryMock.Setup(_ => _.GetBySpecAsync(It.IsAny<FindUserByEmailSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as User);
            User user = null!;
            repositoryMock.Setup(_ => _.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((u, _) => user = u)
                .Returns(Task.CompletedTask);

            IRequestHandler<CreateUserCommand, DetailUserDto> sut = _fixture.Create<CreateUserHandler>();

            //Act
            var response = await sut.Handle(command, CancellationToken.None);

            //Assert
            repositoryMock.Verify(_ => _.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            user.Email.Should().Be(command.Email);
            response.Email.Should().Be(command.Email);
        }

        [Fact]
        public async void HandleShouldThrowExceptionWhenUserExists()
        {
            //Arrange
            var command = _fixture.Create<CreateUserCommand>();

            var repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            repositoryMock.Setup(_ => _.GetBySpecAsync(It.IsAny<FindUserByEmailSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(new User());
            User user = null!;
            repositoryMock.Setup(_ => _.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((u, _) => user = u)
                .Returns(Task.CompletedTask);

            IRequestHandler<CreateUserCommand, DetailUserDto> sut = _fixture.Create<CreateUserHandler>();

            //Act
            var response = async () => await sut.Handle(command, CancellationToken.None);

            //Assert
            await response.Should().ThrowExactlyAsync<EntityAlreadyExistException>().WithMessage($"Entity User with Email = {command.Email} already exist");
            repositoryMock.Verify(_ => _.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void CreateUserCommandShould_CreateEmptyProperties()
        {
            var user = new CreateUserCommand();

            user.FirstName.Should().BeNullOrEmpty();
            user.LastName.Should().BeNullOrEmpty();
            user.Email.Should().BeNullOrEmpty();
        }
    }
}