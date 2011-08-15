using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainsUnobtrusiveExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "DataAnnotations example by Jim Schubert";

            return View();
        }

        public ActionResult Example()
        {
            return View();
        }

        [HttpPost]
        public void Example(Example example)
        {
            if (ModelState.IsValid)
            {
                // Now, examine the example parameter and see if your values are correct!
                System.Diagnostics.Debugger.Break();
                Response.Redirect("~/");
            }
        }
    }
}
