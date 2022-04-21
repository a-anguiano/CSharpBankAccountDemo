using BankAccountDatabase.Core.Model;
using BankAccountDatabase.DAL;
using NUnit.Framework;
using System;
using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.BLL;

namespace BankAccountDatabase.Tests
{
    class TransactionServiceTests
    {
        ITransactionRepository Transactions;
        IBankAccountRepository BankAccounts;
        BankAccount expectedAccount;
        ITransactionService svc;
        Transaction expectedDeposit;

        Transaction expectedWithdrawal;

        [SetUp]
        public void SetUp()
        { 
            BankAccounts = new InMemoryBankAccountRepository();
            Transactions = new InMemoryTransactionRepository(BankAccounts);
            BankAccountService bs = new BankAccountService(BankAccounts);
            svc = new TransactionService(Transactions, bs);
            expectedAccount = new BankAccount
            {
                AccountHolder = "Alice",
                CurrentBalance = 0.00m
            };
            expectedDeposit = new Transaction
            {
                BankAccountId = 1,
                Type = TransactionType.DEPOSIT,
                Amount = 10.00m,
                Timestamp = DateTime.Now,
                Note = "funky note here"
            };
            expectedWithdrawal = new Transaction
            {
                BankAccountId = 1,
                Type = TransactionType.WITHDRAWAL,
                Amount = 10.00m,
                Timestamp = DateTime.Now,
                Note = "funky note here"
            };
            BankAccounts.Add(expectedAccount);
        }

        [Test] 
        public void TestApprovedDeposit()
        {
       
            Result<Transaction> actual = svc.Add(expectedDeposit);
            Assert.IsTrue(actual.Success);

            BankAccount actualAccount = BankAccounts.Get(1);
            Assert.AreEqual(expectedDeposit.Amount, actualAccount.CurrentBalance);

        }

        [Test]
        public void TestApprovedWithdrawal()
        {
            svc.Add(expectedDeposit);
            Result<Transaction> actual = svc.Add(expectedWithdrawal);
            Assert.IsTrue(actual.Success);

            BankAccount actualAccount = BankAccounts.Get(1);
            Assert.AreEqual(0.00m, actualAccount.CurrentBalance);
        }

        [Test]
        public void TestDeclinedDeposit()
        {
            BankAccount expectedAccount = BankAccounts.Get(1);
            expectedAccount.CurrentBalance = 0.00m;
            BankAccounts.Update(expectedAccount);

            expectedDeposit.Amount = -20.00m; //A deposit reversal!
            svc.Add(expectedDeposit);
            Result<Transaction> actual = svc.Add(expectedWithdrawal);
            Assert.IsFalse(actual.Success);
            Assert.AreEqual(actual.Errors[0], TransactionService.NSF_MESSAGE);

            BankAccount actualAccount = BankAccounts.Get(1);
            Assert.AreEqual(0.00m, actualAccount.CurrentBalance);
        }

        [Test]
        public void TestDeclinedWithdrawal()
        {
            BankAccount expectedAccount = BankAccounts.Get(1);
            expectedAccount.CurrentBalance = 0.00m;
            BankAccounts.Update(expectedAccount);

            Result<Transaction> actual = svc.Add(expectedWithdrawal);
            Assert.IsFalse(actual.Success);
            Assert.AreEqual(actual.Errors[0], TransactionService.NSF_MESSAGE);

            BankAccount actualAccount = BankAccounts.Get(1);
            Assert.AreEqual(0.00m, actualAccount.CurrentBalance);
        }

        [Test]
        public void TestAccountNotFound()
        {
            expectedWithdrawal.BankAccountId = 1002349;
            Result<Transaction> actual = svc.Add(expectedWithdrawal);
            Assert.IsFalse(actual.Success);
            Assert.AreEqual("Account retrieval failed: The given key '1002349' was not present in the dictionary.", actual.Errors[0]);
        }
    }
}
