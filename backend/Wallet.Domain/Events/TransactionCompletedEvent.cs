using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Wallet.Domain.Common;

namespace Wallet.Domain.Events
{
    /// <summary>
    /// Raised when transaction completed successfully
    /// </summary>
    public class TransactionCompletedEvent : IDomainEvent
    {

        public Transaction Transaction { get; set; }

        public TransactionCompletedEvent(Transaction transaction)
        {
            Transaction = transaction;
        }

    }
}
