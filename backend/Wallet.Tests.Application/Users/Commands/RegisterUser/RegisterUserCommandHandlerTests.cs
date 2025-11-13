using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.Commands.RegisterUser;
using Wallet.Domain.Entities;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrow_DuplicateEmailException_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new RegisterUserCommand("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "password123");

            _userRepositoryMock
                .Setup(r => r.ExsistByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEmailException>()
                .WithMessage($"*{command.Email}*");
        }

        [Fact]
        public async Task Handle_ShouldThrow_DuplicatePhoneException_WhenPhoneAlreadyExists()
        {
            // Arrange
            var command = new RegisterUserCommand("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "password123");

            _userRepositoryMock
                .Setup(r => r.ExsistByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.ExsistByPhoneAsync(command.PhoneNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicatePhoneException>()
                .WithMessage($"*{command.PhoneNumber}*");
        }

        [Fact]
        public async Task Handle_Should_Add_User_And_Return_Id_When_Valid_Request()
        {
            // Arrange
            var command = new RegisterUserCommand("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "password123");

            _userRepositoryMock
                .Setup(r => r.ExsistByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.ExsistByPhoneAsync(command.PhoneNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _passwordHasherMock
                .Setup(p => p.Hash(command.Password))
                .Returns("hashedPassword");

            _userRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
