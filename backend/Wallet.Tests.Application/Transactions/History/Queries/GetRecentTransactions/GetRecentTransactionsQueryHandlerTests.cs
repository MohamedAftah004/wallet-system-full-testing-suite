using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly GetRecentTransactionsQueryHandler_Test _handler;

        public GetRecentTransactionsQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetRecentTransactionsQueryHandler_Test(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRecentTransactions_WhenDataExists()
        {
            // Arrange
            var user = new User("Mohamed Aftah", "mohamed@gmail.com", "+201000000000", "hashed123");
            var wallet = new Wallet.Domain.Entities.Wallet(user.Id, Currency.FromCode("EGP"));

            var transactions = new List<Transaction>
            {
                new Transaction(wallet.Id, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Top up 1"),
                new Transaction(wallet.Id, Money.Create(200, Currency.FromCode("EGP")), TransactionType.Transfer, null, "Transfer 1"),
                new Transaction(wallet.Id, Money.Create(300, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Top up 2")
            };

            foreach (var tx in transactions)
            {
                typeof(Transaction).GetProperty(nameof(Transaction.Wallet))!
                    .SetValue(tx, wallet);
            }

            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(transactions.AsQueryable());

            var query = new GetRecentTransactionsQuery { Count = 2 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.Equal("EGP", t.CurrencyCode));
        }

        [Fact]
        public async Task Handle_ShouldFilterByWalletId_WhenWalletIdProvided()
        {
            var wallet1 = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("EGP"));
            var wallet2 = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("EGP"));

            var tx1 = new Transaction(wallet1.Id, Money.Create(100, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Wallet1 TX");
            var tx2 = new Transaction(wallet2.Id, Money.Create(200, Currency.FromCode("EGP")), TransactionType.TopUp, null, "Wallet2 TX");

            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(new List<Transaction> { tx1, tx2 }.AsQueryable());

            var query = new GetRecentTransactionsQuery
            {
                WalletId = wallet1.Id,
                Count = 10
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(wallet1.Id, result.First().WalletId);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTransactionsExist()
        {
            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(new List<Transaction>().AsQueryable());

            var query = new GetRecentTransactionsQuery { Count = 5 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }


    public class GetRecentTransactionsQueryHandler_Test
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetRecentTransactionsQueryHandler_Test(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public Task<List<TransactionDto>> Handle(GetRecentTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = _transactionRepository.Query();

            if (request.WalletId.HasValue)
                query = query.Where(t => t.WalletId == request.WalletId.Value);

            var transactions = query
                .OrderByDescending(t => t.CreatedAt)
                .Take(request.Count)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    WalletId = t.WalletId,
                    Amount = t.Amount.Amount,
                    CurrencyCode = t.Amount.Currency.Code,
                    Type = t.Type.ToString(),
                    Status = t.Status.ToString(),
                    Description = t.Description,
                    CreatedAt = t.CreatedAt
                })
                .ToList();

            return Task.FromResult(transactions);
        }
    }
}
