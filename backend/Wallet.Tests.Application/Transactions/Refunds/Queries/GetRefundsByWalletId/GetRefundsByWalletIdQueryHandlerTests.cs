using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.Refunds.DTOs;
using Wallet.Application.Transactions.Refunds.Queries.GetRefundsByWalletId;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.Refunds.Queries
{
    public class GetRefundsByWalletIdQueryHandlerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly GetRefundsByWalletIdQueryHandler _handler;

        public GetRefundsByWalletIdQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetRefundsByWalletIdQueryHandler(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRefunds_WhenTheyExist()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var transactions = new List<Transaction>
            {
                new Transaction(walletId, Money.Create(100, Currency.FromCode("USD")), TransactionType.Payment, null, "Reversed Tx 1"),
                new Transaction(walletId, Money.Create(200, Currency.FromCode("USD")), TransactionType.Payment, null, "Normal Tx")
            };

            transactions[0].MarkCompleted();
            transactions[0].MarkReversed("Refund successful");

            _transactionRepositoryMock
                .Setup(x => x.GetByWalletIdAsync(walletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactions);

            var query = new GetRefundsByWalletIdQuery(walletId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Reversed", result.First().Status);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFound_WhenNoRefundsFound()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var transactions = new List<Transaction>
            {
                new Transaction(walletId, Money.Create(100, Currency.FromCode("USD")), TransactionType.Payment, null, "Completed Tx")
            };

            transactions[0].MarkCompleted();

            _transactionRepositoryMock
                .Setup(x => x.GetByWalletIdAsync(walletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactions);

            var query = new GetRefundsByWalletIdQuery(walletId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }

    public class GetRefundsByWalletIdQueryValidatorTests
    {
        private readonly GetRefundsByWalletIdQueryValidator _validator = new();

        [Fact]
        public void Should_HaveError_WhenWalletIdIsEmpty()
        {
            var query = new GetRefundsByWalletIdQuery(Guid.Empty);
            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "WalletId");
        }

        [Fact]
        public void Should_PassValidation_WhenWalletIdIsValid()
        {
            var query = new GetRefundsByWalletIdQuery(Guid.NewGuid());
            var result = _validator.Validate(query);
            Assert.True(result.IsValid);
        }
    }
}
