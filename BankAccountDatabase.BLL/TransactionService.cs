using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;

namespace BankAccountDatabase.BLL
{
    public class TransactionService : ITransactionService
    {
        ITransactionRepository Repo { get; set; }
        IBankAccountService BankAccounts { get; set; }

        public TransactionService(ITransactionRepository repo, IBankAccountService bankAccounts)
        {
            Repo = repo;
            BankAccounts = bankAccounts;
        }

        public Result<Transaction> Delete(int id)
        {
            Result<Transaction> result = new Result<Transaction>();
            try
            {
                result.Data = Repo.Delete(id);
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
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

            Result<BankAccount> acctResult = BankAccounts.Get(transaction.AccountId);

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
                try
                {
                    result.Data = Repo.Add(transaction);                    
                } catch (Exception e)
                {
                    result.Errors.Add(e.Message);
                }

                if (result.Success)
                {
                    try
                    {
                        Result<BankAccount> baResult = BankAccounts.Save(acct);
                        if (!baResult.Success)
                        {
                            foreach (String error in baResult.Errors)
                            {
                                result.Errors.Add(error);
                            }
                            Repo.Delete(result.Data.Id);
                        }

                    } catch (Exception e)
                    {
                        result.Errors.Add(e.Message);
                        Repo.Delete(result.Data.Id);
                    }
                }
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
                errors.Add("Declined:  Insufficient Funds");
            }

            return delta;
        }
    }
}
