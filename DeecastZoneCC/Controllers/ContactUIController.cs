using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Web.Services.Description;
using MyApp.Entity.Message;
using System.Text;
using System.Web.UI;
using System.IO;

namespace DeecastZoneCC.Controllers
{
    public class ContactUIController : Controller
    {
        // GET: ContactUI
        public ActionResult Index()
        {
            
            return View();
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
            return Redirect("~/ContactUI/Index");
        }
    }
}