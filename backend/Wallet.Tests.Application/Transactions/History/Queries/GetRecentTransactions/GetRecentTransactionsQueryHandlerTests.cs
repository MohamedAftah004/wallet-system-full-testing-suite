using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.History.Queries.GetRecentTransactions;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.History.Queries.GetRecentTransactions
{
    public class GetRecentTransactionsQueryHandlerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly GetRecentTransactionsQueryHandler _handler;

        public GetRecentTransactionsQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetRecentTransactionsQueryHandler(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRecentTransactions_WhenDataExists()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var user = new User("Mohamed Aftah", "mohamed@gmail.com", "+201000000000", "hashed123");
            var wallet = new Wallet.Domain.Entities.Wallet(user.Id, Currency.FromCode("EGP"));

            var transactions = new List<Transaction>
            {
                new Transaction(wallet.Id, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Top up 1"),
                new Transaction(wallet.Id, Money.Create(200, Currency.FromCode("EGP")), TransactionType.Transfer, null, "Transfer 1"),
                new Transaction(wallet.Id, Money.Create(300, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Top up 2")
            };

            // Link navigation properties manually (since EF not running)
            foreach (var tx in transactions)
            {
                typeof(Transaction).GetProperty(nameof(Transaction.Wallet))!
                    .SetValue(tx, wallet);
            }

            var queryable = transactions.AsQueryable();

            _transactionRepositoryMock.Setup(x => x.Query())
                .Returns(queryable);

            var query = new GetRecentTransactionsQuery
            {
                Count = 2
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.False(string.IsNullOrWhiteSpace(t.Type)));
            Assert.All(result, t => Assert.Equal("EGP", t.CurrencyCode));
        }

        [Fact]
        public async Task Handle_ShouldFilterByWalletId_WhenWalletIdProvided()
        {
            // Arrange
            var wallet1 = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("EGP"));
            var wallet2 = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("EGP"));

            var tx1 = new Transaction(wallet1.Id, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Wallet1 TX");
            var tx2 = new Transaction(wallet2.Id, Money.Create(200, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Wallet2 TX");

            var queryable = new List<Transaction> { tx1, tx2 }.AsQueryable();

            _transactionRepositoryMock.Setup(x => x.Query())
                .Returns(queryable);

            var query = new GetRecentTransactionsQuery
            {
                WalletId = wallet1.Id,
                Count = 10
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal(wallet1.Id, result.First().WalletId);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTransactionsExist()
        {
            // Arrange
            _transactionRepositoryMock.Setup(x => x.Query())
                .Returns(new List<Transaction>().AsQueryable());

            var query = new GetRecentTransactionsQuery { Count = 5 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
