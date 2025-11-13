using System;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Wallet.Application.Users.Queries.GetUserDashboard;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetUserDashboard
{
    public class GetUserDashboardQueryValidatorTests
    {
        private readonly GetUserDashboardQueryValidator _validator;

        public GetUserDashboardQueryValidatorTests()
        {
            _validator = new GetUserDashboardQueryValidator();
        }

        [Fact]
        public async Task Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var query = new GetUserDashboardQuery(Guid.Empty);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("User ID is required to fetch dashboard data.");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            // Arrange
            var query = new GetUserDashboardQuery(Guid.NewGuid());

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
