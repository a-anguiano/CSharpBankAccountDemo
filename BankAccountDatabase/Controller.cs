using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;


namespace BankAccountDatabase.UI
{
    class Controller
    {
        public ConsoleIO IO { get; set; }
        public BankAccount CurrentAccount { get; set; }
        public IBankAccountService BankAccounts { get; set; }
        public ITransactionService Transactions { get; set; }

        public Controller(ConsoleIO io, IBankAccountService bankAccounts, ITransactionService transactions)
        {
            IO = io;
            BankAccounts = bankAccounts;
            Transactions = transactions;
        }

        public void Run()
        {
            DisplayTitle();
            bool running = true;

            while(running)
            {
                switch(DisplayMenu().ToString().ToUpper())
                {
                    case "L":
                        IO.Display("");
                        ListAllAccounts();
                        break;
                    case "C":
                        IO.Display("");
                        CreateAnAccount();
                        break;
                    case "W":
                        IO.Display("");
                        WorkWithAnAccount();
                        break;
                    case "Q":
                        IO.Display("");
                        running = false;
                        break;
                    case "A":
                        IO.Display("");
                        AddATransaction();
                        break;
                    case "R":
                        IO.Display("");
                        ReviewTransactions();
                        break;
                    case "X":
                        IO.Display("");
                        CurrentAccount = null;
                        break;
                    default:
                        IO.Display("");
                        IO.Display("Huh?");
                        break;
                }

            }

        }

        private void ReviewTransactions()
        {
            DateTime to = DateTime.Now;
            DateTime from = to.AddMonths(-1);
            bool running = true;

            while(running)
            {
                PrintTransactionRange(from, to);
                IO.Display("");
                switch(IO.GetChar("[m]ore?"))
                {
                    case 'm':
                        to = from;
                        from = to.AddMonths(-1);
                        break;
                    default:
                        running = false;
                        break;
                }
            }

        }

        private void PrintTransactionRange(DateTime from, DateTime to)
        {
            Result<List<Transaction>> result = Transactions.GetAllForAccountAndDateRange(CurrentAccount.Id, from, to);
            if (!result.Success)
            {
                DisplayError(result.Errors);
                return;
            }
            foreach (Transaction tran in result.Data)
            {
                IO.Display($"{tran.Id} {tran.Timestamp} {tran.Type} {tran.Amount}");
            }
        }

        private void AddATransaction()
        {
            Transaction tran = new Transaction();
            char type = IO.GetChar("[D]eposit or [W]ithdrawal");
            switch (type)
            {
                case 'D':
                    tran.Type = TransactionType.DEPOSIT;
                    break;
                case 'W':
                    tran.Type = TransactionType.WITHDRAWAL;
                    break;
                default:
                    IO.Error("Invalid transaction type");
                    return;
            }

            tran.Amount = IO.GetDecimal("Amount");
            tran.Timestamp = DateTime.Now;
            tran.AccountId = CurrentAccount.Id;

            Result<Transaction> result = Transactions.Add(tran);
            if (result.Success)
            {
                IO.Display("");
                IO.Display($"Created transaction {result.Data.Id}");
                IO.Display("");
            }
            else
            {
                DisplayError(result.Errors);
            }

            CurrentAccount = BankAccounts.Get(CurrentAccount.Id).Data;
        }


        private void WorkWithAnAccount()
        {
            int acctNum = IO.GetInt("Account number");
            Result<BankAccount> result = BankAccounts.Get(acctNum);
            if (result.Success)
            {
                CurrentAccount = result.Data;
            } else
            {
                DisplayError(result.Errors);
            }
        }

        private void CreateAnAccount()
        {
            string name = IO.GetString("Account holder name");
            decimal startingBalance = IO.GetDecimal("Starting Balance");
            BankAccount acct = new BankAccount();
            acct.AccountHolder = name;
            acct.CurrentBalance = startingBalance;
            Result<BankAccount> result =BankAccounts.Save(acct);
            if (result.Success)
            {
                IO.Display("");
                IO.Display($"Created account #{result.Data.Id}");
            } else
            {
                DisplayError(result.Errors);
            }
             
        }

        private void DisplayError(List<String> errors)
        {
            IO.Error("ERRORS: ");
            foreach (string s in errors)
            {
                IO.Error(s);
            }

            IO.Display("");
        }

        private void ListAllAccounts()
        {
            Result<List<BankAccount>> accounts = BankAccounts.GetAll();
            if (!accounts.Success)
            {
                DisplayError(accounts.Errors);
                return;
            }

            foreach (BankAccount acct in accounts.Data)
            {
                AccountLineListing(acct);
            }

            IO.Display("");
        }

        private void AccountLineListing(BankAccount acct)
        {
            IO.Display($"{acct.Id} - {acct.AccountHolder} - {acct.CurrentBalance:C2}");
        }

        private char DisplayMenu()
        {
            if (CurrentAccount == null)
            {
                return RunNoAccountMenu();
            } 
            else
            {
                return RunAccountMenu();
            }
        }

        private char RunNoAccountMenu()
        {
            IO.Display("L:  List All Accounts");
            IO.Display("C:  Create an Account");
            IO.Display("W:  Work With an Account");
            IO.Display("Q:  Quit");

            return IO.GetChar("");
        }

        private char RunAccountMenu()
        {
            IO.Title($"{CurrentAccount.Id} - {CurrentAccount.AccountHolder} - {CurrentAccount.CurrentBalance}");

            IO.Display("A:  Add a Transaction");
            IO.Display("R:  Review Transactions");
            IO.Display("X:  Exit Account");

            return IO.GetChar("");
        }

        private void DisplayTitle()
        {
            IO.Title("First National Bank of Klueberia");
        }
    }
}
