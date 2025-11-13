using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Common;
using Wallet.Domain.Enums;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Entities
{
    public class Transaction : BaseEntity
    {

        public Guid WalletId { get; private set; }
        public Money Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public string? ReferenceId { get; private set; }
        public string? Description { get; private set; }
        public TransactionStatus Status { get; private set; }


        //Navigation Proplrty
        public Wallet Wallet { get; private set; }

        //for Ef    
        public Transaction()
        {
        }


        public Transaction(Guid walletId, Money amount, TransactionType type, string? referenceId = null, string? description = null)
        {

            if (walletId == Guid.Empty)
                throw new ArgumentException("Wallet id is required", nameof(walletId));

            if (amount.Amount < 0)
                throw new ArgumentException("Transaction amount must be greater than zero", nameof(amount));

            WalletId = walletId;
            Amount = amount;
            Type = type;
            ReferenceId = referenceId;
            Description = description;
            Status = TransactionStatus.Pending;
        }


        // ----------------------------
        // Domain Methods (behavior) --> what entiity can do 
        // ---------------------------- 


        ///<summary>
        ///Mark transaction as successfulley completed
        /// </summary>
        public void MarkCompleted()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidTransactionException("Only pending transactions can be completed.");

            Status = TransactionStatus.Completed;
            SetUpdatedAt();
        }


        /// <summary>
        /// Mark transaction as failed with a reason.
        /// </summary> 
        public void MarkFailed(string reason)
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidTransactionException("Only pending transactions can be failed.");

            Status = TransactionStatus.Failed;
            Description = $"{Description} | Failed: {reason}";
            SetUpdatedAt();
        }


        /// <summary>
        /// Reverse a completed transaction (e.g. refund).
        /// </summary>
        public void MarkReversed(string reason)
        {
            if (Status != TransactionStatus.Completed)
                throw new InvalidTransactionException("Only completed transactions can be reversed.");

            Status = TransactionStatus.Reversed;
            Description = $"{Description} | Reversed: {reason}";
            SetUpdatedAt();
        }




    }
}
