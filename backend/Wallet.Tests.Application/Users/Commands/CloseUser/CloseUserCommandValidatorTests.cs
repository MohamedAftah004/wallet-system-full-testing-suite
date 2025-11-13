using FluentValidation.TestHelper;
using System;
using Wallet.Application.Users.Commands.CloseUser;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.CloseUser
{
    public class CloseUserCommandValidatorTests
    {
        private readonly CloseUserCommandValidator _validator;

        public CloseUserCommandValidatorTests()
        {
            _validator = new CloseUserCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var command = new CloseUserCommand(Guid.Empty);

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
            var command = new CloseUserCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.UserId);
        }
    }
}
