using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class AboutUsUIController : CommonController
    {
        // GET: AboutUsUI
        String errMsg = String.Empty;
        DataTable dt;
        DataTable dt1;
        public ActionResult Index()
        {
            layoutpopulate();
            FillAboutUsEdit();
            return View();
        }


        private DataTable FillAboutUsEdit()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt1 = new DataTable();
                using (BAAboutUs oBME = new BAAboutUs())
                {
                    dt1 = oBME.GetData("GET", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.ABOUT_US_DESC = Convert.ToString(dt1.Rows[0]["ABOUT_US_DESC"]);
                    ViewBag.REMARKS = Convert.ToString(dt1.Rows[0]["REMARKS"]);
                    ViewBag.ICON_IMG_1 = ConfigurationManager.AppSettings["ABOUT"].ToString() + Convert.ToString(dt1.Rows[0]["LOGO_1"]);
                    ViewBag.ICON_IMG_2 = ConfigurationManager.AppSettings["ABOUT"].ToString() + Convert.ToString(dt1.Rows[0]["LOGO_2"]);
                    ViewBag.ICON_IMG_3 = ConfigurationManager.AppSettings["ABOUT"].ToString() + Convert.ToString(dt1.Rows[0]["LOGO_3"]);
                }
                return dt1;
            }
            catch (Exception ex)
            {
                return dt1;
            }

        }

    }
}