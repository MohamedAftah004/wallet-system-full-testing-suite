using FluentValidation;

namespace Wallet.Application.Users.Queries.GetAllUsersInfo
{
    public class GetAllUsersInfoQueryValidator : AbstractValidator<GetAllUsersInfoQuery>
    {
        public GetAllUsersInfoQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Size)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}
