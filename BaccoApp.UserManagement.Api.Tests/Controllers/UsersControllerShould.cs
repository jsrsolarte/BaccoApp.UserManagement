using AutoFixture;
using AutoFixture.AutoMoq;
using BaccoApp.UserManagement.Api.Controllers;
using BaccoApp.UserManagement.Application;
using BaccoApp.UserManagement.Application.CreateUser;
using BaccoApp.UserManagement.Application.Users.Dtos;
using BaccoApp.UserManagement.Application.Users.GetUsers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace BaccoApp.UserManagement.Api.Tests.Controllers
{
    public class UsersControllerShould
    {
        private readonly IFixture _fixture;

        public UsersControllerShould()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task CreateUserShouldUseMediator_Return201StatusCodeAsync()
        {
            //Arrange

            var mediatrMock = _fixture.Freeze<Mock<IMediator>>();
            mediatrMock.Setup(_ => _.Send(It.IsAny<IRequest<DetailUserDto>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DetailUserDto());

            var sut = _fixture.Build<UsersController>().OmitAutoProperties().Create();

            //Act

            var response = await sut.CreateUser(new CreateUserCommand());

            //Assert

            mediatrMock.Verify(_ => _.Send(It.IsAny<IRequest<DetailUserDto>>(), It.IsAny<CancellationToken>()), Times.Once);
            response.Should().NotBeNull();
            response.Result.Should().BeOfType<CreatedAtActionResult>().Which.ActionName.Should().Be("CreateUser");
            response.Result.Should().BeOfType<CreatedAtActionResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
            response.Result.Should().BeOfType<CreatedAtActionResult>().Which.Value.Should().BeOfType<DetailUserDto>();
        }

        [Fact]
        public async Task UpdateUserShouldUseMediator_Return200StatusCodeAsync()
        {
            //Arrange

            var mediatrMock = _fixture.Freeze<Mock<IMediator>>();
            mediatrMock.Setup(_ => _.Send(It.IsAny<IRequest<DetailUserDto>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DetailUserDto());

            var sut = _fixture.Build<UsersController>().OmitAutoProperties().Create();

            //Act

            var response = await sut.UpdateUser(Guid.NewGuid(), new EditableUserDto());

            //Assert

            mediatrMock.Verify(_ => _.Send(It.IsAny<IRequest<DetailUserDto>>(), It.IsAny<CancellationToken>()), Times.Once);
            response.Should().NotBeNull();
            response.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<DetailUserDto>();
        }

        [Fact]
        public async Task GetUsersShouldUseMediator_Return200StatusCodeAsync()
        {
            //Arrange

            var mediatrMock = _fixture.Freeze<Mock<IMediator>>();
            mediatrMock.Setup(_ => _.Send(It.IsAny<IRequest<PaginationResponse<ListUserDto>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PaginationResponse<ListUserDto>(new PaginationRequest(), new List<ListUserDto>(), 0));

            var sut = _fixture.Build<UsersController>().OmitAutoProperties().Create();

            //Act

            var response = await sut.GetUsers(new GetUsersQuery());

            //Assert

            mediatrMock.Verify(_ => _.Send(It.IsAny<IRequest<PaginationResponse<ListUserDto>>>(), It.IsAny<CancellationToken>()), Times.Once);
            response.Should().NotBeNull();
            response.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<PaginationResponse<ListUserDto>>();
        }
    }
}