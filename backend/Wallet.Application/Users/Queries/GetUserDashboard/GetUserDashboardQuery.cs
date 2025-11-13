using MediatR;
using Wallet.Application.Users.DTOs;

namespace Wallet.Application.Users.Queries.GetUserDashboard
{
    public record GetUserDashboardQuery(Guid UserId) : IRequest<UserDashboardDto>;
}
