using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.DTOs;
using Wallet.Domain.Entities;

namespace Wallet.Application.Users.Queries.GetUserInfoByEmail
{
    public class GetUserInfoByEmailQueryHandler : IRequestHandler<GetUserInfoByEmailQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserInfoByEmailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserInfoByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(EntityType.User, request.Email.ToString());

            return new UserDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Status = user.UserStatus.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }

}
