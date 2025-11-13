using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.FreezeUser
{
    public record FreezeUserCommand(Guid UserId) :IRequest<Unit>;

}
