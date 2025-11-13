using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.Commands.CloseWallet;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;
using CurrentWallet = Wallet.Domain.Entities.Wallet;

namespace Wallet.Tests.Application.Wallets.Commands.CloseWallet
{
    public class CloseWalletCommandHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly CloseWalletCommandHandler _handler;

        public CloseWalletCommandHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _handler = new CloseWalletCommandHandler(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDisableWallet_WhenWalletExistsAndIsActive()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("EGP"), WalletStatus.Active);

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(walletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var command = new CloseWalletCommand(walletId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _walletRepositoryMock.Verify(x => x.UpdateAsync(wallet, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(WalletStatus.Disabled, wallet.Status);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenWalletNotFound()
        {
            // Arrange
            var walletId = Guid.NewGuid();

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(walletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CurrentWallet)null);

            var command = new CloseWalletCommand(walletId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenWalletAlreadyDisabled()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("EGP"), WalletStatus.Disabled);

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(walletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var command = new CloseWalletCommand(walletId);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
