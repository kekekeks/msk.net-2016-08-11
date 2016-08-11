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
    public class SnapshotAwareAccountActor : PersistentActor
    {
        private AccountState _state = new AccountState();
        private int _commandsSinceSnapshot = 0;

        protected override bool ReceiveCommand(object message)
        {
            if (message is AccountState.MoneyCommand)
            {
                Persist(message, cmd =>
                {
                    _state.ProcessCommand(cmd);
                    _commandsSinceSnapshot++;

                    if (_commandsSinceSnapshot >= 3)
                    {
                        SaveSnapshot(_state);
                        _commandsSinceSnapshot = 0;
                    }

                });

            }
            if (message is GetBalanceCommand)
                Sender.Tell(_state.Balance);
            return true;
        }

        protected override bool ReceiveRecover(object message)
        {
            var snap = message as SnapshotOffer;
            if (snap != null)
                _state = (AccountState) snap.Snapshot;
            else
                _state.ProcessCommand(message);
            return true;
        }

        public override string PersistenceId => Self.Path.ToString();
    }
}
