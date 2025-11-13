using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Security.DTOs;

namespace Wallet.Application.Security.Commands.Login
{
    public record LoginCommand(string Email , string Password) : IRequest<AuthResultDto>;

}
