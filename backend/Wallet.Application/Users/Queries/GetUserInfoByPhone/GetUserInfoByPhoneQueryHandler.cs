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

namespace Wallet.Application.Users.Queries.GetUserInfoByPhone
{
    public class GetUserInfoByEmailQueryHandler : IRequestHandler<GetUserInfoByPhoneQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserInfoByEmailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserInfoByPhoneQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByPhoneAsync(request.Phone, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(EntityType.User, request.Phone.ToString());

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
