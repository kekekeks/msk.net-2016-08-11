using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Akka.Actor;
using ClusterClient.Actors;
using ClusterNode.Actors;
using ClusterNode.Sharding;

namespace ClusterClient.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Hello(string hello)
        {
            var resp = await SystemActors.HelloActor.Ask<ClusterHelloActor.HelloResponseMessage>
                (new ClusterHelloActor.HelloMessage(hello ?? "Hello!"), TimeSpan.FromSeconds(5));
            return Json(resp, JsonRequestBehavior.AllowGet);
        }











        public async Task<ActionResult> Shard(int shardId, int entityId) => Content(await

            SystemActors.Sharded.Ask<string>(new ShardEnvelope(shardId, entityId, new ShardedActor.HelloMessage()),
                TimeSpan.FromSeconds(5)));


        public ActionResult Index() => RedirectToAction(nameof(Hello));

    }
}