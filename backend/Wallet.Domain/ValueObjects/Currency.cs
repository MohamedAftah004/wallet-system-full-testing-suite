using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.ValueObjects
{
    public class Currency : IEquatable<Currency>
    {

        public string Code { get; }
        public string Symbol { get; }

        private Currency(string code, string symbol)
        { 
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Currency code is required", nameof(code));

            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Currency symbol is required", nameof(symbol));

            Code = code.ToUpperInvariant();
            Symbol = symbol;
        }


        // EF Core needs this
        private Currency() { }

        public static Currency FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Currency code is required", nameof(code));
                
            code = code.ToUpperInvariant();

            var currencyData = CurrencyRepository.GetByCode(code);

            if (currencyData is null)
                throw new ArgumentException($"Unsupported currency code: {code}", nameof(code));

            return new Currency(currencyData.Code, currencyData.Symbol);
        }


        public override bool Equals(object obj) => Equals(obj as Currency);
        public bool Equals(Currency other) =>
            other != null && Code == other.Code;

        public override int GetHashCode() => Code.GetHashCode();

        public override string ToString() => $"{Code} ({Symbol})";
    }
}
