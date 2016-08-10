using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace TestDemo
{
    public class ActorUnderTest : ReceiveActor
    {
        public class HelloMessage
        {
            public HelloMessage(string hello)
            {
                Hello = hello;
            }

            public string Hello { get; }
        }

        public class EhloMessage
        {
            public EhloMessage(string elho)
            {
                Elho = elho;
            }

            public string Elho { get; }
        }


        public ActorUnderTest()
        {
            Receive<HelloMessage>(hello =>
                Sender.Tell(new EhloMessage(new string(hello.Hello.Reverse().ToArray()))));
        }
    }
}
