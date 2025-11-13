namespace Wallet.Application.Wallets.DTOs
{
    public class CreateWalletResponseDto
    {
        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
