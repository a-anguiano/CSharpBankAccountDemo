using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountDatabase.Core.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        
        [Column("TransactionType")]
        public TransactionType Type { get; set; }
        
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }

        [Column("AccountId")]
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
    }
}
