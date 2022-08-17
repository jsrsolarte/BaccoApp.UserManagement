using AutoFixture;
using FluentAssertions;
using Xunit;

namespace BaccoApp.UserManagement.Application.Tests
{
    public class PaginationShould
    {
        [Fact]
        public void DefaultRequestShouldHaveDefaulValues()
        {
            //Arrange
            var request = new PaginationRequest();

            //Assert
            request.Search.Should().BeNullOrEmpty();
            request.Page.Should().Be(1);
            request.RecordsPerPage.Should().Be(20);
        }

        [Fact]
        public void ResponseShouldReturnTotalPages()
        {
            //Arrange
            var request = new PaginationRequest();
            var fixture = new Fixture();
            var records = fixture.CreateMany<int>(request.RecordsPerPage);
            var response = new PaginationResponse<int>(request, records, 100);

            //Assert
            response.Page.Should().Be(1);
            response.RecordsPerPage.Should().Be(20);
            response.TotalPages.Should().Be(5);
            response.TotalRecords.Should().Be(100);
            response.Records.Should().BeEquivalentTo(records);
        }
    }
}