using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.DTOs;
using Wallet.Application.Wallets.Queries.GetWalletInfoById;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;
using CurrentWallet = Wallet.Domain.Entities.Wallet;

namespace Wallet.Tests.Application.Wallets.Queries.GetWalletInfoById
{
    public class GetWalletInfoByIdQueryHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly GetWalletInfoByIdQueryHandler _handler;

        public GetWalletInfoByIdQueryHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _handler = new GetWalletInfoByIdQueryHandler(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnWalletDto_WhenWalletExists()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var wallet = new CurrentWallet(userId, Currency.FromCode("EGP"), WalletStatus.Active);

            _walletRepositoryMock
                .Setup(x => x.GetByIdAsync(walletId , CancellationToken.None))
                .ReturnsAsync(wallet);

            var query = new GetWalletInfoByIdQuery(walletId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(wallet.Id, result.Id);
            Assert.Equal(wallet.UserId, result.UserId);
            Assert.Equal(wallet.Balance.Amount, result.Balance.Amount);
            Assert.Equal(wallet.Balance.Currency.Code, result.Balance.CurrencyCode);
            Assert.Equal(wallet.Status.ToString(), result.Status.Code);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenWalletNotFound()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            _walletRepositoryMock
                .Setup(x => x.GetByIdAsync(walletId, CancellationToken.None))
                .ReturnsAsync((CurrentWallet)null);

            var query = new GetWalletInfoByIdQuery(walletId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
