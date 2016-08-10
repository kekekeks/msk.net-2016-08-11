using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace TestDemo
{
    class StatefulActor : ReceiveActor
    {
        public DateTime? StartedAt;
        public StatefulActor()
        {
            Receive<string>(cmd =>
            {
                if(cmd == "START")
                    StartedAt = DateTime.UtcNow;
                if (cmd == "STATUS")
                    Sender.Tell(((DateTime.UtcNow - StartedAt)?.TotalSeconds > 1) ? "READY" : "STARTING");
            });
        }

    }
}
