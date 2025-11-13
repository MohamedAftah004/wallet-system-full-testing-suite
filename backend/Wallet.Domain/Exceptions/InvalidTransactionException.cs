using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.Exceptions
{
    public class InvalidTransactionException : Exception
    {
        /// <summary>
        /// when invalid transaction or failed
        /// </summary>
        /// <param name="message"></param>
        public InvalidTransactionException(string message) 
            :base (message)
        {

        }
    }
}
