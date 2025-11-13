using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Users.DTOs;

namespace Wallet.Application.Users.Queries.GetUserInfoByPhone
{
    public record GetUserInfoByPhoneQuery(string Phone) : IRequest<UserDto>;

}
