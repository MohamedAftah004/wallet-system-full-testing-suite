using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.History.Queries.GetAllTransactionsInfo;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Wallet.Application.Common.Models;
using Xunit;

namespace Wallet.Tests.Application.Transactions.History.Queries.GetAllTransactionsInfo
{
    public class GetAllTransactionsInfoQueryHandlerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly GetAllTransactionsInfoQueryHandler _handler;

        public GetAllTransactionsInfoQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetAllTransactionsInfoQueryHandler(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenTransactionsExist()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var transactions = new List<Transaction>
            {
                new Transaction(walletId, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp done"),
                new Transaction(walletId, Money.Create(50, Currency.FromCode("EGP")), TransactionType.Transfer, null, "Transfer pending"),
                new Transaction(walletId, Money.Create(75, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp done 2")
            };

            transactions[0].MarkCompleted();
            transactions[2].MarkCompleted();

            var queryable = transactions.AsQueryable();

            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(queryable);

            var query = new GetAllTransactionsInfoQuery
            {
                Page = 1,
                Size = 10
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(3, result.Items.Count());
            Assert.All(result.Items, item => Assert.NotEqual(Guid.Empty, item.WalletId));
        }

        [Fact]
        public async Task Handle_ShouldFilterByType_WhenTypeProvided()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var transactions = new List<Transaction>
            {
                new Transaction(walletId, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp done"),
                new Transaction(walletId, Money.Create(50, Currency.FromCode("EGP")), TransactionType.Transfer, null, "Transfer pending")
            };

            var queryable = transactions.AsQueryable();

            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(queryable);

            var query = new GetAllTransactionsInfoQuery
            {
                Page = 1,
                Size = 10,
                Type = TransactionType.TopUp.ToString()
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(TransactionType.TopUp.ToString(), result.Items.First().Type);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenNoTransactionsFound()
        {
            // Arrange
            var queryable = new List<Transaction>().AsQueryable();

            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(queryable);

            var query = new GetAllTransactionsInfoQuery
            {
                Page = 1,
                Size = 10
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }
    }
}
