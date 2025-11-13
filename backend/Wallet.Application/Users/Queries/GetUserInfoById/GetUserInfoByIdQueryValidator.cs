using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Queries.GetUserInfoById
{
    public class GetUserInfoByIdQueryValidator : AbstractValidator<GetUserInfoByIdQuery>
    {
        public GetUserInfoByIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required to retrieve user information.");
        }
    }
}
