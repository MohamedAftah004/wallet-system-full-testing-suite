using FluentAssertions;
using System;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Domain.ValueObjects
{
    public class CurrencyTests
    {
        #region FromCode

        [Fact]
        public void FromCode_ShouldReturnCurrency_WhenCodeIsValid()
        {
            // Arrange
            var result = Currency.FromCode("usd");

            // Assert
            result.Code.Should().Be("USD");
            result.Symbol.Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void FromCode_ShouldThrow_WhenCodeIsInvalid(string invalidCode)
        {
            // Act
            var act = () => Currency.FromCode(invalidCode);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*Currency code is required*");
        }

        [Fact]
        public void FromCode_ShouldThrow_WhenCurrencyNotSupported()
        {
            // Act
            var act = () => Currency.FromCode("ABC");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*Unsupported currency code*");
        }

        #endregion

        #region Equality

        [Fact]
        public void Equals_ShouldReturnTrue_WhenCodesAreSame()
        {
            var c1 = Currency.FromCode("USD");
            var c2 = Currency.FromCode("usd");

            c1.Equals(c2).Should().BeTrue();
            c1.GetHashCode().Should().Be(c2.GetHashCode());
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenCodesAreDifferent()
        {
            var usd = Currency.FromCode("USD");
            var eur = Currency.FromCode("EUR");

            usd.Equals(eur).Should().BeFalse();
        }

        #endregion
    }
}
