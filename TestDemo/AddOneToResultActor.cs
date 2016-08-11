using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace TestDemo
{
    public class AddOneToResultAsyncActor : ReceiveActor
    {
        private readonly ICanTell _target;

        public AddOneToResultAsyncActor(ICanTell target)
        {
            _target = target;
        }

        public AddOneToResultAsyncActor()
        {
            ReceiveAsync<string>(async s =>
            {
                int result = await _target.Ask<int>(s);
                Sender.Tell(result + 1);
            });
        }
    }

    public class AddOneToResultActor : ReceiveActor
    {
        public AddOneToResultActor(ICanTell target)
        {

            Receive<string>(s =>
            {
                var sender = Sender;
                target.Ask<int>(s).ContinueWith(t =>
                {
                    if (t.IsCompleted)
                        sender.Tell(t.Result + 1);
                });
            });
            Receive<int>(i => Sender.Tell(i + 1));
        }
    }


    public class AddOneToResultActor2 : ReceiveActor
    {
        public AddOneToResultActor2(ICanTell target)
        {
            Receive<string>(s =>
            {
                target.Ask<int>(s).PipeTo(Sender, success: i => i + 1);
            });
        }
    }
}
