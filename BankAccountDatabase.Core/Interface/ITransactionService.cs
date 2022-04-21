using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;

namespace BankAccountDatabase.Core.Interface
{
    public interface ITransactionService
    {
        public Result<Transaction> Get(int id);
        public Result<List<Transaction>> GetAllForAccount(int accountId);
        public Result<List<Transaction>> GetAllForAccountAndDateRange(int accountId, DateTime from, DateTime to);
        public Result<Transaction> Add(Transaction transaction);
    }
}
