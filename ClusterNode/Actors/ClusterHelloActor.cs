using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster;

namespace ClusterNode.Actors
{
    public class ClusterHelloActor : ReceiveActor
    {
        public class HelloMessage
        {
            public string Hello { get; set; }

            public HelloMessage(string hello)
            {
                Hello = hello;
            }
        }

        public class HelloResponseMessage
        {
            public string Elho { get; set; }
            public string Address { get; set; }

            public HelloResponseMessage(string elho, string address)
            {
                Elho = elho;
                Address = address;
            }
        }

        public ClusterHelloActor()
        {
            Receive<HelloMessage>(hello =>
            {
                Sender.Tell(new HelloResponseMessage(new string(hello.Hello.Reverse().ToArray()),
                    Cluster.Get(Context.System).SelfAddress.ToString()));
            });
        }

    }
}
