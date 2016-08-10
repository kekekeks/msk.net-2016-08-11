using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Akka.Actor;
using ClusterClient.Actors;
using ClusterNode.Actors;

namespace ClusterClient.Controllers
{
    public class HelloController : Controller
    {
        public async Task<ActionResult> Index(string hello)
        {
            var resp = await SystemActors.HelloActor.Ask<ClusterHelloActor.HelloResponseMessage>
                (new ClusterHelloActor.HelloMessage(hello ?? "Hello!"), TimeSpan.FromSeconds(5));
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

    }
}