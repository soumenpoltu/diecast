using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class UploadDecalsDetailsUIController : CommonController
    {
        // GET: UploadDecalsDetailsUI
        public ActionResult Index()
        {
            layoutpopulate();
            return View();
        }
    }
}