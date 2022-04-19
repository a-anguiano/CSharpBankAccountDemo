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
            string connectionString = "Server=localhost;Database=BankOfKlueberia;User Id=sa;Password=ZvBxRp7ss!;"; 



            ConsoleIO io = new ConsoleIO();
            IBankAccountRepository bankRepo = new SqlBankAccountRepository(connectionString);
            ITransactionRepository transactionRepo = new SqlTransactionRepository(connectionString);
            IBankAccountService bankAccounts = new BankAccountService(bankRepo);
            ITransactionService transactions = new TransactionService(transactionRepo, bankAccounts);
            Controller controller = new Controller(io, bankAccounts, transactions);
            controller.Run();

        }
    }
}
