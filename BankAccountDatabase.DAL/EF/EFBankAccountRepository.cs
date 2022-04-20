using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccountDatabase.DAL.EF
{
    public class EFBankAccountRepository : IBankAccountRepository
    {
        public DBFactory DbFac { get; set; }

        public EFBankAccountRepository(DBFactory dbfac)
        {
            DbFac = dbfac;
        }

        public BankAccount Add(BankAccount bankAccount)
        {
            using (var db = DbFac.GetDbContext())
            {
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
            }

            return bankAccount;
        }

        public BankAccount Delete(int id)
        {
            using (var db = DbFac.GetDbContext())
            {
                BankAccount acct = db.BankAccounts.Find(id);
                db.BankAccounts.Remove(acct);
                db.SaveChanges();
                return acct;
            }
        }

        public BankAccount Get(int id)
        {
            using (var db = DbFac.GetDbContext())
            {
                return db.BankAccounts.Find(id);
            }
        }

        public List<BankAccount> GetAll()
        {
            using (var db = DbFac.GetDbContext())
            {
                return db.BankAccounts.ToList();
            }
        }

        public void Update(BankAccount bank)
        {
            using (var db = DbFac.GetDbContext())
            {
                db.BankAccounts.Update(bank);
                db.SaveChanges();
            }
        }
    }
}
