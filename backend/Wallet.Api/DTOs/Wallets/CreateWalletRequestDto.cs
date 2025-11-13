using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;

namespace Wallet.Api.DTOs.Wallets
{
    public class CreateWalletRequestDto
    {
        public Guid UserId { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
