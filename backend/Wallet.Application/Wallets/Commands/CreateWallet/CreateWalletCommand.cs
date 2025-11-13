using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Wallets.DTOs;

namespace Wallet.Application.Wallets.Commands.CreateWallet
{
    public record CreateWalletCommand(Guid UserId , string CurrencyCode) : IRequest<CreateWalletResponseDto>;
    
}
