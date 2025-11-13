using FluentAssertions;
using Wallet.Application.Common.Models;
using Xunit;

namespace Wallet.Tests.Application.Common.Models
{
    public class PagedResultTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties_Correctly()
        {
            // Arrange
            var items = new List<string> { "A", "B", "C" };
            int totalCount = 10;
            int page = 2;
            int size = 3;

            // Act
            var result = new PagedResult<string>(items, totalCount, page, size);

            // Assert
            result.Items.Should().BeEquivalentTo(items);
            result.TotalCount.Should().Be(totalCount);
            result.Page.Should().Be(page);
            result.Size.Should().Be(size);
        }

        [Fact]
        public void TotalPages_Should_Calculate_Correctly()
        {
            var result = new PagedResult<int>(new List<int>(), totalCount: 25, page: 1, size: 10);
            result.TotalPages.Should().Be(3);
        }

        [Fact]
        public void HasNextPage_Should_Be_True_When_Not_On_Last_Page()
        {
            var result = new PagedResult<int>(new List<int>(), totalCount: 30, page: 2, size: 10);
            result.HasNextPage.Should().BeTrue();
        }

        [Fact]
        public void HasNextPage_Should_Be_False_When_On_Last_Page()
        {
            var result = new PagedResult<int>(new List<int>(), totalCount: 30, page: 3, size: 10);
            result.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public void HasPreviousPage_Should_Be_True_When_Page_Greater_Than_1()
        {
            var result = new PagedResult<int>(new List<int>(), totalCount: 30, page: 2, size: 10);
            result.HasPreviousPage.Should().BeTrue();
        }

        [Fact]
        public void HasPreviousPage_Should_Be_False_When_On_First_Page()
        {
            var result = new PagedResult<int>(new List<int>(), totalCount: 30, page: 1, size: 10);
            result.HasPreviousPage.Should().BeFalse();
        }
    }
}
