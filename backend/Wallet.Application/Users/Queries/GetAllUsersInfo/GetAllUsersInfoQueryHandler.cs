using MediatR;
using Wallet.Application.Common.Models;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Wallet.Application.Users.Queries.GetAllUsersInfo
{
    public class GetAllUsersInfoQueryHandler
        : IRequestHandler<GetAllUsersInfoQuery, PagedResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersInfoQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PagedResult<UserDto>> Handle(GetAllUsersInfoQuery request, CancellationToken cancellationToken)
        {
            var query = _userRepository.Query();

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedUsers = await query
                .OrderBy(u => u.FullName)
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .Select(u => new UserDto
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Status = u.UserStatus.ToString(),
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<UserDto>(pagedUsers, totalCount, request.Page, request.Size);
        }
    }
}
