using BankAccountDatabase.BLL;
using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.DAL;
using System;

namespace BankAccountDatabase.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleIO io = new ConsoleIO();
            IBankAccountRepository bankRepo = new InMemoryBankAccountRepository();
            ITransactionRepository transactionRepo = new InMemoryTransactionRepository();
            IBankAccountService bankAccounts = new BankAccountService(bankRepo);
            ITransactionService transactions = new TransactionService(transactionRepo, bankAccounts);
            Controller controller = new Controller(io, bankAccounts, transactions);
            controller.Run();

        }
    }
}
