using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using ClusterNode.Actors;
using ClusterNode.Sharding;

namespace ClusterNode
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ((AkkaConfigurationSection) ConfigurationManager.GetSection("akka")).AkkaConfig;

            Console.Title = "Node " + GetPortNumber();
            config = ConfigurationFactory.ParseString($"akka.remote.helios.tcp.port={GetPortNumber()}")
                .WithFallback(config);
            var system = ActorSystem.Create("cluster", config);

            var clusterStatus = system.ActorOf<ClusterListenerActor>();
            system.ActorOf<ClusterHelloActor>("hello");


            ShardingSystem.Initialize(system, false);

            while (true)
            {
                var line = Console.ReadLine();
                if (line == null || line == "exit")
                {
                    Console.WriteLine("Shutting down");
                    system.Terminate();
                    system.WhenTerminated.Wait(TimeSpan.FromSeconds(5));
                    return;
                }
                if (line == "status")
                {
                    Console.WriteLine("Online nodes: \r\n" +
                                      string.Join("\r\n",
                                          clusterStatus.Ask<ClusterListenerActor.GetActiveMembersResponseMessage>(
                                              new ClusterListenerActor.GetActiveMembersMessage()).Result.Members));
                }
            }
        }




        static int GetPortNumber()
        {
            return 2550 + Process.GetProcesses().Count(p => p.ProcessName.StartsWith("ClusterNode")) - 1;
        }




    }
}
