using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class TrackOrderUIController : CommonController
    {
        // GET: TrackOrder
        public ActionResult Index()
        {
            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {
                layoutpopulate();
                return View();
            }
            else
            {
                return Redirect("~/LoginUI/Index");
            }
        }
    }
}