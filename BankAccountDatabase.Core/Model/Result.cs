using System;
using System.Collections.Generic;

namespace BankAccountDatabase.Core.Model
{
    public class Result<T>
    {
        public T Data { get; set; }
        public List<String> Errors { get; set; } = new List<string>();
        public bool Success { get
            {
                return Errors.Count == 0;
            } 
        }
    }
}
