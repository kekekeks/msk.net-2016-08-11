using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using Akka.Persistence;

namespace PersistenceDemo
{
    public class AccountActor : PersistentActor
    {
        public class WithdrawCommand
        {
            public WithdrawCommand(decimal amount)
            {
                Amount = amount;
            }

            public decimal Amount { get; }
        }

        public class DepositCommand
        {
            public DepositCommand(decimal amount)
            {
                Amount = amount;
            }

            public decimal Amount { get; }
        }

        public class GetBalanceCommand
        {
             
        }

        private decimal _balance;

        void ProcessCommand(object cmd)
        {
            cmd.Match()
                .With<DepositCommand>(d => _balance += d.Amount)
                .With<WithdrawCommand>(d => _balance -= d.Amount);
        }

        protected override bool ReceiveRecover(object message)
        {
            ProcessCommand(message);
            return true;
        }

        protected override bool ReceiveCommand(object message)
        {
            if (message is WithdrawCommand || message is DepositCommand)
                Persist(message, ProcessCommand);
            if (message is GetBalanceCommand)
                Sender.Tell(_balance);
            return true;
        }

        public override string PersistenceId => Self.Path.ToString();
    }
}
