using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;

namespace BankAccountDatabase.Core.Interface
{
    public interface ITransactionRepository
    {
        public Transaction Get(int id);
        public List<Transaction> GetAllForAccount(int accountId);
        public List<Transaction> GetAllForAccountWithinDateRange(int accountId, DateTime from, DateTime to);
        public Transaction AddTransactionToAccount(BankAccount account, Transaction transaction);
    }
}
