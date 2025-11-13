using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Wallets.Commands.CloseWallet
{
    public record CloseWalletCommand(Guid WalletId) : IRequest<Unit>;
    
}
