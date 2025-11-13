using System;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Enums;

namespace Wallet.Application.Common.Services
{
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task EnsureUserIsActiveAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            if (user.UserStatus != UserStatus.Active)
                throw new InvalidOperationException("Only active users can perform this operation.");
        }
    }

    public interface IUserValidator
    {
        Task EnsureUserIsActiveAsync(Guid userId, CancellationToken cancellationToken);
    }
}
