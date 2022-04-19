using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BankAccountDatabase.DAL
{
    public class SqlTransactionRepository : ITransactionRepository
    {
        public string ConnectionString { get; set; }

        public SqlTransactionRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public Transaction Add(Transaction transaction)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Transactions (AccountId, TransactionType, Timestamp, Amount, Note) " +
                             "VALUES (@AccountId, @TransactionType, @Timestamp, @Amount, @Note) " +
                             "SELECT SCOPE_IDENTITY()";

                connection.Open();

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@AccountId", transaction.AccountId);
                command.Parameters.AddWithValue("@TransactionType", transaction.Type.ToString());
                command.Parameters.AddWithValue("@Timestamp", transaction.Timestamp);
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Note", transaction.Note);

                transaction.Id = Convert.ToInt32(command.ExecuteScalar());

            }

            return transaction;
        }

        public Transaction Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Transaction Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> GetAllForAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> GetAllForAccountWithinDateRange(int accountId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
