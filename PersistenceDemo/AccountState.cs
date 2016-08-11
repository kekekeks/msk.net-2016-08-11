using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;

namespace PersistenceDemo
{
    public struct AccountState
    {
        public class MoneyCommand
        {
            public MoneyCommand(decimal amount)
            {
                Amount = amount;
            }

            public decimal Amount { get; }
        }


        public decimal Balance { get; private set; }

        public void ProcessCommand(object command)
        {
            var m = command as MoneyCommand;
            if (m != null)
                Balance += m.Amount;
        }
    }
}
