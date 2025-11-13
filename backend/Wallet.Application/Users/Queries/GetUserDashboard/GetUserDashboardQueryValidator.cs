using FluentValidation;

namespace Wallet.Application.Users.Queries.GetUserDashboard
{
    public class GetUserDashboardQueryValidator : AbstractValidator<GetUserDashboardQuery>
    {
        public GetUserDashboardQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required to fetch dashboard data.");
        }
    }
}
