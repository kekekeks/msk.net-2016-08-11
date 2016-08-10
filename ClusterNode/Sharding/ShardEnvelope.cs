using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Sharding;

namespace ClusterNode.Sharding
{
    public class ShardEnvelope
    {
        public int ShardId { get; }
        public int EntityId { get; }
        public object Message { get; }

        public ShardEnvelope(int shardId, int entityId, object message)
        {
            ShardId = shardId;
            EntityId = entityId;
            Message = message;
        }
    }

    public sealed class MessageExtractor : IMessageExtractor
    {
        public string EntityId(object message)
        {
            return (message as ShardEnvelope)?.EntityId.ToString();
        }

        public string ShardId(object message)
        {
            return (message as ShardEnvelope)?.ShardId.ToString();
        }

        public object EntityMessage(object message)
        {
            return (message as ShardEnvelope)?.Message;
        }
    }

}
