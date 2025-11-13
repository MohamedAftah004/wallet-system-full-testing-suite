using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Queries.GetUserInfoByEmail
{
    public class GetUserInfoByEmailQueryValidator : AbstractValidator<GetUserInfoByEmailQuery>
    {
        public GetUserInfoByEmailQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required to retrieve user information.");
        }
    }
}
