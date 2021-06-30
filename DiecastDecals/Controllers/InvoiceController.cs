using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using HtmlAgilityPack;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using IronPdf;

namespace DiecastDecals.Controllers
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



        public ActionResult InvoiceHistroy()
        {
            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {
                layoutpopulate();
            getinvoice();
            return View(ds.Tables[0].Rows);
        }
            else
            {
                return Redirect("~/LoginUI/Index");
    }

}

        private void getinvoice()
        {
            ds = new DataSet();
            HttpCookie objCookie = Request.Cookies["__asd"];

            using (BAInvoice oBMC = new BAInvoice())
            {
                ds = oBMC.Get<Int32>("GET_ALL_INVOICE", 0, Encrypted.Decryptdata(objCookie["a"].ToString()).ToString(), ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            
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
           if(ds.Tables[0] != null)
           {
                ViewBag.billaddr = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                ViewBag.inv = ds.Tables[0].Rows[0]["DTLS_INVOICE"].ToString();
                ViewBag.invdate = ds.Tables[0].Rows[0]["INV_DATE"].ToString();
           }
            if (ds.Tables[1] != null)
            {
                ViewBag.shipaddr = ds.Tables[1].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_COUNTRY"].ToString();
          
            }
            dt = ds.Tables[2];
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewBag.tax = dt.Rows[0]["TAX"];
                ViewBag.totalamnt = dt.Rows[0]["TOTAL_AMOUNT"];
                ViewBag.subtotal = dt.Rows[0]["SUB_TOTAL"];
                ViewBag.shipping = dt.Rows[0]["SHIPPING_AMOUNT"];
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
                ViewBag.billaddr = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                ViewBag.inv = ds.Tables[0].Rows[0]["DTLS_INVOICE"].ToString();
                ViewBag.invdate = ds.Tables[0].Rows[0]["INV_DATE"].ToString();
            }
            if (ds.Tables[1] != null)
            {
                ViewBag.shipaddr = ds.Tables[1].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_COUNTRY"].ToString();

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

        [HttpPost]
        public void Export(string GridHtml)
        {
            var htmlToPdf = new HtmlToPdf();
            var html = @"" + GridHtml + "";

            var pdf = htmlToPdf.RenderHtmlAsPdf(html);
            var OutputPath = "HtmlToPDF.pdf";
            pdf.SaveAs(OutputPath);
            System.Diagnostics.Process.Start(OutputPath);

            //HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["b"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
            //HtmlDocument doc = new HtmlDocument();
            //doc.OptionFixNestedTags = true;
            //doc.LoadHtml(GridHtml);
            //GridHtml = doc.DocumentNode.OuterHtml;

            //using (MemoryStream stream = new System.IO.MemoryStream())
            //{
            //    Encoding unicode = Encoding.UTF8;

            //    StringReader sr = new StringReader(GridHtml);
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

            //    pdfDoc.Open();
            //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //    pdfDoc.Close();
            //    return File(stream.ToArray(), "application/pdf", "OrderStatus.pdf");
            //}
        }


        protected void send(string recepientEmail,string firstname, string lastname, string phone, string address, string country, string city, string productname)
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
                    mail.From = new MailAddress(form_username, "Diecast Decals");
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
                    mail1.From = new MailAddress(form_username, "Diecast Decals");
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