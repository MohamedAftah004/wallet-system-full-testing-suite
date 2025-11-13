using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;

namespace Wallet.Application.Users.Commands.FreezeUser
{
    public class FreezeUserCommandHandler : IRequestHandler<FreezeUserCommand, Unit>
    {

        private readonly IUserRepository _userRepository;

        public FreezeUserCommandHandler(IUserRepository userRespository)
        {
            _userRepository = userRespository;            
        }

        public async Task<Unit> Handle(FreezeUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId , cancellationToken);

            if(user == null)
                throw new EntityNotFoundException(EntityType.User, request.UserId.ToString());

            if(user.UserStatus != Domain.Enums.UserStatus.Active)
                throw new InvalidOperationException($"User with ID {request.UserId} should be activate to freeze it.");

            if(user.UserStatus == Domain.Enums.UserStatus.Frozen)
                throw new InvalidOperationException($"User with ID {request.UserId} is already frozen.");

            await _userRepository.FreezeAsync(request.UserId, cancellationToken);
            return Unit.Value;
        }
    }
}
