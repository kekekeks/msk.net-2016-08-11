using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace PersistenceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Directory.Exists("snapshots"))
                Directory.Delete("snapshots", true);




            var sys = ActorSystem.Create("sys");

            var aref = sys.ActorOf<AccountActor>("account1");
            aref.Tell(new AccountState.MoneyCommand(100));
            aref.Tell(new AccountState.MoneyCommand(101));
            aref.Tell(new AccountState.MoneyCommand(102));
            aref.Tell(new AccountState.MoneyCommand(103));
            aref.Tell(new AccountState.MoneyCommand(104));


            Console.WriteLine("Balance: " +
                aref.Ask<decimal>(new GetBalanceCommand()).Result);


            aref.Tell(PoisonPill.Instance);
            try
            {
                Console.WriteLine("Balance: " +
                    aref.Ask<decimal>(new GetBalanceCommand(), TimeSpan.FromSeconds(1)).Result);
            }
            catch
            {
                Console.WriteLine("Operation timed out, actor is dead");
            }


            aref = sys.ActorOf<AccountActor>("account1");
            Console.WriteLine("Balance: " + aref.Ask<decimal>(new GetBalanceCommand()).Result);

            Console.ReadLine();
        }
    }
}
