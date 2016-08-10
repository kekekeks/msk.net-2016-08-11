using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster;

namespace ClusterNode.Actors
{
    public class ShardedActor : ReceiveActor
    {
        private int _entityId;
        private int _shardId;
        public class HelloMessage
        {
             
        }

        protected override void PreStart()
        {
            _entityId = int.Parse(Self.Path.Elements.Last());
            _shardId = int.Parse(Self.Path.Elements[Self.Path.Elements.Count - 2]);

            base.PreStart();
        }

        public ShardedActor()
        {
            Receive<HelloMessage>(_ =>
            {
                Sender.Tell(
                    $"I'm sharded actor #{_entityId} in shard #{_shardId} running on {Cluster.Get(Context.System).SelfAddress}");


            });

        }

    }
}
