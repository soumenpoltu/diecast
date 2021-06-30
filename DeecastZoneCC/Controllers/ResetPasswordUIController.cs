using MyApp.db.Encryption;
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
    public class ResetPasswordUIController : Controller
    {
        // GET: ResetPasswordUI
        DataTable dt;
        DataTable dt1;
        String errMsg = String.Empty;
        String vString = String.Empty, vString1 = String.Empty, vString2 = String.Empty, vString3 = String.Empty, vString4 = String.Empty;
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult btn_Resetpass_click(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            EntityDtlsCustomer oBMAST = null;
            DataTable dt = new DataTable();
            EntityDtlsCustomer oSysUser = new EntityDtlsCustomer();
            try
            {
                if (ModelState.IsValid)
                {
                    errMsg = String.Empty;
                    oBMAST = new EntityDtlsCustomer();
                    if (Session["custmail"] != null)
                    {
                        ViewBag.hf_CUSTOMER_EMAIL = Convert.ToString(Session["custmail"]);
                    }
                    using (BADtlsCustomer BS = new BADtlsCustomer())
                    {
                        dt = BS.Get("TYPE_4", 0, ViewBag.hf_CUSTOMER_EMAIL, 1, ref errMsg, "2021", 1);
                    }
                    oBMAST.CUSTOMER_KEY = Convert.ToInt32(dt.Rows[0]["CUSTOMER_KEY"]);
                    //oBMAST.USER_KEY = Convert.ToInt32(hf_USER_KEY.Value);
                    oBMAST.CUSTOMER_EMAIL = ViewBag.hf_CUSTOMER_EMAIL;
                    oBMAST.CUSTOMER_PASSWORD = Encrypted.Encryptdata(form["npassword"]);
                    oBMAST.CUSTOMER_CPASSWORD = Encrypted.Encryptdata(form["cpassword"]);

                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BADtlsCustomer oBMC = new BADtlsCustomer())
                    {
                        if (form["npassword"] != null && form["cpassword"] == form["npassword"])
                        {
                            vRef = oBMC.SaveRegister<Object, Int32>("UPDATE_PASS", oBMAST, null, ref vKey, ref errMsg, "", 1);
                        }

                        if (vRef == 1)
                        {
                            ClearControl(form);
                            TempData["JavaScriptMsg"] = string.Format("popup();");
                            //ClientScript.RegisterStartupScript(GetType(), "popup", "popup();", true);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("popuperror();");
                //ClientScript.RegisterStartupScript(GetType(), "popupservererror", "popupservererror(); console.log('" + ex.Message + "');", true);

            }
            return Redirect("/ResetPasswordUI/Index");
        }

        private void ClearControl(FormCollection form)
        {
            form["npassword"] = "";
            form["cpassword"] = "";
        }
    }
}