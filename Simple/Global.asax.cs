using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Akka.Actor;
using Simple.Actors;

namespace Simple
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static ActorSystem ActorSystem;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            InitializeActorSystem();
        }

        void InitializeActorSystem()
        {
            ActorSystem = ActorSystem.Create("sys");
            SystemActors.HelloActor = ActorSystem.ActorOf<HelloActor>("hello");
        }

        protected void Application_End()
        {
            ActorSystem.Terminate();
            ActorSystem.WhenTerminated.Wait(TimeSpan.FromSeconds(5));
        }


    }
}
