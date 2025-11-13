using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(string FullName , string Email , string PhoneNumber , string Password) : IRequest<Guid>;
    
}
