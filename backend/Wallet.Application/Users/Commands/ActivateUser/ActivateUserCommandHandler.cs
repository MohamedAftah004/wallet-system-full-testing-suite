using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Enums;

namespace Wallet.Application.Users.Commands.ActivateUser
{
    public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, Unit>
    {

        private readonly IUserRepository _userRepository;
        public ActivateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        public async Task<Unit> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId , cancellationToken);

            if (user == null)
                //shoud creating not found exception class on Application/Common/Exceptions
                throw new EntityNotFoundException(EntityType.User , request.UserId.ToString());

            if (user.UserStatus == UserStatus.Active)
                throw new InvalidOperationException($"User with ID {request.UserId} is already active.");

            await _userRepository.ActivateAsync(request.UserId , cancellationToken);
            return Unit.Value;
        }
    }
}
