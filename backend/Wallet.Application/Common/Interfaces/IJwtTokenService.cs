using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Entities;

namespace Wallet.Application.Common.Interfaces
{
    public interface IJwtTokenService 
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
