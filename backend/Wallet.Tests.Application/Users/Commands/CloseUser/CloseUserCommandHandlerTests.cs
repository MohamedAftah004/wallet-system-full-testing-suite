using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.Commands.CloseUser;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.CloseUser
{
    public class CloseUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CloseUserCommandHandler _handler;

        public CloseUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new CloseUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrow_EntityNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new CloseUserCommand(Guid.NewGuid());
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"*{command.UserId}*");
        }

        [Fact]
        public async Task Handle_ShouldThrow_InvalidOperationException_WhenUserIsPendingActivation()
        {
            // Arrange
            var user = new User("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "hashedPass");

            var command = new CloseUserCommand(user.Id);
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*not active*");
        }

        [Fact]
        public async Task Handle_ShouldThrow_InvalidOperationException_WhenUserAlreadyClosed()
        {
            // Arrange
            var user = new User("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "hashedPass");
            user.Close();

            var command = new CloseUserCommand(user.Id);
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*already Closed*");
        }

        [Fact]
        public async Task Handle_Should_Call_CloseAsync_When_UserExists_And_IsActive()
        {
            // Arrange
            var user = new User("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "hashedPass");
            user.Activate();

            var command = new CloseUserCommand(user.Id);
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(repo => repo.CloseAsync(command.UserId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _userRepositoryMock.Verify(repo => repo.CloseAsync(command.UserId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
