using AutoFixture;
using AutoFixture.AutoMoq;
using BaccoApp.UserManagement.Api.Filters;
using BaccoApp.UserManagement.Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Net;
using Xunit;

namespace BaccoApp.UserManagement.Api.Tests.Filters
{
    public class AppExceptionFilterAttributeShould
    {
        private readonly IFixture fixture;

        public AppExceptionFilterAttributeShould()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Fact]
        public void ChangeStatusCodeTo400WhenIsAppException()
        {
            // Arrange
            var appExceptionFilterAttribute = fixture.Create<AppExceptionFilterAttribute>();
            fixture.Customize<ActionContext>(_ => _.OmitAutoProperties()
                .With(_ => _.HttpContext, new DefaultHttpContext())
                .With(_ => _.RouteData, new RouteData())
                .With(_ => _.ActionDescriptor, new ActionDescriptor()));

            var resultExecutingContext = fixture.Build<ExceptionContext>()
                .OmitAutoProperties()
                .With(_ => _.Exception, new AppException())
                .Create();

            // Act
            appExceptionFilterAttribute.OnException(resultExecutingContext);

            // Assert
            resultExecutingContext.HttpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            resultExecutingContext.Result.Should().NotBeNull();
        }

        [Fact]
        public void ChangeStatusCodeTo500WhenIsGenericException()
        {
            // Arrange
            var appExceptionFilterAttribute = fixture.Create<AppExceptionFilterAttribute>();
            fixture.Customize<ActionContext>(_ => _.OmitAutoProperties()
                .With(_ => _.HttpContext, new DefaultHttpContext())
                .With(_ => _.RouteData, new RouteData())
                .With(_ => _.ActionDescriptor, new ActionDescriptor()));

            var resultExecutingContext = fixture.Build<ExceptionContext>()
                .OmitAutoProperties()
                .With(_ => _.Exception, new Exception())
                .Create();

            // Act
            appExceptionFilterAttribute.OnException(resultExecutingContext);

            // Assert
            resultExecutingContext.HttpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            resultExecutingContext.Result.Should().NotBeNull();
        }
    }
}