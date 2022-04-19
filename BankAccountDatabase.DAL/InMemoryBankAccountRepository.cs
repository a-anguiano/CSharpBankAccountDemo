using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace BankAccountDatabase.DAL
{
    public class InMemoryBankAccountRepository : IBankAccountRepository
    {
        public Dictionary<int, BankAccount> Accounts { get; set; } = new Dictionary<int, BankAccount>();
        private int _nextId = 1;

        public BankAccount Add(BankAccount bankAccount)
        {
            bankAccount.Id = _nextId;
            _nextId++;
            Accounts.Add(bankAccount.Id, bankAccount);
            return bankAccount;
        }

        public BankAccount Delete(int id)
        {
            BankAccount acct = Accounts[id];
            Accounts.Remove(id);
            return acct;
        }

        public BankAccount Get(int id)
        {
            return Accounts[id];
        }

        public List<BankAccount> GetAll()
        {
            return Accounts.Values.ToList();
        }

        public void Update(BankAccount bank)
        {
            Accounts[bank.Id] = bank;
        }
    }
}
