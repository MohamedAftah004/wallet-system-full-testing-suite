using FluentValidation.TestHelper;
using System;
using Wallet.Application.Users.Commands.DisableUser;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.DisableUser
{
    public class DisableUserCommandValidatorTests
    {
        private readonly DisableUserCommandValidator _validator;

        public DisableUserCommandValidatorTests()
        {
            _validator = new DisableUserCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var command = new DisableUserCommand(Guid.Empty);

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
            var command = new DisableUserCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.UserId);
        }
    }
}
