using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Dsl;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Xunit;

namespace TestDemo
{
    public class Tests : TestKit
    {
        
        [Fact]
        public void Actor_Should_Respond_To_Hello_Message()
        {
            Sys.ActorOf<ActorUnderTest>().Tell(new ActorUnderTest.HelloMessage("HELO"));
            ExpectMsg<ActorUnderTest.EhloMessage>((msg, sender) => msg.Elho == "OLEH", TimeSpan.FromSeconds(1));


        }

        
        [Fact]
        public void Actor_Should_Respond_In_One_Second()
        {
            Within(TimeSpan.FromSeconds(1), () =>
            {
                Sys.ActorOf<ActorUnderTest>().Tell(new ActorUnderTest.HelloMessage("HELO"));
                ExpectMsg<ActorUnderTest.EhloMessage>((msg, sender) => msg.Elho == "OLEH");
            });
        }

        [Fact]
        public void Actor_Should_Become_Ready_After_Getting_Message()
        {
            var actor = Sys.ActorOf<StatefulActor>();
            actor.Tell("START");
            AwaitAssert(() =>
            {
                actor.Tell("STATUS");
                ExpectMsg("READY");
            }, TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(250));
        }

        [Fact]
        public void Actor_Should_Change_Internal_State()
        {
            var testRef = ActorOfAsTestActorRef<StatefulActor>(Props.Create<StatefulActor>());
            testRef.Tell("START");
            Assert.NotNull(testRef.UnderlyingActor.StartedAt);
        }


    }
}
