using FluentAssertions;
using System;
using Wallet.Domain.Entities;
using Xunit;

namespace Wallet.Tests.Domain.PaymentGatewayLogs
{
    public class PaymentGatewayLogTests
    {
        #region Constructor

        [Fact]
        public void Constructor_ShouldInitializePaymentGatewayLog_WithValidData()
        {
            // Arrange
            var transactionId = Guid.NewGuid();

            // Act
            var log = new PaymentGatewayLog(
                transactionId,
                "Stripe",
                "{request}",
                "{response}",
                "200"
            );

            // Assert
            log.TransactionId.Should().Be(transactionId);
            log.GatewayName.Should().Be("Stripe");
            log.RequestPayload.Should().Be("{request}");
            log.ResponsePayload.Should().Be("{response}");
            log.StatusCode.Should().Be("200");
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenTransactionIdIsEmpty()
        {
            // Act
            var act = () => new PaymentGatewayLog(
                Guid.Empty,
                "Stripe",
                "{request}",
                "{response}",
                "200"
            );

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*TransactionId*");
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenGatewayNameIsNull()
        {
            // Act
            var act = () => new PaymentGatewayLog(
                Guid.NewGuid(),
                null,
                "{request}",
                "{response}",
                "200"
            );

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*gatewayName*");
        }

        [Fact]
        public void Constructor_ShouldSetDefaultValues_WhenPayloadsOrStatusCodeAreNull()
        {
            // Arrange
            var transactionId = Guid.NewGuid();

            // Act
            var log = new PaymentGatewayLog(transactionId, "Stripe", null, null, null);

            // Assert
            log.RequestPayload.Should().Be("{}");
            log.ResponsePayload.Should().Be("{}");
            log.StatusCode.Should().Be("Unknown");
        }

        #endregion
    }
}
