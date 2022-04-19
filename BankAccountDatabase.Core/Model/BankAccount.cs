using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountDatabase.Core.Model
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountHolder { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
