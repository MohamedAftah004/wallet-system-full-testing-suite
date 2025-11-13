using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Models;
using Wallet.Application.Users.DTOs;

namespace Wallet.Application.Users.Queries.GetAllUsersInfo
{
    public record GetAllUsersInfoQuery(int Page = 1, int Size = 10)
        : IRequest<PagedResult<UserDto>>;
}
