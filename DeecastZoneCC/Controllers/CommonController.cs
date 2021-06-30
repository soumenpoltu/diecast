using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class CommonController : Controller
    {

        // GET: Common
        String errMsg = String.Empty;
        DataTable dt1;  //pagesetting
        public string layoutpopulate()
        {
            checklogin();
            FillSiteSettingEdit();
            return "1";
        }

        public void checklogin()
        {

            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {
                ViewBag.customername = Encrypted.DecryptASCII(objCookie["fname"].ToString()).ToString();
                //getcart(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                ViewBag.logindisplay = "style='display:inline-block;'";
            }
            else
            {
                //ViewBag.cartitemscount = "0";
                //ViewBag.cartitemprice = "0";
                //ViewBag.customername = "Guest";
                ViewBag.logindisplay = "style='display:none;'";
            }
        }
        private String FillSiteSettingEdit()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt1 = new DataTable();
                using (BAHeadSiteSetting oBME = new BAHeadSiteSetting())
                {
                    dt1 = oBME.GetHomeSetting("GET", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.CONTACT_NO = Convert.ToString(dt1.Rows[0]["CONTACT_NO"]);
                    ViewBag.MAIL = Convert.ToString(dt1.Rows[0]["MAIL_ID"]);
                    ViewBag.ADDRESS = Convert.ToString(dt1.Rows[0]["OFFICE_ADDRESS"]);

                    ViewBag.FACEBOOK_LINK = Convert.ToString(dt1.Rows[0]["FACEBOOK_LINK"]);
                    ViewBag.TWITTER_LINK = Convert.ToString(dt1.Rows[0]["TWITTER_LINK"]);

                    ViewBag.INSTAGRAM_LINK = Convert.ToString(dt1.Rows[0]["INSTA_LINK"]);

                    ViewBag.FOOTER_NOTE = Convert.ToString(dt1.Rows[0]["FOOTER_NOTE"]);


                    ViewBag.LARGE_VIDEO_LINK = Convert.ToString(dt1.Rows[0]["LARGE_VIDEO_LINK"]);

                    ViewBag.BANNER_VIDEO_LINK = Convert.ToString(dt1.Rows[0]["BANNER_VIDEO_LINK"]);

                    ViewBag.BANNER_DESC = Convert.ToString(dt1.Rows[0]["BANNER_DESC"]);



                    ViewBag.img_WEBSITE_LOGO = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["WEBSITE_LOGO"]);
                  
                    ViewBag.img_BANNER_IMAGE = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["BANNER_IMAGE"]);
                 
                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                //dt1 = null;
            }
        }

        [HttpPost]
        public ActionResult btn_Submit_click(FormCollection form)
        {
            try
            {
                string to_username = ConfigurationManager.AppSettings["to_username"].ToString();
                string form_username = ConfigurationManager.AppSettings["form_username"].ToString();
                string form_password = ConfigurationManager.AppSettings["form_password"].ToString();
                string smtpAddress = "smtp.gmail.com"; //103.21.58.247
                int portNumber = 587;
                bool enableSSL = true; //false

                using (MailMessage mail = new MailMessage())
                {

                    //ValidEmail(txt_mail.Value);
                    if (form["txt_name"] != "" && form["txt_email"] != "" && form["txt_phn"] != "" && form["txt_msg"] != "")
                    {
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/main-assets/contactmail.html")))
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("FULLNAME", form["txt_name"].ToString());
                            body = body.Replace("EMAILID", form["txt_email"]);
                            body = body.Replace("PHONE", form["txt_phn"]);
                            body = body.Replace("MESSAGE", form["txt_msg"]);
                        }

                        mail.From = new MailAddress(form_username);
                        mail.To.Add(to_username);
                        mail.Subject = "New call appointment from " + form["txt_name"] + " for Diecast Zone CC";
                        mail.Body = body.ToString();
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(form_username, Encrypted.Decryptdata(form_password));
                            smtp.EnableSsl = enableSSL;
                            smtp.Send(mail);
                        }
                    }
                }
                using (MailMessage mail = new MailMessage())
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/main-assets/contactreply.html")))
                    {
                        body = reader.ReadToEnd();
                        body = body.Replace("FULLNAME", form["txt_name"].ToString());

                    }
                    mail.From = new MailAddress(form_username);
                    mail.To.Add(form["txt_email"]);
                    mail.Subject = "Thank you for contacting on Diecast Zone CC. Our Team will reach to you soon.";
                    mail.Body = body.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(form_username, Encrypted.Decryptdata(form_password));
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
                TempData["JavaScriptMsg"] = string.Format("popup();");
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("popupservererror();");
                // ClientScript.RegisterStartupScript(GetType(), "popupservererror", "popupservererror(); console.log('" + ex.Message + "');", true);
            }           
            return Redirect("~/IndexUI/Index");
        }

        [HttpGet]
        public ActionResult LogoutIndex()
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