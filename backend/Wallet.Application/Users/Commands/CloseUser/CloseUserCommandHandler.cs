using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.Commands.ActivateUser;
using Wallet.Domain.Enums;

namespace Wallet.Application.Users.Commands.CloseUser
{
    public class CloseUserCommandHandler : IRequestHandler<CloseUserCommand , Unit>
    {


        private readonly IUserRepository _userRepository;
        public CloseUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<Unit> Handle(CloseUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                //shoud creating not found exception class on Application/Common/Exceptions
                throw new EntityNotFoundException(EntityType.User, request.UserId.ToString());

            if (user.UserStatus == UserStatus.PendingActivation)
                throw new InvalidOperationException($"User with ID {request.UserId} is not active, so you cannot close it.");

            if (user.UserStatus == UserStatus.Closed)
                throw new InvalidOperationException($"User with ID {request.UserId} is already Closed.");

            await _userRepository.CloseAsync(request.UserId, cancellationToken);

            return Unit.Value;
        }
    }
}
