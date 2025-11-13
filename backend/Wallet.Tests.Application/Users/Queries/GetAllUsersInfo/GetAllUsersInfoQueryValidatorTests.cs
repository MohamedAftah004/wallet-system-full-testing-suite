using FluentValidation.TestHelper;
using Wallet.Application.Users.Queries.GetAllUsersInfo;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetAllUsersInfo
{
    public class GetAllUsersInfoQueryValidatorTests
    {
        private readonly GetAllUsersInfoQueryValidator _validator;

        public GetAllUsersInfoQueryValidatorTests()
        {
            _validator = new GetAllUsersInfoQueryValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Page_Is_Less_Than_One()
        {
            // Arrange
            var query = new GetAllUsersInfoQuery { Page = 0, Size = 10 };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Page);
        }

        [Fact]
        public void Should_Have_Error_When_Size_Is_Not_In_Range()
        {
            // Arrange
            var query = new GetAllUsersInfoQuery { Page = 1, Size = 150 };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Size);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Page_And_Size_Are_Valid()
        {
            // Arrange
            var query = new GetAllUsersInfoQuery { Page = 1, Size = 10 };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
