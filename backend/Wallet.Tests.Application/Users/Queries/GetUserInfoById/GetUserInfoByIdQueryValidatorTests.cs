using System;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Wallet.Application.Users.Queries.GetUserInfoById;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetUserInfoById
{
    public class GetUserInfoByIdQueryValidatorTests
    {
        private readonly GetUserInfoByIdQueryValidator _validator;

        public GetUserInfoByIdQueryValidatorTests()
        {
            _validator = new GetUserInfoByIdQueryValidator();
        }

        [Fact]
        public async Task Should_HaveError_When_UserId_IsEmpty()
        {
            // Arrange
            var query = new GetUserInfoByIdQuery(Guid.Empty);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("UserId is required to retrieve user information.");
        }

        [Fact]
        public async Task Should_NotHaveError_When_UserId_IsValid()
        {
            // Arrange
            var query = new GetUserInfoByIdQuery(Guid.NewGuid());

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
