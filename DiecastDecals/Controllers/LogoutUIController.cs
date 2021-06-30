using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class LogoutUIController : Controller
    {
        // GET: LogoutUI

        [HttpGet]
        public ActionResult Index()
        {
            Session.Abandon();
            Session.Clear();
            if (Session["oUser"] != null)
                Session["oUser"] = null;
            if (Session["urlpath"] != null)
                Session["urlpath"] = null;
            if (Session["val"] != null)
                Session["val"] = null;
            if (Session["oSysUser"] != null)
                Session["oSysUser"] = null;

            HttpCookie cookie = Request.Cookies["__asd"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return Redirect("/IndexUI/Index");
        }
    }
}