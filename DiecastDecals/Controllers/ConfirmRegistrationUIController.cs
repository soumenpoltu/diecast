using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class ConfirmRegistrationUIController : CommonController
    {
        // GET: ConfirmRegistration

        Byte vRef = 0; Int32 vKey = 0;
        String errMsg = String.Empty;
        String vString2 = String.Empty;
        String vString3 = String.Empty;
        DataTable dt = null;
        DataTable dt2 = null;
        DataTable dt1 = null;
        DataTable dt10 = null;
        public ActionResult Index()
        {
            layoutpopulate();

            return View();
        }

    }
}