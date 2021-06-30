using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class ForgotPasswordUIController : CommonController
    {
        // GET: ForgotPasswordUI
        DataTable dt;
        DataTable dt1;
        String errMsg = String.Empty;
        String vString = String.Empty, vString1 = String.Empty, vString2 = String.Empty, vString3 = String.Empty, vString4 = String.Empty;
        public ActionResult Index()
        {

            layoutpopulate();
          
            return View();
        }


        [HttpPost]
        public ActionResult btn_submit_Click(FormCollection form)
        {
            EntityDtlsCustomer oBMST = new EntityDtlsCustomer();
            String errMsg = String.Empty;
            String to_username = form["txt_mail"];
            //String emailId = txt_User_Email.Text;
            
            DataTable dt = new DataTable();
            using (BADtlsCustomer BASU = new BADtlsCustomer())
            {
                dt = BASU.Get("TYPE_4", 0, to_username, 1, ref errMsg, "2021", 1);
                if(dt != null && dt.Rows.Count > 0)
                {
                    oBMST.CUSTOMER_FNAME = dt.Rows[0]["CUSTOMER_FNAME"].ToString();
                    oBMST.CUSTOMER_LNAME = dt.Rows[0]["CUSTOMER_LNAME"].ToString();
                    oBMST.CUSTOMER_EMAIL = dt.Rows[0]["CUSTOMER_EMAIL"].ToString();
                    String txt_User_Name = oBMST.CUSTOMER_FNAME + " " + oBMST.CUSTOMER_LNAME;
                    Session["custmail"] = oBMST.CUSTOMER_EMAIL;


                    if (SendEmailVerification(form, form["txt_mail"]))
                    {
                        return Redirect("/ResetPasswordUI/Index");
                    }

                    return Redirect("/ResetPasswordUI/Index");
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("popuperror();");
                    return Redirect("/ForgotPasswordUI/Index");
                }

            }
           
        }

        private bool SendEmailVerification(FormCollection form,String user_email)
        {
            String errMsg = String.Empty;
            DataTable dt = new DataTable();
            EntityDtlsCustomer oBMAST = new EntityDtlsCustomer();
            bool rt = false;

            using (BADtlsCustomer oBMC = new BADtlsCustomer())
            {
                if (form["txt_mail"] != oBMAST.CUSTOMER_EMAIL)
                {
                    dt = oBMC.Get<Int16>("TYPE_4", 0, form["txt_mail"], 0, ref errMsg, null, null);
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