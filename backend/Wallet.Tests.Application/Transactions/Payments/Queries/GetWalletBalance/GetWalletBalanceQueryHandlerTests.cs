using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.Payments.DTOs;
using Wallet.Application.Transactions.Payments.Queries.GetWalletBalance;
using Wallet.Domain.Entities;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.Payments.Queries.GetWalletBalance
{
    public class GetWalletBalanceQueryHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly GetWalletBalanceQueryHandler _handler;

        public GetWalletBalanceQueryHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _handler = new GetWalletBalanceQueryHandler(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenWalletNotFound()
        {
            // Arrange
            var query = new GetWalletBalanceQuery(Guid.NewGuid());
            _walletRepositoryMock.Setup(x => x.GetByIdAsync(query.WalletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Wallet.Domain.Entities.Wallet?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnWalletBalanceDto_WhenWalletExists()
        {
            // Arrange
            var wallet = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.Deduct(250.75m);

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var query = new GetWalletBalanceQuery(wallet.Id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WalletBalanceDto>(result);
            Assert.Equal(wallet.Id, result.WalletId);
            Assert.Equal(250.75m, result.Amount);
            Assert.Equal("USD", result.CurrencyCode);
            Assert.Equal("$", result.CurrencySymbol);
            Assert.Equal(wallet.UpdatedAt ?? wallet.CreatedAt, result.LastUpdated);
        }
    }
}
