using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.CloseUser
{
    public record CloseUserCommand(Guid UserId) : IRequest<Unit>;
    
}
