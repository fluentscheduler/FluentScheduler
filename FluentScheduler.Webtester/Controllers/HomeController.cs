using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace FluentScheduler.WebTester.Controllers
{
    public class HomeController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("license")]
        public ActionResult License()
        {
            return View();
        }
    }
}
