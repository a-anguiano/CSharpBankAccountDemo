using System;
using BankAccountDatabase.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace BankAccountDatabase.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public AppDbContext() : base()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
            .Entity<Transaction>()
            .Property(e => e.Type)
            .HasConversion(
                v => v.ToString(),
                v => (TransactionType)Enum.Parse(typeof(TransactionType), v));
        }
    }
}
