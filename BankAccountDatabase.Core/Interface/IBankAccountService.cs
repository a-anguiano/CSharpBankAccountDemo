using BankAccountDatabase.Core.Model;
using System.Collections.Generic;

namespace BankAccountDatabase.Core.Interface
{
    public interface IBankAccountService
    {
        public Result<BankAccount> Get(int id);
        public Result<List<BankAccount>> GetAll();
        public Result<BankAccount> Save(BankAccount bankAccount);
        public Result<BankAccount> Delete(int id);

    }
}
