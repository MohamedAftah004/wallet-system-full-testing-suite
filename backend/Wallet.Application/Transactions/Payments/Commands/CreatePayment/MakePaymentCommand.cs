using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Transactions.Payments.DTOs;

namespace Wallet.Application.Transactions.Payments.Commands.MakePayment
{
    public record MakePaymentCommand(Guid WalletId , decimal Amount , string Description) : IRequest<PaymentResponseDto>;
   
}
