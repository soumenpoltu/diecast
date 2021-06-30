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
using IronPdf;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace DiecastDecals.Controllers
{
    public class OrderHistoryUIController : CommonController
    {
        // GET: OrderHistoryUI

        DataTable dt;
        DataTable dt1;
        DataSet ds;
        String errMsg = String.Empty;
        String vString = String.Empty;
        public ActionResult Index()
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

        public ActionResult ViewInvoice(string invoice = null)
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
                    return View("Index");
                }
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
                ds = oBMC.Get<Int32>("INVOICE_HISTROY", 0, Encrypted.Decryptdata(objCookie["a"].ToString()).ToString(), ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

            }


        }


        private void getcart(string invoice)
        {
            ds = new DataSet();
            String productname = string.Empty;
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

            }

        }
    }
}