using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.DTOs;
using Wallet.Application.Wallets.Queries.GetUserWallets;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;
using CurrentWallet = Wallet.Domain.Entities.Wallet;

namespace Wallet.Tests.Application.Wallets.Queries.GetUserWallets
{
    public class GetUserWalletsQueryHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly GetUserWalletsQueryHandler _handler;

        public GetUserWalletsQueryHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _handler = new GetUserWalletsQueryHandler(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnWalletDtos_WhenWalletsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var wallet1 = new CurrentWallet(userId, Currency.FromCode("EGP"), WalletStatus.Active);
            var wallet2 = new CurrentWallet(userId, Currency.FromCode("USD"), WalletStatus.Frozen);
            var wallets = new List<CurrentWallet> { wallet1, wallet2 };

            _walletRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallets);

            var query = new GetUserWalletsQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, w => Assert.Equal(userId, w.UserId));
            _walletRepositoryMock.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenNoWalletsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _walletRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CurrentWallet>());

            var query = new GetUserWalletsQuery(userId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenWalletsIsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _walletRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<CurrentWallet>)null);

            var query = new GetUserWalletsQuery(userId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
