using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;

namespace BankAccountDatabase.DAL
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        public Dictionary<int, Transaction> Transactions { get; set; } = new Dictionary<int, Transaction>();
        private int _nextId = 1;

        public Transaction Add(Transaction transaction)
        {
            transaction.Id = _nextId;
            _nextId++;
            Transactions.Add(transaction.Id, transaction);
            return transaction;
        }

        public Transaction Delete(int id)
        {
            Transaction tran = Transactions[id];
            Transactions.Remove(id);
            return tran;
        }

        public Transaction Get(int id)
        {
            return Transactions[id];
        }

        public List<Transaction> GetAllForAccount(int accountId)
        {
            List<Transaction> result = new List<Transaction>();
            foreach (Transaction t in Transactions.Values)
            {
                if (t.BankAccountId == accountId)
                {
                    result.Add(t);
                }
            }

            return result;
        }

        public List<Transaction> GetAllForAccountWithinDateRange(int accountId, DateTime from, DateTime to)
        {
            List<Transaction> result = new List<Transaction>();

            foreach (Transaction t in GetAllForAccount(accountId))
            {
                if (t.Timestamp > from && t.Timestamp <= to)
                {
                    result.Add(t);
                }
            }

            return result;
        }
    }
}
