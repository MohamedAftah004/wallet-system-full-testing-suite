using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Enums;

namespace Wallet.Application.Users.Commands.DisableUser
{
    public class DisableUserCommandHandler : IRequestHandler<DisableUserCommand, Unit>
    {


        private readonly IUserRepository _userRepository;
        public DisableUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<Unit> Handle(DisableUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                //shoud creating not found exception class on Application/Common/Exceptions
                throw new EntityNotFoundException(EntityType.User, request.UserId.ToString());

            if (user.UserStatus != UserStatus.Active)
                throw new InvalidOperationException($"User with ID {request.UserId} is already Not-Active.");

            await _userRepository.DisableAsync(request.UserId, cancellationToken);
            return Unit.Value;

        }
    }
}
