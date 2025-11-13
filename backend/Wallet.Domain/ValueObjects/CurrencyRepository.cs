using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace Wallet.Domain.ValueObjects
{
    public static class CurrencyRepository
    {
        private static readonly Lazy<List<CurrencyData>> _currencies = new Lazy<List<CurrencyData>>(LoadCurrencies);

        public static CurrencyData? GetByCode(string code)
        {
            return _currencies.Value.FirstOrDefault(c => string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase));
        }

        private static List<CurrencyData> LoadCurrencies()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "currencies.json");
    
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[DEBUG] Looking for currencies.json at: {filePath}");

                 throw new FileNotFoundException($"Currencies JSON file not found at {filePath}");
            }

            var json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            //comment for loging the reslt
            Console.WriteLine($"[DEBUG] Looking for currencies file at: {filePath}");
            return JsonSerializer.Deserialize<List<CurrencyData>>(json, options)
                   ?? new List<CurrencyData>();
        }
    }

    public class CurrencyData
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Decimal_Digits { get; set; }
    }
}
