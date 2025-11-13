using FluentValidation;

namespace Wallet.Application.Transactions.History.Queries.GetRecentTransactions
{
    public class GetRecentTransactionsQueryValidator : AbstractValidator<GetRecentTransactionsQuery>
    {
        public GetRecentTransactionsQueryValidator()
        {
            RuleFor(x => x.Count)
                .InclusiveBetween(1, 100)
                .WithMessage("Count must be between 1 and 100.");
        }
    }
}
