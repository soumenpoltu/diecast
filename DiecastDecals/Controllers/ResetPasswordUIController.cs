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
    public class ResetPasswordUIController : CommonController
    {
        // GET: ResetPasswordUI
        DataTable dt;
        DataTable dt1;
        String errMsg = String.Empty;
        String vString = String.Empty, vString1 = String.Empty, vString2 = String.Empty, vString3 = String.Empty, vString4 = String.Empty;
        public ActionResult Index()
        {
            layoutpopulate();
            return View();
        }


        #region Site Setting
        private String FillSiteSetting()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAHeadSiteSetting oBME = new BAHeadSiteSetting())
                {
                    dt = oBME.GetData("ALL", "", ref errMsg, null, 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.CONTACT_NO = Convert.ToString(dt.Rows[0]["CONTACT_NO"]);
                    ViewBag.MAIL = Convert.ToString(dt.Rows[0]["MAIL"]);
                    ViewBag.ADDRESS = Convert.ToString(dt.Rows[0]["ADDRESS"]);
                    ViewBag.HEADER_LOGO = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[0]["HEADER_LOGO"]);
                    ViewBag.FACEBOOK_LINK = Convert.ToString(dt.Rows[0]["FACEBOOK_LINK"]);
                    ViewBag.TWITTER_LINK = Convert.ToString(dt.Rows[0]["TWITTER_LINK"]);
                    ViewBag.INSTAGRAM_LINK = Convert.ToString(dt.Rows[0]["INSTAGRAM_LINK"]);
                    ViewBag.PRINTEREST_LINK = Convert.ToString(dt.Rows[0]["PRINTEREST_LINK"]);


                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion

        [HttpPost]
        public ActionResult btn_submit_Click(FormCollection form)
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
                    oBMAST.CUSTOMER_PASSWORD = Encrypted.Encryptdata(form["txt_pass"]);
                    oBMAST.CUSTOMER_CPASSWORD = Encrypted.Encryptdata(form["txt_cPass"]);

                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BADtlsCustomer oBMC = new BADtlsCustomer())
                    {
                        if (form["txt_pass"] != null && form["txt_cPass"] == form["txt_pass"])
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("UPDATE_PASS", oBMAST, null, ref vKey, ref errMsg,"",1);
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
                TempData["JavaScriptMsg"] = string.Format("popupservererror();");
                //ClientScript.RegisterStartupScript(GetType(), "popupservererror", "popupservererror(); console.log('" + ex.Message + "');", true);

            }
            return Redirect("/ResetPasswordUI/Index");
        }

        private void ClearControl(FormCollection form)
        {
            form["txt_pass"] = "";
            form["txt_cPass"] = "";
        }
    }
}