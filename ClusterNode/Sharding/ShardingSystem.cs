using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Sharding;
using ClusterNode.Actors;

namespace ClusterNode.Sharding
{
    public static class ShardingSystem
    {
        public static ICanTell Initialize(ActorSystem system, bool client)
        {
            if (!client)
                return ClusterSharding.Get(system).Start(
                    typeName: "my-sharded-actor",
                    entityProps: Props.Create<ShardedActor>(),
                    settings: ClusterShardingSettings.Create(system),
                    messageExtractor: new MessageExtractor()
                    );
            else
                return ClusterSharding.Get(system).StartProxy("my-sharded-actor", "server", new MessageExtractor());



            // send message to entity through shard region
            //region.Tell(new Envelope(shardId: 1, entityId: 1, message: "hello"))
        }
    }
}
