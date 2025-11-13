using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.DisableUser
{
    public record DisableUserCommand(Guid UserId) : IRequest<Unit>;
}
