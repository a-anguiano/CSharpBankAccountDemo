using BankAccountDatabase.Core.Model;
using BankAccountDatabase.DAL;
using BankAccountDatabase.DAL.EF;
using BankAccountDatabase.UI;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore; //MUST EXPLICITLY INCLUDE SO THAT ExecuteSQLRaw() works!!!
using System;
using System.Collections.Generic;

namespace BankAccountDatabase.Tests
{
    class EFTransactionRepositoryTests
    {
        EFTransactionRepository db;
        DBFactory dbf;
        Transaction expected = new Transaction
        {
            Id = 1,
            BankAccountId = 1,
            Type = TransactionType.DEPOSIT,
            //Timestamp is variable!!!!! probably should consider changing that
            Amount = 50.00m,
            Note = "Initial Deposit"
        };

        private bool CompareTransactionsWithoutTimestamp(Transaction expected, Transaction actual)
        {
            return expected.Id == actual.Id && expected.BankAccountId == actual.BankAccountId &&
                   expected.Type == actual.Type && expected.Amount == actual.Amount &&
                   expected.Note == actual.Note;
        }

        [SetUp]
        public void SetUp()
        {
            ConfigProvider cp = new ConfigProvider();
            dbf = new DBFactory(cp.Config, FactoryMode.TEST);
            db = new EFTransactionRepository(dbf);
            dbf.GetDbContext().Database.ExecuteSqlRaw("SetKnownGoodState");
        }

        [Test]
        public void TestAdd()
        {
            Transaction expected = new Transaction
            {
                BankAccountId = 1,
                Type = TransactionType.DEPOSIT,
                Amount = 10.00m,
                Timestamp = DateTime.Now,
                Note = "A note"
            };
            BankAccount one = dbf.GetDbContext().BankAccounts.Find(1);
            db.AddTransactionToAccount(one, expected);
            Assert.IsTrue(CompareTransactionsWithoutTimestamp(expected, db.Get(expected.Id)));
        }

        [Test]
        public void TestGet()
        {
            Assert.IsTrue(CompareTransactionsWithoutTimestamp(expected, db.Get(1)));
        }

        [Test]
        public void TestGetAllForAccount()
        {
            Transaction muddyTheWaters = new Transaction
            {
                BankAccountId = 2,
                Amount = 20.00m,
                Type = TransactionType.DEPOSIT,
                Note = "",
                Timestamp = DateTime.Now
            };
            BankAccount two = dbf.GetDbContext().BankAccounts.Find(2);
            db.AddTransactionToAccount(two, muddyTheWaters);

            List<Transaction> actual = db.GetAllForAccount(1);
            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(CompareTransactionsWithoutTimestamp(expected, actual[0]));
        }

        [Test]
        public void TestGetAllForAccountAndDateRangeWithinRange()
        {
            List<Transaction> actual = db.GetAllForAccountWithinDateRange(1, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(1));
            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(CompareTransactionsWithoutTimestamp(expected, actual[0]));
        }

        [Test]
        public void TestGetAllForAccountOutsideOfRange()
        {
            List<Transaction> actual = db.GetAllForAccountWithinDateRange(1, DateTime.Now.AddDays(-200), DateTime.Now.AddDays(-100));
            Assert.AreEqual(0, actual.Count);
        }

        
    }
}
