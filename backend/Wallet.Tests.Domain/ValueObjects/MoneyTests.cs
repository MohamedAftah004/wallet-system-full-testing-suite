using FluentAssertions;
using System;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Domain.ValueObjects
{
    public class MoneyTests
    {
        private static Currency USD => Currency.FromCode("USD");

        #region Constructor

        [Fact]
        public void Constructor_ShouldInitializeCorrectly_WhenValidData()
        {
            var money = new Money(100, USD);

            money.Amount.Should().Be(100);
            money.Currency.Should().Be(USD);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenAmountIsNegative()
        {
            var act = () => new Money(-10, USD);

            act.Should().Throw<ArgumentException>().WithMessage("*Amount cannot be negative*");
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenCurrencyIsNull()
        {
            var act = () => new Money(10, null);

            act.Should().Throw<ArgumentNullException>().WithMessage("*currency*");
        }

        #endregion

        #region Add / Subtract

        [Fact]
        public void Add_ShouldReturnNewMoney_WithSumOfAmounts()
        {
            var money1 = Money.Create(100, Currency.FromCode("USD"));
            var money2 = Money.Create(50, Currency.FromCode("USD"));

            var result = money1.Add(money2);

            result.Amount.Should().Be(150);
        }


      

        [Fact]
        public void Subtract_ShouldReturnNewMoney_WithDifference()
        {
            var money1 = Money.Create(100, Currency.FromCode("USD"));
            var money2 = Money.Create(50, Currency.FromCode("USD"));

            var result = money1.Subtract(money2);

            result.Amount.Should().Be(50);
        }

        [Fact]
        public void Subtract_ShouldThrow_WhenInsufficientFunds()
        {
            var money1 = Money.Create(50, Currency.FromCode("USD"));
            var money2 = Money.Create(100, Currency.FromCode("USD"));

            var act = () => money1.Subtract(money2);

            act.Should().Throw<InvalidOperationException>().WithMessage("*Insufficient funds*");
        }


        #endregion

        #region Equality

        [Fact]
        public void Equals_ShouldReturnTrue_WhenAmountAndCurrencyAreSame()
        {
            var money1 = Money.Create(100, Currency.FromCode("USD"));
            var money2 = Money.Create(100, Currency.FromCode("USD"));

            money1.Equals(money2).Should().BeTrue();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenAmountDiffers()
        {
            var money1 = Money.Create(100, Currency.FromCode("USD"));
            var money2 = Money.Create(50, Currency.FromCode("USD"));

            money1.Equals(money2).Should().BeFalse();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenCurrencyDiffers()
        {
            var money1 = Money.Create(100, Currency.FromCode("USD"));
            var money2 = Money.Create(50, Currency.FromCode("USD"));

            money1.Equals(money2).Should().BeFalse();
        }

        #endregion
    }
}
