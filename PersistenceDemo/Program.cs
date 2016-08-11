using System;
using System.Collections.Generic;
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
            var sys = ActorSystem.Create("sys");

            var aref = sys.ActorOf<AccountActor>("account1");
            aref.Tell(new AccountActor.DepositCommand(100));

            Console.WriteLine("Balance: " +
                aref.Ask<decimal>(new AccountActor.GetBalanceCommand()).Result);


            aref.Tell(PoisonPill.Instance);
            try
            {
                Console.WriteLine("Balance: " +
                    aref.Ask<decimal>(new AccountActor.GetBalanceCommand(), TimeSpan.FromSeconds(1)).Result);
            }
            catch
            {
                Console.WriteLine("Operation timed out, actor is dead");
            }


            aref = sys.ActorOf<AccountActor>("account1");
            Console.WriteLine("Balance: " + aref.Ask<decimal>(new AccountActor.GetBalanceCommand()).Result);

            Console.ReadLine();
        }
    }
}
