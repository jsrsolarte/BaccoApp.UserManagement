using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoMapper;
using BaccoApp.UserManagement.Application.Users;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Application.Users.UpdateBaseUser;
using BaccoApp.UserManagement.Application.Users.UpdateEditableUser;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Exceptions;
using BaccoApp.UserManagement.Domain.Ports;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace BaccoApp.UserManagement.Application.Tests.Users.UpdateEditableUser
{
    public class UpdateEditableUserHandlerShould
    {
        private readonly IFixture _fixture;

        public UpdateEditableUserHandlerShould()
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
            assertion.Verify(typeof(UpdateEditableUserHandler).GetConstructors());
        }

        [Fact]
        public async void HandleShouldUpdateUserIfExist_ReturnNewUser()
        {
            //Arrange
            var command = _fixture.Create<UpdateEditableUserCommand>();
            var user = _fixture.Build<User>()
                .With(_ => _.Id, command.Id)
                .Create();
            var repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            repositoryMock.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            User userUpdated = null!;
            repositoryMock.Setup(_ => _.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((u, _) => userUpdated = u)
                .Returns(Task.CompletedTask);

            IRequestHandler<UpdateEditableUserCommand, DetailUserDto> sut = _fixture.Create<UpdateEditableUserHandler>();

            //Act
            var response = await sut.Handle(command, CancellationToken.None);

            //Assert
            repositoryMock.Verify(_ => _.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            response.Id.Should().Be(user.Id);
            response.Email.Should().Be(user.Email);
            response.FirstName.Should().Be(command.User?.FirstName);
            response.LastName.Should().Be(command.User?.LastName);

            userUpdated.Id.Should().Be(user.Id);
            userUpdated.Email.Should().Be(user.Email);
            userUpdated.FirstName.Should().Be(command.User?.FirstName);
            userUpdated.LastName.Should().Be(command.User?.LastName);
        }

        [Fact]
        public async void HandleShouldThrowExceptionWhenUserNotExists()
        {
            //Arrange
            var command = _fixture.Create<UpdateEditableUserCommand>();

            var repositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            repositoryMock.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as User);

            IRequestHandler<UpdateEditableUserCommand, DetailUserDto> sut = _fixture.Create<UpdateEditableUserHandler>();

            //Act
            var response = async () => await sut.Handle(command, CancellationToken.None);

            //Assert
            await response.Should().ThrowExactlyAsync<EntityNotExistException>().WithMessage($"Entity User with Id = {command.Id} not exist");
            repositoryMock.Verify(_ => _.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}