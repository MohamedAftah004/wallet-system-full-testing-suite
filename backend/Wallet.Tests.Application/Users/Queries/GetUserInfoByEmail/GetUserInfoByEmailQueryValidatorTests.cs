using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Wallet.Application.Users.Queries.GetUserInfoByEmail;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetUserInfoByEmail
{
    public class GetUserInfoByEmailQueryValidatorTests
    {
        private readonly GetUserInfoByEmailQueryValidator _validator;

        public GetUserInfoByEmailQueryValidatorTests()
        {
            _validator = new GetUserInfoByEmailQueryValidator();
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Empty()
        {
            // Arrange
            var query = new GetUserInfoByEmailQuery(string.Empty);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required to retrieve user information.");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Email_Is_Provided()
        {
            // Arrange
            var query = new GetUserInfoByEmailQuery("mohamed@gmail.com");

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }
    }
}
