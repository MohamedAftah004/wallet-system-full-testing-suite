using System;
using System.Collections.Generic;
using Wallet.Domain.Common;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Money Balance { get; private set; }
        public WalletStatus Status { get; private set; }

        // Navigation properties
        public User User { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        // EF Core parameterless constructor
        private Wallet() { }

        // Constructor with required fields
        public Wallet(Guid userId, Currency currency, WalletStatus status = WalletStatus.PendingActivation)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required", nameof(userId));

            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            UserId = userId;
            Balance = Money.Create(0, currency);
            Status = status;
        }

        /// <summary>
        /// Add funds to the wallet.
        /// </summary>
        public void TopUp(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Top-up amount must be greater than zero", nameof(amount));

            var moneyToAdd = Money.Create(amount, Balance.Currency);
            Balance = Balance.Add(moneyToAdd);
            SetUpdatedAt();
        }

        /// <summary>
        /// Deduct funds from the wallet.
        /// </summary>
        public void Deduct(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deduct amount must be greater than zero", nameof(amount));

            var moneyToDeduct = Money.Create(amount, Balance.Currency);
            Balance = Balance.Subtract(moneyToDeduct);
            SetUpdatedAt();
        }

        /// <summary>
        /// Activate the wallet (allow transactions).
        /// </summary>
        public void Activate()
        {
            Status = WalletStatus.Active;
            SetUpdatedAt();
        }

        /// <summary>
        /// Disable the wallet (close permanently).
        /// </summary>
        public void Disable()
        {
            Status = WalletStatus.Disabled;
            SetUpdatedAt();
        }

        /// <summary>
        /// Freeze the wallet (temporarily suspend transactions).
        /// </summary>
        public void Freeze()
        {
            Status = WalletStatus.Frozen;
            SetUpdatedAt();
        }
    }
}
