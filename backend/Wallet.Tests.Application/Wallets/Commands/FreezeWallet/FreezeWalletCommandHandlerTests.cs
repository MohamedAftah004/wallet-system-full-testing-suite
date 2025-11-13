using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Wallets.Commands.FreezeWallet;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;
using CurrentWallet = Wallet.Domain.Entities.Wallet;

namespace Wallet.Tests.Application.Wallets.Commands.FreezeWallet
{
    public class FreezeWalletCommandHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly FreezeWalletCommandHandler _handler;

        public FreezeWalletCommandHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _handler = new FreezeWalletCommandHandler(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldFreezeWallet_WhenWalletIsActive()
        {
            // Arrange
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("EGP"), WalletStatus.Active);
            var command = new FreezeWalletCommand(wallet.Id);

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            _walletRepositoryMock.Setup(x => x.UpdateAsync(wallet, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(WalletStatus.Frozen, wallet.Status);
            _walletRepositoryMock.Verify(x => x.UpdateAsync(wallet, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenWalletNotFound()
        {
            // Arrange
            var command = new FreezeWalletCommand(Guid.NewGuid());
            _walletRepositoryMock.Setup(x => x.GetByIdAsync(command.WalletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CurrentWallet)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenWalletAlreadyFrozen()
        {
            // Arrange
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("EGP"), WalletStatus.Frozen);
            var command = new FreezeWalletCommand(wallet.Id);

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenWalletDisabled()
        {
            // Arrange
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("EGP"), WalletStatus.Disabled);
            var command = new FreezeWalletCommand(wallet.Id);

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
