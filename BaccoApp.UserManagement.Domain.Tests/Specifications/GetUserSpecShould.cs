using AutoFixture;
using AutoFixture.AutoMoq;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Specifications;
using FluentAssertions;
using Xunit;

namespace BaccoApp.UserManagement.Domain.Tests.Specifications
{
    public class GetUserSpecShould
    {
        private readonly IFixture _fixture;

        public GetUserSpecShould()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Fact]
        public void GetUserSpecWithSearch_ReturnsUsersList()
        {
            //Arrange
            var users = _fixture.CreateMany<User>(100);

            var search = users.ElementAt(50).FirstName[^4..];

            var usersFilters = users.Where(_ => _.FirstName.Contains(search) || _.LastName.Contains(search));
            var spec = new GetUsersSpec(search);

            //Act

            var result = spec.Evaluate(users);

            //Assert

            result.Count().Should().Be(usersFilters.Count());
            result.Should().BeEquivalentTo(usersFilters);
        }

        [Fact]
        public void GetUserSpecWithSearch_ReturnsUsersListWithExactlyEmail()
        {
            //Arrange
            var users = _fixture.CreateMany<User>(100);

            var search = users.ElementAt(50).Email;

            var usersFilters = users.Where(_ => _.Email.Equals(search));
            var spec = new GetUsersSpec(search);

            //Act

            var result = spec.Evaluate(users);

            //Assert

            result.Count().Should().Be(usersFilters.Count());
            result.Should().BeEquivalentTo(usersFilters);
        }

        [Fact]
        public void GetUserSpecWithSearchAndPagination_ReturnsUserList()
        {
            //Arrange
            var users = _fixture.CreateMany<User>(100);

            var search = users.ElementAt(50).FirstName[^4..];
            const int pageSize = 10;
            var usersFilters = users.Where(_ => _.FirstName.Contains(search) || _.LastName.Contains(search)).Take(pageSize);
            var spec = new GetUsersSpec(search, pageSize, 1);

            //Act

            var result = spec.Evaluate(users);

            //Assert

            result.Count().Should().Be(usersFilters.Count());
            result.Should().BeEquivalentTo(usersFilters);
        }

        [Fact]
        public void GetUserSpecWithPagination_ReturnsUserList()
        {
            //Arrange
            var users = _fixture.CreateMany<User>(100);

            const int pageSize = 10;
            const int page = 5;

            var usersFilters = users.Skip((page - 1) * pageSize).Take(pageSize);
            var spec = new GetUsersSpec(string.Empty, pageSize, page);

            //Act

            var result = spec.Evaluate(users);

            //Assert

            result.Count().Should().Be(usersFilters.Count());
            result.Should().BeEquivalentTo(usersFilters);
        }

        [Fact]
        public void GetUserSpecWithSearchAndPagination_ReturnsFilterUserList()
        {
            //Arrange
            var users = _fixture.CreateMany<User>(100);

            var search = users.ElementAt(50).FirstName[^4..];
            const int pageSize = 100;
            var usersFilters = users.Where(_ => _.FirstName.Contains(search) || _.LastName.Contains(search)).Take(pageSize);
            var spec = new GetUsersSpec(search, pageSize, 1);

            //Act

            var result = spec.Evaluate(users);

            //Assert

            result.Count().Should().Be(usersFilters.Count());
            result.Should().BeEquivalentTo(usersFilters);
        }

        [Fact]
        public void GetUserSpecWithSearchAndPagination_ReturnsUser()
        {
            //Arrange
            var users = _fixture.CreateMany<User>(100);

            var search = users.ElementAt(50).FirstName;
            const int pageSize = 100;
            var usersFilters = users.Where(_ => _.FirstName.Contains(search) || _.LastName.Contains(search)).Take(pageSize);
            var spec = new GetUsersSpec(search, pageSize, 1);

            //Act

            var result = spec.Evaluate(users);

            //Assert

            result.Count().Should().Be(usersFilters.Count());
            result.Should().BeEquivalentTo(usersFilters);
        }
    }
}