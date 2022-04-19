using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountDatabase.Core.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }

    }
}
