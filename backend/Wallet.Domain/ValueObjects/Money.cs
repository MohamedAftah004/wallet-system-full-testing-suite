using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.ValueObjects    
{
    public class Money : IEquatable<Money>
    {
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }


        private Money() { }
        public Money(decimal amount , Currency currency)
        {

            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
            
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            Amount = amount;
            Currency = currency;
        }


        // Factory method
        public static Money Create(decimal amount , Currency currency)
        {
            return new Money(amount, currency);
        }

        //add
        public Money Add(Money other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException("Cannot add money with different currencies");

            return new Money(Amount + other.Amount, Currency);
        }


        //subtruct
        public Money Subtract(Money other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException("Cannot Subtract money with different currencies");

            if (Amount < other.Amount)
                throw new InvalidOperationException("Insufficient funds");

            return new Money(Amount - other.Amount, Currency);
        }



        // Equality logic
        public bool Equals(Money? other)
        {
            if (other is null) return false;
            return Amount == other.Amount && Currency.Equals(other.Currency);
        }
        public override bool Equals(object? obj) => Equals(obj as Money);

        public override int GetHashCode() => HashCode.Combine(Amount, Currency);

        public override string ToString() => $"{Amount} {Currency.Code}";
    }
}
