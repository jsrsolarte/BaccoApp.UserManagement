using BaccoApp.UserManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BaccoApp.UserManagement.Domain.Tests.Entities
{
    public class UserShould
    {
        [Fact]
        public void UserShould_CreateEmptyProperties()
        {
            var user = new User();

            user.FirstName.Should().BeNullOrEmpty();
            user.LastName.Should().BeNullOrEmpty();
            user.Email.Should().BeNullOrEmpty();
        }
    }
}