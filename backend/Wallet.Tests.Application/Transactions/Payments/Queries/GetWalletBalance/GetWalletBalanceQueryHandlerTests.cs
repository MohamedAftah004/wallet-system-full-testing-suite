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

      
    }
}
