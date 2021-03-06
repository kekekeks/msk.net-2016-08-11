﻿using System;
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
        private AccountState _state = new AccountState();

        protected override bool ReceiveCommand(object message)
        {
            if (message is AccountState.MoneyCommand)
                Persist(message, _state.ProcessCommand);
            if (message is GetBalanceCommand)
                Sender.Tell(_state.Balance);
            return true;
        }

        protected override bool ReceiveRecover(object message)
        {
            _state.ProcessCommand(message);
            return true;
        }

        public override string PersistenceId => Self.Path.ToString();
    }
}
