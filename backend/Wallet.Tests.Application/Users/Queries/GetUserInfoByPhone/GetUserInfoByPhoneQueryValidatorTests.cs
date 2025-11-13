using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Wallet.Application.Users.Queries.GetUserInfoByPhone;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetUserInfoByPhone
{
    public class GetUserInfoByPhoneQueryValidatorTests
    {
        private readonly GetUserInfoByPhoneQueryValidator _validator;

        public GetUserInfoByPhoneQueryValidatorTests()
        {
            _validator = new GetUserInfoByPhoneQueryValidator();
        }

        [Fact]
        public async Task Should_HaveError_When_Phone_IsEmpty()
        {
            // Arrange
            var query = new GetUserInfoByPhoneQuery(string.Empty);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Phone)
                .WithErrorMessage("Phone is required to retrieve user information.");
        }

        [Fact]
        public async Task Should_NotHaveError_When_Phone_IsValid()
        {
            // Arrange
            var query = new GetUserInfoByPhoneQuery("+201000000000");

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }
    }
}
