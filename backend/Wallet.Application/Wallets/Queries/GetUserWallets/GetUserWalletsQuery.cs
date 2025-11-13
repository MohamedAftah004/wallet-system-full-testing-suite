using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Wallets.DTOs;

namespace Wallet.Application.Wallets.Queries.GetUserWallets
{
   
    public record GetUserWalletsQuery(Guid UserId) : IRequest<IEnumerable<WalletDto>>;
}
