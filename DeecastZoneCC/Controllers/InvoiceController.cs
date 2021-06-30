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
    public class InvoiceController : CommonController
    {
        // GET: Invoice

        DataTable dt;
        DataTable dt1;
        DataSet ds;
        String errMsg = String.Empty;
        String vString = String.Empty;

        public ActionResult Index(string invoice = null)
        {
            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {


                if (invoice != null)
                {

                    layoutpopulate();
                    getcart(invoice);
                    return View(dt.Rows);

                }
                else
                {


                    layoutpopulate();
                    getcart();

                    return View(dt.Rows);
                }
            }
            else
            {
                return Redirect("~/LoginUI/Index");
            }

        }

        private void getcart()
        {
            ds = new DataSet();
            String productname = string.Empty;
            using (BAInvoice oBMC = new BAInvoice())
            {
                ds = oBMC.Get<Int32>("GET_INVOICE", 0, Session["InvoiceID"].ToString(), ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (ds.Tables[0] != null)
            {
                ViewBag.addr = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                ViewBag.inv = ds.Tables[0].Rows[0]["DTLS_INVOICE"].ToString();
                ViewBag.invdate = ds.Tables[0].Rows[0]["INV_DATE"].ToString();
            }

            if (ds.Tables[1] != null)
            {
                ViewBag.biladdr = ds.Tables[1].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_COUNTRY"].ToString();

            }

            dt = ds.Tables[2];
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewBag.tax = dt.Rows[0]["TAX"];
                ViewBag.totalamnt = dt.Rows[0]["TOTAL_AMOUNT"];
                ViewBag.subtotal = dt.Rows[0]["SUB_TOTAL"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        productname += dt.Rows[i]["PRODUCT"].ToString();
                    }
                    else
                    {
                        productname += "," + dt.Rows[i]["PRODUCT"].ToString();
                    }

                }


                send(ds.Tables[0].Rows[0]["CUSTOMER_EMAIL"].ToString(), ds.Tables[0].Rows[0]["CUSTOMER_FNAME"].ToString(), ds.Tables[0].Rows[0]["CUSTOMER_LNAME"].ToString(), ds.Tables[0].Rows[0]["CUSTOMER_PHONE"].ToString(), ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString(), ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString(), ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString(), productname);

            }


        }


        private void getcart(string invoice)
        {
            ds = new DataSet();

            using (BAInvoice oBMC = new BAInvoice())
            {
                ds = oBMC.Get<Int32>("GET_INVOICE", 0, invoice.ToString(), ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (ds.Tables[0] != null)
            {
                ViewBag.addr = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                ViewBag.inv = ds.Tables[0].Rows[0]["DTLS_INVOICE"].ToString();
                ViewBag.invdate = ds.Tables[0].Rows[0]["INV_DATE"].ToString();
            }

            if (ds.Tables[1] != null)
            {
                ViewBag.biladdr = ds.Tables[1].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_COUNTRY"].ToString();
              
            }

            dt = ds.Tables[2];
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewBag.tax = dt.Rows[0]["TAX"];
                ViewBag.totalamnt = dt.Rows[0]["TOTAL_AMOUNT"];
                ViewBag.subtotal = dt.Rows[0]["SUB_TOTAL"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {


                }


            }

        }



        protected void send(string recepientEmail, string firstname, string lastname, string phone, string address, string country, string city, string productname)
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
                    string template = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/main-assets/mailtemplate.html")))
                    {
                        template = reader.ReadToEnd();
                        template = template.Replace("FULLNAME", firstname + " " + lastname);
                        template = template.Replace("EMAILID", recepientEmail);
                        template = template.Replace("MOBILENO", phone);
                        //template = template.Replace("COMNAME", Session["COMPANY_NAME"].ToString());
                        template = template.Replace("COMADDRESS", address);
                        template = template.Replace("COUNTRY", country);
                        // template = template.Replace("STATE", "");
                        template = template.Replace("CITY", city);
                        //template = template.Replace("ZIP", "");
                        template = template.Replace("PRODUCTNAME", productname);
                    }
                    mail.From = new MailAddress(form_username);
                    mail.To.Add(to_username);
                    mail.Subject = "New Product Purchase";
                    mail.Body = template.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(form_username, Encrypted.Decryptdata(form_password));
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                using (MailMessage mail1 = new MailMessage())
                {
                    string templatebody = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/main-assets/regmailtemplate.html")))
                    {
                        templatebody = reader.ReadToEnd();
                        templatebody = templatebody.Replace("FULLNAME", firstname + " " + lastname);
                        templatebody = templatebody.Replace("PRICINGPLAN", productname);

                    }
                    mail1.From = new MailAddress(form_username);
                    mail1.To.Add(recepientEmail);
                    mail1.Subject = "Thank you for shopping on Diecast Decals";
                    mail1.Body = templatebody.ToString();
                    mail1.IsBodyHtml = true;

                    using (SmtpClient smtp1 = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp1.UseDefaultCredentials = false;
                        smtp1.Credentials = new NetworkCredential(form_username, Encrypted.Decryptdata(form_password));
                        smtp1.EnableSsl = enableSSL;
                        smtp1.Send(mail1);
                    }
                }
            }
            catch (Exception ex)
            {
                // ClientScript.RegisterStartupScript(GetType(), "popupservererror", "popupservererror(); console.log('" + ex.Message + "');", true);
            }
            TempData["JavaScriptFunction"] = string.Format("popup();");
            //return Redirect("~/Register/Index");
        }




    }
}