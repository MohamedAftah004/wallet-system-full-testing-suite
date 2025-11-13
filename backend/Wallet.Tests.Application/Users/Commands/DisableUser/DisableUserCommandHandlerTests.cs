using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.Commands.DisableUser;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;

namespace Wallet.Tests.Application.Users.Commands.DisableUser
{
    public class DisableUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DisableUserCommandHandler _handler;

        public DisableUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DisableUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrow_EntityNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new DisableUserCommand(Guid.NewGuid());
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
        public async Task Handle_ShouldThrow_InvalidOperationException_WhenUserIsNotActive()
        {
            // Arrange
            var user = new User("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "hashedPass");

            var command = new DisableUserCommand(user.Id);
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*Not-Active*");
        }

        [Fact]
        public async Task Handle_Should_Call_DisableAsync_When_UserExists_And_IsActive()
        {
            // Arrange
            var user = new User("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "hashedPass");
            user.Activate();

            var command = new DisableUserCommand(user.Id);
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(repo => repo.DisableAsync(command.UserId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _userRepositoryMock.Verify(repo => repo.DisableAsync(command.UserId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
