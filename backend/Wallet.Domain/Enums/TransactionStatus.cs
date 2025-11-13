using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.Enums
{
    public enum TransactionStatus
    {
        /// <summary>
        /// Transaction Created but not complete yeet.
        /// </summary>
        Pending = 0 ,

        /// <summary>
        /// Transaction Completed.
        /// </summary>
        Completed = 1 ,

        /// <summary>
        /// Transaction Failed.
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Transaction was canceled or reversed.
        /// </summary>
        Reversed = 3
    }
}
