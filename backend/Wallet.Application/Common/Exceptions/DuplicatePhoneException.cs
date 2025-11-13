using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Common.Exceptions
{
    public class DuplicatePhoneException : Exception
    {

        public DuplicatePhoneException(string phone)
        : base($"A user with phone: `{phone}` already exists.")
        {
        }
    }
}
