using BankAccountDatabase.Core.Interface;
using BankAccountDatabase.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BankAccountDatabase.DAL
{
    public class SqlBankAccountRepository : IBankAccountRepository
    {
        public string ConnectionString { get; set; }
        public SqlBankAccountRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public BankAccount Add(BankAccount bankAccount)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO BankAccounts (AccountHolder, CurrentBalance) " +
                             "VALUES (@AccountHolder, @CurrentBalance) " +
                             "SELECT SCOPE_IDENTITY()";

                connection.Open();

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@AccountHolder", bankAccount.AccountHolder);
                command.Parameters.AddWithValue("@CurrentBalance", bankAccount.CurrentBalance);

                bankAccount.Id = Convert.ToInt32(command.ExecuteScalar());
                
            }

            return bankAccount;
        }

        public BankAccount Delete(int id)
        {
            throw new NotImplementedException();
        }

        public BankAccount Get(int id)
        {
            List<BankAccount> result = new List<BankAccount>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT Id, AccountHolder, CurrentBalance " +
                             "FROM BankAccounts " +
                             "WHERE Id = @Id";

                connection.Open();

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BankAccount acct = new BankAccount();
                        acct.Id = (int)reader["Id"];
                        acct.AccountHolder = reader["AccountHolder"].ToString();
                        acct.CurrentBalance = (decimal)reader["CurrentBalance"];
                        result.Add(acct);
                    }
                }
            }

            if (result.Count == 0)
            {
                throw new Exception("No results found.");
            }

            return result[0];
        }

        public List<BankAccount> GetAll()
        {
            List<BankAccount> result = new List<BankAccount>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT Id, AccountHolder, CurrentBalance " + 
                             "FROM BankAccounts";

                connection.Open();

                var command = new SqlCommand(sql, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BankAccount acct = new BankAccount();
                        acct.Id = (int)reader["Id"];
                        acct.AccountHolder = reader["AccountHolder"].ToString();
                        acct.CurrentBalance = (decimal)reader["CurrentBalance"];
                        result.Add(acct);
                    }
                }
            }

            return result;
        }

        public void Update(BankAccount bankAccount)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "UPDATE BankAccounts " +
                             "SET AccountHolder = @AccountHolder " +
                             ",   CurrentBalance = @CurrentBalance " +
                             "WHERE Id = @Id ";

                connection.Open();

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@AccountHolder", bankAccount.AccountHolder);
                command.Parameters.AddWithValue("@CurrentBalance", bankAccount.CurrentBalance);
                command.Parameters.AddWithValue("@Id", bankAccount.Id);

                

                bankAccount.Id = command.ExecuteNonQuery();

            }
        }
    }
}
