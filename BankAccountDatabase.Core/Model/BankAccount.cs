using System;
using System.Collections.Generic;

namespace BankAccountDatabase.Core.Model
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountHolder { get; set; }
        public decimal CurrentBalance { get; set; }

        public List<Transaction> Transactions { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BankAccount account &&
                   Id == account.Id &&
                   AccountHolder == account.AccountHolder &&
                   CurrentBalance == account.CurrentBalance &&
                   EqualityComparer<List<Transaction>>.Default.Equals(Transactions, account.Transactions);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, AccountHolder, CurrentBalance, Transactions);
        }
    }
}
