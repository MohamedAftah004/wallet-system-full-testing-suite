using FluentValidation.TestHelper;
using Wallet.Application.Users.Commands.ActivateUser;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.ActivateUser
{
    public class ActivateUserCommandValidatorTests
    {
        private readonly ActivateUserCommandValidator _validator;

        public ActivateUserCommandValidatorTests()
        {
            _validator = new ActivateUserCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var command = new ActivateUserCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserId)
                  .WithErrorMessage("UserId is required.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            // Arrange
            var command = new ActivateUserCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.UserId);
        }
    }
}
