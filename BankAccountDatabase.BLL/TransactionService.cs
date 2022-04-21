using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;

namespace BankAccountDatabase.BLL
{
    public class TransactionService : ITransactionService
    {
        public const string NSF_MESSAGE = "Declined:  Insufficient Funds";

        ITransactionRepository Repo { get; set; }
        IBankAccountService BankAccounts { get; set; }

        public TransactionService(ITransactionRepository repo, IBankAccountService bankAccounts)
        {
            Repo = repo;
            BankAccounts = bankAccounts;
        }

        public Result<Transaction> Get(int id)
        {
            Result<Transaction> result = new Result<Transaction>();
            try
            {
                result.Data = Repo.Get(id);
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }

        public Result<List<Transaction>> GetAllForAccount(int accountId)
        {
            Result<List<Transaction>> result = new Result<List<Transaction>>();
            try
            {
                result.Data = Repo.GetAllForAccount(accountId);
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }

        public Result<List<Transaction>> GetAllForAccountAndDateRange(int accountId, DateTime from, DateTime to)
        {
            Result<List<Transaction>> result = new Result<List<Transaction>>();
            try
            {
                result.Data = Repo.GetAllForAccountWithinDateRange(accountId, from, to);
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }

        public Result<Transaction> Add(Transaction transaction)
        {
            Result<Transaction> result = new Result<Transaction>();

            Result<BankAccount> acctResult = BankAccounts.Get(transaction.BankAccountId);

            if (!acctResult.Success)
            {
                result.Errors.Add("Account retrieval failed: " + string.Join(',', acctResult.Errors));
                return result;
            }

            BankAccount acct = acctResult.Data;
            transaction.Note = "";

            decimal delta = ValidateTransaction(acct, transaction, result.Errors);

            if (result.Success)
            {
                acct.CurrentBalance += delta;

                Repo.AddTransactionToAccount(acct, transaction);
            }

            return result;
        }

        private decimal ValidateTransaction(BankAccount acct, Transaction transaction, List<String> errors)
        {
            decimal delta = transaction.Amount;

            if (transaction.Type == TransactionType.WITHDRAWAL)
            {
                delta *= -1;
            }

            if (acct.CurrentBalance + delta < 0)
            {
                errors.Add(NSF_MESSAGE);
            }

            return delta;
        }
    }
}
