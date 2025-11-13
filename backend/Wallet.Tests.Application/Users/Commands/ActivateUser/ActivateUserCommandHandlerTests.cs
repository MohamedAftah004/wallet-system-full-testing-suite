using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.Commands.ActivateUser;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.ActivateUser
{
    public class ActivateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ActivateUserCommandHandler _handler;

        public ActivateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new ActivateUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrow_EntityNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new ActivateUserCommand(Guid.NewGuid());
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
        public async Task Handle_ShouldThrow_InvalidOperationException_WhenUserAlreadyActive()
        {
            // Arrange
            var user = new User("mohamed aftah", "mohamd@example.com", "123456789", "hashedPass");
            user.Activate(); 

            var command = new ActivateUserCommand(user.Id);
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*already active*");
        }

        [Fact]
        public async Task Handle_Should_Call_ActivateAsync_When_UserExists_And_IsInactive()
        {
            // Arrange
            var user = new User("mohamed aftah", "mohamd@example.com", "123456789", "hashedPass");

            var command = new ActivateUserCommand(user.Id);
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(repo => repo.ActivateAsync(command.UserId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _userRepositoryMock.Verify(repo => repo.ActivateAsync(command.UserId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
