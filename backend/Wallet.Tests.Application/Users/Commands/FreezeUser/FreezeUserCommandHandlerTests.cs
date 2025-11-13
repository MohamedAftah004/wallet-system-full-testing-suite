using FluentValidation.TestHelper;
using System;
using Wallet.Application.Users.Commands.FreezeUser;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.FreezeUser
{
    public class FreezeUserCommandValidatorTests
    {
        private readonly FreezeUserCommandValidator _validator;

        public FreezeUserCommandValidatorTests()
        {
            _validator = new FreezeUserCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var command = new FreezeUserCommand(Guid.Empty);

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
            var command = new FreezeUserCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.UserId);
        }
    }
}
