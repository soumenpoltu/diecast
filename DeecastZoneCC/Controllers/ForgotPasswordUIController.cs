using MyApp.db.MyAppBal;
using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class ForgotPasswordUIController : Controller
    {
        // GET: ForgotPasswordUI
        DataTable dt;
        DataTable dt1;
        String errMsg = String.Empty;
        String vString = String.Empty, vString1 = String.Empty, vString2 = String.Empty, vString3 = String.Empty, vString4 = String.Empty;
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult btn_submit_Click(FormCollection form)
        {
            EntityDtlsCustomer oBMST = new EntityDtlsCustomer();
            String errMsg = String.Empty;
            String to_username = form["useremail"];
            //String emailId = txt_User_Email.Text;

            DataTable dt = new DataTable();
            using (BADtlsCustomer BASU = new BADtlsCustomer())
            {
                dt = BASU.Get("TYPE_4", 0, to_username, 1, ref errMsg, "2021", 1);
                if(dt.Rows.Count>0)
                {
                    oBMST.CUSTOMER_NAME = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                    oBMST.CUSTOMER_EMAIL = dt.Rows[0]["CUSTOMER_EMAIL"].ToString();
                }

                Session["custmail"] = oBMST.CUSTOMER_EMAIL;
            }
            //String txt_User_Name = oBMST.CUSTOMER_FNAME + " " + oBMST.CUSTOMER_LNAME;
            


            if (SendEmailVerification(form, form["useremail"]))
            {
                return Redirect("/ResetPasswordUI/Index");

            }
            else
            {
                TempData["JavaScriptMsg"] = string.Format("popupvalidation();");
                return Redirect("/ForgotPasswordUI/Index");

            }
            
        }

        private bool SendEmailVerification(FormCollection form, String user_email)
        {
            String errMsg = String.Empty;
            DataTable dt = new DataTable();
            EntityDtlsCustomer oBMAST = new EntityDtlsCustomer();
            bool rt = false;

            using (BADtlsCustomer oBMC = new BADtlsCustomer())
            {
                if (form["useremail"] != oBMAST.CUSTOMER_EMAIL)
                {
                    dt = oBMC.Get<Int16>("TYPE_4", 0, form["useremail"], 0, ref errMsg, null, null);
                    if (dt.Rows.Count > 0)
                    {
                        rt = true;
                    }
                }
            }
            return rt;
        }
    }
}