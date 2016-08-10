using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Akka.Actor;

namespace Simple.Actors
{
    public class HelloActor : ReceiveActor
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

            public HelloResponseMessage(string elho)
            {
                Elho = elho;
            }
        }

        public HelloActor()
        {
            Receive<HelloMessage>(hello =>
            {
                Sender.Tell(new HelloResponseMessage(new string(hello.Hello.Reverse().ToArray())));
            });
        }



    }
}