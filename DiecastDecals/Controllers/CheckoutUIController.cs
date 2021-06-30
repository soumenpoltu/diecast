using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class CheckoutUIController : CommonController
    {
        // GET: CheckoutUI
        public ActionResult Index()
        {
            layoutpopulate();
            return View();
        }
    }
}