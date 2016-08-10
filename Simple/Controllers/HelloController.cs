using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Akka.Actor;
using Simple.Actors;

namespace Simple.Controllers
{
    public class HelloController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var resp = await SystemActors.HelloActor.Ask<HelloActor.HelloResponseMessage>
                (new HelloActor.HelloMessage("Hello!"));
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

    }
}