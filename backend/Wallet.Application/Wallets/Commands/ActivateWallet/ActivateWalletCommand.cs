using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Wallets.Commands.ActivateWallet
{
    public record ActivateWalletCommand(Guid WalletId) : IRequest<Unit>;
}
