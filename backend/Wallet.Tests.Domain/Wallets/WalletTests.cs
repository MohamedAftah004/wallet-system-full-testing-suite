using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;
using CurrentWallet = Wallet.Domain.Entities.Wallet;

namespace Wallet.Tests.Domain.Wallets
{
    public class WalletTests
    {

        #region Constructor
        [Fact]
        public void Constructor_ShouldInitializeWallet_WithZeroBalance_AndPendingStatus()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currency = Currency.FromCode("USD");

            //Act
            var wallet = new CurrentWallet(userId, currency);

            //Assert
            wallet.UserId.Should().Be(userId);
            wallet.Balance.Amount.Should().Be(0);
            wallet.Balance.Currency.Should().Be(Currency.FromCode("USD"));
            wallet.Status.Should().Be(WalletStatus.PendingActivation);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenUserIdIsEmpty()
        {
            //Arrange
            var currency = Currency.FromCode("USD");

            //Act
            var act = () => new CurrentWallet(Guid.Empty, currency);

            //Assert
            act.Should().Throw<ArgumentException>().WithMessage("*UserId*");

        }
        #endregion


        #region TopUp
        [Fact]
        public void TopUp_ShouldIncreaseBalance_WhenAmountIsPositive()
        {
            //Arrange
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("USD"));
            var initialBalance = wallet.Balance.Amount;

            //Act 
            wallet.TopUp(100);

            //Assert
            wallet.Balance.Amount.Should().Be(initialBalance + 100);

        }

        [Fact]
        public void TopUp_ShouldThrow_WhenAmountIsZeroOrNegative()
        {
            //Arrange
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("USD"));

            //Act
            var act = () => wallet.TopUp(0);

            //Assert
            act.Should().Throw<ArgumentException>().WithMessage("*greater than zero*");

        }
        #endregion


        #region Deduct
        [Fact]
        public void Deduct_ShouldIncreaseBalance_WhenAmountIsPositive()
        {
            //Arrange
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.TopUp(200);

            //Act
            wallet.Deduct(50);

            //Assert
            wallet.Balance.Amount.Should().Be(150);
        }


        [Fact]
        public void Deduct_ShouldThrow_WhenAmountIsZeroOrNegative()
        {

            //Arrange
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("USD"));

            //Act
            var act = () => wallet.Deduct(-10);

            //Assert
            act.Should().Throw<ArgumentException>().WithMessage("*greater than zero*");
        }
        #endregion


        #region Status

        #region Activate
        [Fact]
        public void Activate_ShouldChangeStatusToActive()
        {
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.Activate();
            wallet.Status.Should().Be(WalletStatus.Active);
        }
        #endregion

        #region Freeze
        [Fact]
        public void Freeze_ShouldChangeStatusToFreeze()
        {
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.Freeze();
            wallet.Status.Should().Be(WalletStatus.Frozen);
        }
        #endregion

        #region Disable
        [Fact]
        public void Disable_ShouldChangeStatusToDisabled()
        {
            var wallet = new CurrentWallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.Disable();
            wallet.Status.Should().Be(WalletStatus.Disabled);
        }
        #endregion

        #endregion

    }
}
