using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Akka.Actor;
using Akka.Cluster.Routing;
using Akka.Configuration.Hocon;
using Akka.Remote.Routing;
using Akka.Routing;
using Akka.Util.Internal;
using ClusterClient.Actors;
using ClusterNode.Actors;

namespace ClusterClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            InitializeActorSystem();
        }

        void InitializeActorSystem()
        {
            var conf = ((AkkaConfigurationSection) ConfigurationManager.GetSection("akka")).AkkaConfig;
            ActorSystem = ActorSystem.Create("cluster", conf);




            var routees = new[] {"/user/hello"};
            SystemActors.HelloActor =
                ActorSystem.ActorOf(Props.Create<ClusterHelloActor>().WithRouter(
                    new ClusterRouterGroup(new RoundRobinGroup(routees), 
                        new ClusterRouterGroupSettings(int.MaxValue, routees, false, "server"))
                    ), "hello");




            //x => ((ClusterHelloActor.HelloMessage) x).Hello.GetHashCode()
        }

        public static ActorSystem ActorSystem { get; set; }

        protected void Application_End()
        {
            ActorSystem.Terminate();
            ActorSystem.WhenTerminated.Wait(TimeSpan.FromSeconds(5));
        }
    }
}
