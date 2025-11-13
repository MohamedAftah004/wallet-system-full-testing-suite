using Wallet.Domain.Common;
using Wallet.Domain.Entities;
using WalletEntity = Wallet.Domain.Entities.Wallet;

namespace Wallet.Domain.Events
{

    /// <summary>
    /// Raised when a new wallet is created for a user
    /// </summary>
    public class WalletCreatedEvent : IDomainEvent
    {

        public WalletEntity CreatedWallet { get; }

        public WalletCreatedEvent(WalletEntity wallet)
        {
            CreatedWallet = wallet;
        }
    }
}
