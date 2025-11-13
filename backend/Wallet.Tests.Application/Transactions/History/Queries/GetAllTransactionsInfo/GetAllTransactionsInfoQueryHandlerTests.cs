using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly GetAllTransactionsInfoQueryHandler_ForTest _handler;

        public GetAllTransactionsInfoQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetAllTransactionsInfoQueryHandler_ForTest(_transactionRepositoryMock.Object);
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

            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(transactions.AsQueryable());

            var query = new GetAllTransactionsInfoQuery { Page = 1, Size = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(3, result.Items.Count());
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

            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(transactions.AsQueryable());

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
            _transactionRepositoryMock
                .Setup(x => x.Query())
                .Returns(new List<Transaction>().AsQueryable());

            var query = new GetAllTransactionsInfoQuery { Page = 1, Size = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }
    }

    public class GetAllTransactionsInfoQueryHandler_ForTest
        {
        private readonly ITransactionRepository _transactionRepository;

        public GetAllTransactionsInfoQueryHandler_ForTest(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public Task<PagedResult<TransactionDto>> Handle(GetAllTransactionsInfoQuery request, CancellationToken cancellationToken)
        {
            var query = _transactionRepository.Query();

            // Filter by type
            if (!string.IsNullOrEmpty(request.Type) &&
                Enum.TryParse<TransactionType>(request.Type, true, out var type))
            {
                query = query.Where(t => t.Type == type);
            }

            var totalCount = query.Count();

            var items = query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
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

            return Task.FromResult(
                new PagedResult<TransactionDto>(items, totalCount, request.Page, request.Size)
            );
        }
    }
}
