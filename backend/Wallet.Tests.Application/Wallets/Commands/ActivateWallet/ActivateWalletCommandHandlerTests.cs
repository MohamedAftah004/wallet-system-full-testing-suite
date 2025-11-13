using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Wallets.Commands.ActivateWallet;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using CurrentWallet = Wallet.Domain.Entities.Wallet;
using Xunit;

namespace Wallet.Tests.Application.Wallets.Commands.ActivateWallet
{
    public class ActivateWalletCommandHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly ActivateWalletCommandHandler _handler;

        public ActivateWalletCommandHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _handler = new ActivateWalletCommandHandler(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldActivateWallet_WhenWalletIsPendingActivation()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("EGP"), WalletStatus.PendingActivation);
            _walletRepositoryMock.Setup(x => x.GetByIdAsync(walletId, It.IsAny<CancellationToken>())).ReturnsAsync(wallet);

            var command = new ActivateWalletCommand(walletId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(WalletStatus.Active, wallet.Status);
            _walletRepositoryMock.Verify(x => x.UpdateAsync(wallet, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenWalletNotFound()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            _walletRepositoryMock.Setup(x => x.GetByIdAsync(walletId, It.IsAny<CancellationToken>())).ReturnsAsync((CurrentWallet)null);

            var command = new ActivateWalletCommand(walletId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(WalletStatus.Active, "Wallet is already active.")]
        [InlineData(WalletStatus.Disabled, "Wallet is disabled and cannot be reactivated.")]
        [InlineData(WalletStatus.Frozen, "Wallet is frozen and must be unfrozen before activation.")]
        public async Task Handle_ShouldThrowException_WhenWalletStatusIsInvalid(WalletStatus status, string expectedMessage)
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("EGP"), status);
            _walletRepositoryMock.Setup(x => x.GetByIdAsync(walletId, It.IsAny<CancellationToken>())).ReturnsAsync(wallet);

            var command = new ActivateWalletCommand(walletId);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(expectedMessage, ex.Message);
            _walletRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CurrentWallet>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
