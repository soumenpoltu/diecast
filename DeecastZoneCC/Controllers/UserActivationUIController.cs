using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class UserActivationUIController : Controller
    {
        // GET: UserActivationUI
        Byte vRef = 0; Int32 vKey = 0;
        String errMsg = String.Empty;
        String vString2 = String.Empty;
        String vString3 = String.Empty;
        DataTable dt = null;
        DataTable dt2 = null;
        DataTable dt1 = null;
        DataTable dt10 = null;
        EntityDtlsCustomer oBMAST = null;

        public ActionResult Index()
        {
            return View();
        }

        #region Sitesetting
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
        public ActionResult btn_verify_Click(FormCollection form)
        {
            oBMAST = new EntityDtlsCustomer();
            oBMAST.CUSTOMER_KEY = Convert.ToInt32(Session["custkey"]);
            //oBMAST.USER_EMAIL = hf_USER_EMAIL.Value;

            oBMAST.ACTIVATION_CODE = Convert.ToString(Session["OTP"]);
            oBMAST.CUSTOMER_EMAIL = Session["mailid"].ToString();

            //using (BADtlsCustomer BASU = new BADtlsCustomer())
            //{
            //    dt2 = BASU.Get("CHECK_OTP", oBMAST.CUSTOMER_KEY, "", ref errMsg, "", 1);
            //}
            //oBMAST.ACTIVATION_CODE = Convert.ToString(dt2.Rows[0]["ACTIVATION_CODE"]);
            if (oBMAST.ACTIVATION_CODE == form["txt_otp"])
            {
                using (BADtlsCustomer oBMC = new BADtlsCustomer())
                {
                    vRef = oBMC.SaveChanges<Object, Int32>("UPDATE_TAG", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                }
                if (vRef == 1)
                {
                    return Redirect("/ConfirmRegistrationUI/Index");
                }

            }
            else
            {
                TempData["JavaScriptMsg"] = string.Format("popupinvalidcode();");
                form["txt_otp"] = "";
            }
            return Redirect("/UserActivationUI/Index");
        }

        [HttpPost]
        public ActionResult btn_resend_Click(FormCollection form)
        {
            oBMAST = new EntityDtlsCustomer();
            oBMAST.CUSTOMER_KEY = Convert.ToInt32(Session["custkey"]);
            //oBMAST.USER_EMAIL = hf_USER_EMAIL.Value;

            oBMAST.ACTIVATION_CODE = Convert.ToString(Session["OTP"]);


            using (BADtlsCustomer BASU = new BADtlsCustomer())
            {
                dt2 = BASU.Get("CHECK_OTP", oBMAST.CUSTOMER_KEY, "", ref errMsg, "", 1);
            }
            oBMAST.CUSTOMER_EMAIL = Convert.ToString(dt2.Rows[0]["CUSTOMER_EMAIL"]);
            Session["mail"] = oBMAST.CUSTOMER_EMAIL;
            if (oBMAST.ACTIVATION_CODE != form["txt_otp"] || form["txt_otp"] == "")
            {
                sendmail(oBMAST.CUSTOMER_EMAIL, oBMAST.ACTIVATION_CODE);
                form["txt_otp"] = "";
            }
            return Redirect("/UserActivationUI/Index");
        }

        private String sendmail(String to_username, string code)
        {
            EntityDtlsCustomer oBMST = new EntityDtlsCustomer();
            to_username = Convert.ToString(Session["mail"]);
            String EmailAddress = Encrypted.Encryptdata(to_username);
            try
            {
                // string to_username = ConfigurationManager.AppSettings["to_username"].ToString();
                string form_username = ConfigurationManager.AppSettings["form_username"].ToString();
                string form_password = ConfigurationManager.AppSettings["form_password"].ToString();
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true; //false

                using (MailMessage mail = new MailMessage())
                {
                    string strBody = string.Empty;
                    strBody += "<html><head></head><body>";
                    strBody += Environment.NewLine;
                    strBody += "Your Account Verification OTP is " + Session["OTP"];
                    strBody += Environment.NewLine;
                    //strBody += "<a href='" + Request.Url.AbsoluteUri.Replace("/LoginUI/Index", "/UserActivationUI/Index?ActivationCode=" + code.ToString()) + "'>Click the following link to activate your account.</a>";
                    strBody += "<br/>Thanks.</body></html>";

                    mail.From = new MailAddress(form_username);
                    mail.To.Add(to_username);
                    mail.Subject = "User Activation Mail";
                    mail.Body = strBody.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(form_username, Encrypted.Decryptdata(form_password));
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                    TempData["JavaScriptMsg"] = string.Format("popup();");
                    //ClientScript.RegisterStartupScript(GetType(), "popup", "popup();", true);
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("popupservererror();");
                //ClientScript.RegisterStartupScript(GetType(), "popupservererror", "popupservererror(); console.log('" + ex.Message + "');", true);
                return "fail";
            }
        }
    }
}