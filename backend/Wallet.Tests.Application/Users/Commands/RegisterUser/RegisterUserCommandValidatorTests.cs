using FluentValidation.TestHelper;
using System;
using Wallet.Application.Users.Commands.RegisterUser;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidatorTests
    {
        private readonly RegisterUserCommandValidator _validator;

        public RegisterUserCommandValidatorTests()
        {
            _validator = new RegisterUserCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FullName_Is_Empty()
        {
            // Arrange
            var command = new RegisterUserCommand("", "mohamed@gmail.com", "01000000000", "password123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FullName);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            // Arrange
            var command = new RegisterUserCommand("Mohamed Aftah", "", "01000000000", "password123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
        {
            // Arrange
            var command = new RegisterUserCommand("Mohamed Aftah", "mohamed@gmail.com", "invalid", "password123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Short()
        {
            // Arrange
            var command = new RegisterUserCommand("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var command = new RegisterUserCommand("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "password123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
