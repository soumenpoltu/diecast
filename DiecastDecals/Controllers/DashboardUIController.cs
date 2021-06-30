using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class DashboardUIController : CommonController
    {
        // GET: DashboardUI
        public ActionResult Index()
        {
            layoutpopulate();
            return View();
        }
    }
}