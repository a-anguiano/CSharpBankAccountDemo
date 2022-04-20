using BankAccountDatabase.BLL;
using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.DAL;
using BankAccountDatabase.DAL.EF;
using System;

namespace BankAccountDatabase.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigProvider cp = new ConfigProvider();
            DBFactory dbFac = new DBFactory(cp.Config);

            ConsoleIO io = new ConsoleIO();
            IBankAccountRepository bankRepo = new EFBankAccountRepository(dbFac);
            ITransactionRepository transactionRepo = new EFTransactionRepository(dbFac);
            IBankAccountService bankAccounts = new BankAccountService(bankRepo);
            ITransactionService transactions = new TransactionService(transactionRepo, bankAccounts);
            Controller controller = new Controller(io, bankAccounts, transactions);
            controller.Run();

        }
    }
}
