using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Queries.GetUserInfoByPhone
{
    public class GetUserInfoByPhoneQueryValidator : AbstractValidator<GetUserInfoByPhoneQuery>
    {
        public GetUserInfoByPhoneQueryValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required to retrieve user information.");
        }
    }
}
