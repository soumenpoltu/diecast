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

namespace DiecastDecals.Controllers
{
    public class ContactUsUIController : CommonController
    {
        // GET: ContactUsUI

        DataTable dt;
        String errMsg = String.Empty;
        String vString = String.Empty, vString1 = String.Empty, vString2 = String.Empty, vString3 = String.Empty, vString4 = String.Empty;
        public ActionResult Index()
        {
            layoutpopulate();
            return View();
        }



        [HttpPost]
        public ActionResult btn_Contact_click(FormCollection form)
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
                    if (form["txt_Name"] != "" && form["txt_Email"] != "" && form["txtMessage"] != "")
                    {
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/main-assets/contactmail.html")))
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("FULLNAME", form["txt_Name"].ToString());
                            body = body.Replace("EMAILID", form["txt_Email"]);
                            body = body.Replace("PHONE", "N/A");
                            body = body.Replace("MESSAGE", form["txtMessage"]);
                        }

                        mail.From = new MailAddress(form_username,"Diecast Decals");
                        mail.To.Add(to_username);
                        mail.Subject = "New appointment query from " + form["txt_Name"] + " for Diecast Decals";
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
                        body = body.Replace("FULLNAME", form["txt_Name"].ToString());

                    }
                    mail.From = new MailAddress(form_username,"Diecast Decals");
                    mail.To.Add(form["txt_Email"]);
                    mail.Subject = "Thank you for contacting on Diecast Decals";
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
                TempData["JavaScriptFunction"] = string.Format("popup();");
            }
            catch (Exception ex)
            {               
                TempData["JavaScriptFunction"] = string.Format("popupservererror();");
            }
           
            return Redirect("~/ContactUsUI/Index");
        }
    }
}