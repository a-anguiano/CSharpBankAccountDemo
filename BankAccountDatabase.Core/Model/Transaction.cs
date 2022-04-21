using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccountDatabase.Core.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        
        [Column("TransactionType")]
        public TransactionType Type { get; set; }
        
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }

        [Column("AccountId")]
        public int BankAccountId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Transaction transaction &&
                   Id == transaction.Id &&
                   Type == transaction.Type &&
                   Timestamp == transaction.Timestamp &&
                   Amount == transaction.Amount &&
                   Note == transaction.Note &&
                   BankAccountId == transaction.BankAccountId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type, Timestamp, Amount, Note, BankAccountId);
        }
    }
}
