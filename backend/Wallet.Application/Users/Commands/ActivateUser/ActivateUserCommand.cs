using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.ActivateUser
{
    public record ActivateUserCommand(Guid UserId) : IRequest<Unit>;
    
}
