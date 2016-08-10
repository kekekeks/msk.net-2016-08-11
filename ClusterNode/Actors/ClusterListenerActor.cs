using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster;

namespace ClusterNode.Actors
{
    class ClusterListenerActor : ReceiveActor
    {
        private readonly HashSet<string> _members = new HashSet<string>();

        public class GetActiveMembersMessage
        {
            
             
        }

        public class GetActiveMembersResponseMessage
        {
            public List<string> Members{get; set; }
        }

        public ClusterListenerActor()
        {
            Cluster.Get(Context.System).Subscribe(Self, typeof(ClusterEvent.IMemberEvent));

            Receive<ClusterEvent.IMemberEvent>(status =>
            {
                if (status.Member.Status == MemberStatus.Up)
                    _members.Add(status.Member.Address.ToString());
                else
                    _members.Remove(status.Member.Address.ToString());
            });
            Receive<GetActiveMembersMessage>(_ => Sender.Tell(new GetActiveMembersResponseMessage {Members = _members.ToList()}));

        }

    }
}
