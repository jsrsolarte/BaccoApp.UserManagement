using AutoFixture;
using AutoFixture.AutoMoq;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Specifications;
using FluentAssertions;
using Xunit;

namespace BaccoApp.UserManagement.Domain.Tests.Specifications
{
    public class FindUserByEmailSpecShould
    {
        private readonly IFixture _fixture;

        public FindUserByEmailSpecShould()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Fact]
        public void FindUserByEmail_ReturnsUniqueUser()
        {
            //Arrange
            var users = _fixture.CreateMany<User>(100);

            var search = users.ElementAt(50).Email;

            var spec = new FindUserByEmailSpec(search);

            //Act

            var result = spec.Evaluate(users).FirstOrDefault();

            //Assert

            result.Should().Be(users.ElementAt(50));
        }
    }
}