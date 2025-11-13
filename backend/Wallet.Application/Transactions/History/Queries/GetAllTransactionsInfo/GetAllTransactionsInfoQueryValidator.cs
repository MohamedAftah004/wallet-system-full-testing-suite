using FluentValidation;

namespace Wallet.Application.Transactions.History.Queries.GetAllTransactionsInfo
{
    public class GetAllTransactionsInfoQueryValidator : AbstractValidator<GetAllTransactionsInfoQuery>
    {
        public GetAllTransactionsInfoQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0.");

            RuleFor(x => x.Size)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("ToDate must be greater than or equal to FromDate.");
        }
    }
}
