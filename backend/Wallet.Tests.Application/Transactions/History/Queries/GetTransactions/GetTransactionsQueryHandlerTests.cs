using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Models;
using Wallet.Application.Transactions.History.Queries.GetTransactions;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.History.Queries.GetTransactions
{
    public class GetTransactionsQueryHandlerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly GetTransactionsQueryHandler _handler;

        public GetTransactionsQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetTransactionsQueryHandler(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedTransactions_WhenDataExists()
        {
            // Arrange
            var walletId = Guid.NewGuid();

            var transactions = new List<Transaction>
            {
                new Transaction(walletId, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp 1"),
                new Transaction(walletId, Money.Create(200, Currency.FromCode("EGP")), TransactionType.Transfer, null, "Transfer 1"),
                new Transaction(walletId, Money.Create(300, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp 2")
            };

            _transactionRepositoryMock.Setup(x => x.Query()).Returns(transactions.AsQueryable());

            var query = new GetTransactionsQuery(walletId, null, null, null, null, 1, 2);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PagedResult<TransactionDto>>(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
            Assert.All(result.Items, t => Assert.Equal(walletId, t.WalletId));
        }

        [Fact]
        public async Task Handle_ShouldFilterByType_WhenTypeProvided()
        {
            // Arrange
            var walletId = Guid.NewGuid();

            var transactions = new List<Transaction>
            {
                new Transaction(walletId, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp 1"),
                new Transaction(walletId, Money.Create(200, Currency.FromCode("EGP")), TransactionType.Transfer, null, "Transfer 1")
            };

            _transactionRepositoryMock.Setup(x => x.Query()).Returns(transactions.AsQueryable());

            var query = new GetTransactionsQuery(walletId, "Transfer", null, null, null, 1, 2);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Transfer", result.Items.First().Type);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResult_WhenNoTransactionsFound()
        {
            // Arrange
            var walletId = Guid.NewGuid();

            _transactionRepositoryMock.Setup(x => x.Query())
                .Returns(new List<Transaction>().AsQueryable());

            var query = new GetTransactionsQuery(walletId, "TopUp", "Completed", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow, 1, 2);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }
    }
}
