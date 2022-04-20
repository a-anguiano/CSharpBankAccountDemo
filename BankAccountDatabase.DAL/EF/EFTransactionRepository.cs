using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccountDatabase.DAL.EF
{
    public class EFTransactionRepository : ITransactionRepository
    {
        private DBFactory DbFac;

        public EFTransactionRepository(DBFactory dbFac)
        {
            DbFac = dbFac;
        }

        public Transaction Add(Transaction transaction)
        {
            using (var db = DbFac.GetDbContext())
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return transaction;
            }
        }

        public Transaction Delete(int id)
        {
            using (var db = DbFac.GetDbContext())
            {
                Transaction transaction = db.Transactions.Find(id);
                db.Remove(transaction);
                db.SaveChanges();
                return transaction;
            }
        }

        public Transaction Get(int id)
        {
            using (var db = DbFac.GetDbContext())
            {
                return db.Transactions.Find(id);
            }
        }

        public List<Transaction> GetAllForAccount(int accountId)
        {
            using (var db = DbFac.GetDbContext())
            {
                List<Transaction> results = db.Transactions
                    .Where(t =>  t.BankAccountId == accountId ).ToList();

                return results;
            }
        }

        public List<Transaction> GetAllForAccountWithinDateRange(int accountId, DateTime from, DateTime to)
        {
            using (var db = DbFac.GetDbContext())
            {
                List<Transaction> results = db.Transactions
                    .Where(t => t.BankAccountId == accountId
                             && t.Timestamp > from
                             && t.Timestamp <= to
                    ).ToList();

                return results;
            }
        }
    }
}
