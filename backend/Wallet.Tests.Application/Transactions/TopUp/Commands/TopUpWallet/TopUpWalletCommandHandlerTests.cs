using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.TopUp.Commands.TopUpWallet;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.TopUp.Commands
{
    public class TopUpWalletCommandHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly TopUpWalletCommandHandler _handler;

        public TopUpWalletCommandHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _handler = new TopUpWalletCommandHandler(
                _walletRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFound_WhenWalletNotFound()
        {
            // Arrange
            var command = new TopUpWalletCommand(Guid.NewGuid(), 100, "Test top-up");
            _walletRepositoryMock.Setup(x => x.GetByIdAsync(command.WalletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Wallet.Domain.Entities.Wallet?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperation_WhenWalletNotActive()
        {
            // Arrange
            var wallet = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("EGP"));
            var command = new TopUpWalletCommand(wallet.Id, 100, "Top-up inactive wallet");

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperation_WhenUserNotActive()
        {
            // Arrange
            var wallet = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("EGP"));
            wallet.Activate();

            var user = new User("Test User", "test@example.com", "01023203909" , "passhashed");

            var command = new TopUpWalletCommand(wallet.Id, 100, "Inactive user");

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(wallet.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

       
    }
}
