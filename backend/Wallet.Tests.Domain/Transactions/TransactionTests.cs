using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Domain.Transactions
{
    public class TransactionTests
    {

        #region Constructor
        [Fact]
        public void Constructor_ShouldInitializeTransaction_WithPendingStatus() 
        { 
            //Arrange
            var walletId = Guid.NewGuid();
            var amount = Money.Create(100, Currency.FromCode("USD"));

            //Act
            var transaction = new Transaction(walletId , amount, TransactionType.TopUp, "REF123", "Test transaction");

            //Assert
            transaction.WalletId.Should().Be(walletId);
            transaction.Amount.Amount.Should().Be(100);
            transaction.Amount.Currency.Code.Should().Be("USD");
            transaction.Type.Should().Be(TransactionType.TopUp);
            transaction.Status.Should().Be(TransactionStatus.Pending);
            transaction.ReferenceId.Should().Be("REF123");
            transaction.Description.Should().Be("Test transaction");
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenWalletIdIsEmpty()
        {
            // Arrange
            var amount = Money.Create(50, Currency.FromCode("USD"));

            // Act
            var act = () => new Transaction(Guid.Empty, amount, TransactionType.TopUp);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*Wallet id*");
        }


        //[Fact]
        //public void Constructor_ShouldThrow_WhenAmountIsNegative()
        //{
        //    // Arrange
        //    var walletId = Guid.NewGuid();
        //    var amount = Money.Create(-50, Currency.FromCode("USD"));

        //    // Act
        //    var act = () => new Transaction(walletId, amount, TransactionType.TopUp);

        //    // Assert
        //    act.Should().Throw<ArgumentException>().WithMessage("*greater than zero*");
        //}
        #endregion

        #region MarkCompleted
        [Fact]
        public void MarkCompleted_ShouldChangeStatusToCompleted_WhenPending()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(100, Currency.FromCode("USD")), TransactionType.TopUp);

            // Act
            transaction.MarkCompleted();

            // Assert
            transaction.Status.Should().Be(TransactionStatus.Completed);

        }


        [Fact]
        public void MarkCompleted_ShouldThrow_WhenNotPending()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(100, Currency.FromCode("USD")), TransactionType.TopUp);
            transaction.MarkCompleted(); // already completed

            // Act
            var act = () => transaction.MarkCompleted();

            // Assert
            act.Should().Throw<InvalidTransactionException>().WithMessage("*pending*");
        }
        #endregion

        #region MarkFailed
        [Fact]
        public void MarkFailed_ShouldChangeStatusToFailed_WhenPending()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(100, Currency.FromCode("USD")), TransactionType.TopUp, description: "Initial");

            // Act
            transaction.MarkFailed("Network Error");

            // Assert
            transaction.Status.Should().Be(TransactionStatus.Failed);
            transaction.Description.Should().Contain("Failed: Network Error");
        }

        [Fact]
        public void MarkFailed_ShouldThrow_WhenNotPending()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(100, Currency.FromCode("USD")), TransactionType.TopUp);
            transaction.MarkCompleted();

            // Act
            var act = () => transaction.MarkFailed("Network Error");

            // Assert
            act.Should().Throw<InvalidTransactionException>().WithMessage("*pending*");
        }
        #endregion

        #region MarkReversed
        [Fact]
        public void MarkReversed_ShouldChangeStatusToReversed_WhenCompleted()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(200, Currency.FromCode("USD")), TransactionType.Payment, description: "Order #123");
            transaction.MarkCompleted();

            // Act
            transaction.MarkReversed("Refund requested");

            // Assert
            transaction.Status.Should().Be(TransactionStatus.Reversed);
            transaction.Description.Should().Contain("Reversed: Refund requested");
        }

        [Fact]
        public void MarkReversed_ShouldThrow_WhenNotCompleted()
        {
            // Arrange
            var transaction = new Transaction(Guid.NewGuid(), Money.Create(200, Currency.FromCode("USD")), TransactionType.Payment);

            // Act
            var act = () => transaction.MarkReversed("Refund requested");

            // Assert
            act.Should().Throw<InvalidTransactionException>().WithMessage("*completed*");
        }
        #endregion

    }
}
