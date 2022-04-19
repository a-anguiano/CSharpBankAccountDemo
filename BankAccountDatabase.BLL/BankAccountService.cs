using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;

namespace BankAccountDatabase.BLL
{
    public class BankAccountService : IBankAccountService
    {
        public IBankAccountRepository Repo { get; set; }
        public BankAccountService(IBankAccountRepository repo)
        {
            Repo = repo;
        }


        public Result<BankAccount> Delete(int id)
        {
            Result<BankAccount> result = new Result<BankAccount>();
            try
            {
                result.Data = Repo.Delete(id);
            } catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }

        public Result<BankAccount> Get(int id)
        {
            Result<BankAccount> result = new Result<BankAccount>();
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

        public Result<List<BankAccount>> GetAll()
        {
            Result<List<BankAccount>> result = new Result<List<BankAccount>>();
            try
            {
                result.Data = Repo.GetAll();
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }

        public Result<BankAccount> Save(BankAccount bankAccount)
        {
            Result<BankAccount> result = new Result<BankAccount>();
            try
            {
                if (bankAccount.Id != 0)
                {
                    result.Data = bankAccount;
                    Repo.Update(bankAccount);
                } else
                {
                    result.Data = Repo.Add(bankAccount);
                }
            } catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }
    }
}
