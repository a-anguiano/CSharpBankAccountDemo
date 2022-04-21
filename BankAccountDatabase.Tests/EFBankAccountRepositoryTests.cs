using BankAccountDatabase.DAL;
using BankAccountDatabase.DAL.EF;
using BankAccountDatabase.UI;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;  //MUST EXPLICITLY INCLUDE SO THAT ExecuteSQLRaw() works!!!
using BankAccountDatabase.Core.Model;
using System.Linq;

namespace BankAccountDatabase.Tests
{
    public class EFBankAccountRepositoryTests
    {
        EFBankAccountRepository db;
        DBFactory dbf;
        BankAccount Alice = new BankAccount
        {
            Id = 1,
            AccountHolder = "Alice",
            CurrentBalance = 50.00m
        };

        [SetUp]
        public void Setup()
        {
            ConfigProvider cp = new ConfigProvider();
            dbf = new DBFactory(cp.Config, FactoryMode.TEST);
            db = new EFBankAccountRepository(dbf);
            dbf.GetDbContext().Database.ExecuteSqlRaw("SetKnownGoodState");
        }

        [Test]
        public void TestGetAll()
        {
            Assert.AreEqual(2, db.GetAll().Count);
        }

        [Test]
        public void TestGetOne()
        { 
            Assert.AreEqual(Alice, db.Get(1));
        }

        [Test]
        public void TestAdd()
        {
            BankAccount expected = new BankAccount
            {
                AccountHolder = "Carol",
                CurrentBalance = 0.00m
            };

            db.Add(expected);
            expected.Id = 3;

            Assert.AreEqual(expected, db.Get(3));
        }

        [Test]
        public void TestDelete()
        {
            BankAccount actual = db.Delete(1);
            Assert.AreEqual(Alice.Id, actual.Id);
            Assert.AreEqual(Alice.AccountHolder, actual.AccountHolder);
            Assert.AreEqual(Alice.CurrentBalance, actual.CurrentBalance);

            Assert.AreEqual(1, db.GetAll().Count);
            using (var db = dbf.GetDbContext())
            {
                //Should only be one account in the database now-- see test/sql/knowngoodstate.sql for details
                Assert.AreEqual(0, db.Transactions.Where(t => t.BankAccountId == 1).ToList().Count);
            }
        }

        [Test]
        public void TestUpdate()
        {
            Alice.AccountHolder = "Not Alice";
            Alice.CurrentBalance = 20.00m;
            db.Update(Alice);
            BankAccount actual = db.Get(1);
            Assert.AreEqual(Alice.Id, actual.Id);
            Assert.AreEqual(Alice.AccountHolder, actual.AccountHolder);
            Assert.AreEqual(Alice.CurrentBalance, actual.CurrentBalance);
        }
    }
}