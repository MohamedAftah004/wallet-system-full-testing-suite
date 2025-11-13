using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Services;
using Wallet.Application.Transactions.Refunds.Commands.RefundTransaction;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.Refunds.Commands
{
    public class RefundTransactionCommandHandlerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IUserValidator> _userValidatorMock;
        private readonly RefundTransactionCommandHandler _handler;

        public RefundTransactionCommandHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _userValidatorMock = new Mock<IUserValidator>();
            _handler = new RefundTransactionCommandHandler(
                _transactionRepositoryMock.Object,
                _walletRepositoryMock.Object,
                _userValidatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFound_WhenTransactionNotFound()
        {
            // Arrange
            var command = new RefundTransactionCommand(Guid.NewGuid());
            _transactionRepositoryMock.Setup(x => x.GetByIdAsync(command.TransactionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Transaction?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperation_WhenTransactionNotCompleted()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(100, Currency.FromCode("USD")), TransactionType.Payment, null, "Pending Tx");
            // manually keep it pending (default)

            _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction);

            var command = new RefundTransactionCommand(transaction.Id);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperation_WhenTransactionOlderThan7Days()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(100, Currency.FromCode("USD")), TransactionType.Payment, null, "Completed Tx");
            transaction.MarkCompleted();
            transaction.SetCreatedAt(DateTime.UtcNow.AddDays(-10)); // simulate old transaction

            _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction);

            var command = new RefundTransactionCommand(transaction.Id);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldRefundTransaction_WhenValid()
        {
            // Arrange
            var wallet = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.Activate();

            var transaction = new Transaction(wallet.Id, Money.Create(100, Currency.FromCode("USD")), TransactionType.Payment, null, "Completed Tx");
            transaction.MarkCompleted();

            _transactionRepositoryMock.Setup(x => x.GetByIdAsync(transaction.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction);

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            _userValidatorMock.Setup(x => x.EnsureUserIsActiveAsync(wallet.UserId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new RefundTransactionCommand(transaction.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            Assert.Equal(TransactionStatus.Reversed, transaction.Status);
            _walletRepositoryMock.Verify(x => x.UpdateAsync(wallet, It.IsAny<CancellationToken>()), Times.Once);
            _transactionRepositoryMock.Verify(x => x.UpdateAsync(transaction, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    // helper to set createdAt for testing
    internal static class TransactionExtensions
    {
        public static void SetCreatedAt(this Transaction transaction, DateTime date)
        {
            typeof(Transaction).GetProperty("CreatedAt")!
                .SetValue(transaction, date);
        }
    }
}
