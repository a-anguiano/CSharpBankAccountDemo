using BankAccountDatabase.Core.Model;
using System.Collections.Generic;

namespace BankAccountDatabase.Core.Interface
{
    public interface IBankAccountRepository
    {
        public BankAccount Get(int id);
        public List<BankAccount> GetAll();
        public BankAccount Add(BankAccount bankAccount);
        public void Update(BankAccount bank);
        public BankAccount Delete(int id);
    }
}
