using FluentAssertions;
using System;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;

namespace Wallet.Tests.Domain.Users
{
    public class UserTests
    {
        #region Constructor

        [Fact]
        public void Constructor_ShouldInitializeUser_WithPendingStatus()
        {
            // Arrange
            var fullName = "John Doe";
            var email = "john@example.com";
            var phone = "123456789";
            var passwordHash = "hashedPassword123";

            // Act
            var user = new User(fullName, email, phone, passwordHash);

            // Assert
            user.FullName.Should().Be(fullName);
            user.Email.Should().Be(email);
            user.PhoneNumber.Should().Be(phone);
            user.PasswordHash.Should().Be(passwordHash);
            user.UserStatus.Should().Be(UserStatus.PendingActivation);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrow_WhenFullNameIsInvalid(string invalidName)
        {
            // Act
            var act = () => new User(invalidName, "email@test.com", "123", "pwd");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*Fullname*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrow_WhenEmailIsInvalid(string invalidEmail)
        {
            var act = () => new User("John", invalidEmail, "123", "pwd");

            act.Should().Throw<ArgumentException>().WithMessage("*Email*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrow_WhenPhoneIsInvalid(string invalidPhone)
        {
            var act = () => new User("John", "email@test.com", invalidPhone, "pwd");

            act.Should().Throw<ArgumentException>().WithMessage("*Phone*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrow_WhenPasswordIsInvalid(string invalidPassword)
        {
            var act = () => new User("John", "email@test.com", "123", invalidPassword);

            act.Should().Throw<ArgumentException>().WithMessage("*Password*");
        }

        #endregion

        #region Update Methods

        [Fact]
        public void UpdateFullName_ShouldUpdate_WhenValid()
        {
            // Arrange
            var user = new User("John", "email@test.com", "123", "pwd");

            // Act
            user.UpdateFullName("Johnny");

            // Assert
            user.FullName.Should().Be("Johnny");
        }

        [Fact]
        public void UpdateFullName_ShouldThrow_WhenEmpty()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            var act = () => user.UpdateFullName("");

            act.Should().Throw<ArgumentException>().WithMessage("*Full name*");
        }

        [Fact]
        public void UpdatePhoneNumber_ShouldUpdate_WhenValid()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            user.UpdatePhoneNumber("987654321");

            user.PhoneNumber.Should().Be("987654321");
        }

        [Fact]
        public void UpdatePhoneNumber_ShouldThrow_WhenEmpty()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            var act = () => user.UpdatePhoneNumber(" ");

            act.Should().Throw<ArgumentException>().WithMessage("*Phone number*");
        }

        [Fact]
        public void UpdatePassword_ShouldUpdate_WhenValid()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            user.UpdatePassword("newPassword123");

            user.PasswordHash.Should().Be("newPassword123");
        }

        [Fact]
        public void UpdatePassword_ShouldThrow_WhenEmpty()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            var act = () => user.UpdatePassword("");

            act.Should().Throw<ArgumentException>().WithMessage("*Password*");
        }

        #endregion

        #region Status Changes

        [Fact]
        public void Activate_ShouldChangeStatusToActive()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            user.Activate();

            user.UserStatus.Should().Be(UserStatus.Active);
        }

        [Fact]
        public void Close_ShouldChangeStatusToClosed()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            user.Close();

            user.UserStatus.Should().Be(UserStatus.Closed);
        }

        [Fact]
        public void Freeze_ShouldChangeStatusToFrozen()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            user.Freeze();

            user.UserStatus.Should().Be(UserStatus.Frozen);
        }

        [Fact]
        public void Disable_ShouldChangeStatusToDisabled()
        {
            var user = new User("John", "email@test.com", "123", "pwd");

            user.Disable();

            user.UserStatus.Should().Be(UserStatus.Disabled);
        }

        #endregion
    }
}
