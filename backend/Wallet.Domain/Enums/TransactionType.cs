using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.Enums
{
    public enum TransactionType
    {
        /// <summary>
        /// Adding Funds To wallet.
        /// </summary>
        TopUp = 0,

        /// <summary>
        /// Like Product Payment.
        /// </summary>
        Payment = 1,

        /// <summary>
        /// Like Service Cancellation and retake the your money.
        /// </summary>
        Refund = 2,

        /// <summary>
        /// Transfare from wallet to another wallet.
        /// </summary>  
        Transfer = 3

    }
}
