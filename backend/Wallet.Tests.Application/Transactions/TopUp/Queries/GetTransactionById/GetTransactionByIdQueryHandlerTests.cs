using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Application.Transactions.TopUp.Queries.GetTransactionById;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.TopUp.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryHandlerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly GetTransactionByIdQueryHandler _handler;

        public GetTransactionByIdQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetTransactionByIdQueryHandler(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFound_WhenTransactionNotFound()
        {
            // Arrange
            var query = new GetTransactionByIdQuery(Guid.NewGuid());
            _transactionRepositoryMock.Setup(x => x.GetByIdAsync(query.TransactionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Transaction?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnTransactionResponse_WhenTransactionExists()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var transaction = new Transaction(walletId, Money.Create(250, Currency.FromCode("USD")), TransactionType.TopUp, "REF123", "Top-up Successful");

            _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction);

            var query = new GetTransactionByIdQuery(transaction.Id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TransactionResponseDto>(result);
            Assert.Equal(transaction.Id, result.Id);
            Assert.Equal(transaction.WalletId, result.WalletId);
            Assert.Equal(transaction.Amount.Amount, result.Amount);
            Assert.Equal(transaction.Amount.Currency.Code, result.CurrencyCode);
            Assert.Equal(transaction.Type.ToString(), result.Type);
            Assert.Equal(TransactionStatus.Completed.ToString(), result.Status);
            Assert.Equal(transaction.ReferenceId, result.ReferenceId);
            Assert.Equal(transaction.Description, result.Description);
        }
    }
}
