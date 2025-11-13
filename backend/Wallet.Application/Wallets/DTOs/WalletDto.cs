using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Wallets.DTOs
{
    public class WalletDto
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public BalanceDto Balance { get; set; } = new();
        public StatusDto Status { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
