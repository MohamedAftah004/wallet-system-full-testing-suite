using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.Exceptions
{
    public class InsufficientBalanceException : Exception
    {

        /// <summary>
        /// like -> 200$ - 1900$
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="attemptedAmount"></param>
        public InsufficientBalanceException(decimal balance , decimal attemptedAmount)
            :base($"Insufficient balance. Current balance: {balance}, attempted: {attemptedAmount}")
        {
        }
    }
}
