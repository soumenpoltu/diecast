using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System.Configuration;

namespace DeecastZoneCC.Controllers
{
    public class CheckOutUIController : CommonController
    {
        // GET: CheckOutUI
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



                getcart(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                getbillingaddr(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                layoutpopulate();
                return View(dt.Rows);
            }
            else
            {
                return Redirect("~/LoginUI/Index");
            }
        }


        private void getcart(string useremail)
        {
            dt = new DataTable();

            using (BACart oBMC = new BACart())
            {
                dt = oBMC.Get<Int32>("GET_INVOICE_CART", 0, useremail, ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (dt != null && dt.Rows.Count > 0)
            {

                ViewBag.subtotal = dt.Rows[0]["SUB_TOTAL"];
                ViewBag.tax = dt.Rows[0]["TAX"];
                ViewBag.total = dt.Rows[0]["TOTAL_AMOUNT"];
                
            }


        }


        private void getbillingaddr(string email)
        {
            ds = new DataSet();
            using (BAAddress oBMC = new BAAddress())
            {
                ds = oBMC.GetData("GET_ADDR", email, ref errMsg, "2019", 1);
              
            }

            if (ds != null)
            {

                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewBag.billingaddress = ds.Tables[1].Rows[0]["CUSTOMER_ADDRESS"].ToString();
                    ViewBag.billingcity = ds.Tables[1].Rows[0]["CUSTOMER_CITY"].ToString();
                    ViewBag.billingcountry = ds.Tables[1].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                    ViewBag.billingphone = ds.Tables[1].Rows[0]["CUSTOMER_PHONE"].ToString();
                    ViewBag.billingpin = ds.Tables[1].Rows[0]["CUSTOMER_PINCODE"].ToString();
                }

            }

        }

        [HttpPost]
        public ActionResult SaveAddress(FormCollection form)
        {

            Byte vRef = 0; Int32 vKey = 0;


            HttpCookie objCookie = Request.Cookies["__asd"];
            EntityDtlsCustomer oBMAST = null;

            errMsg = String.Empty;
            oBMAST = new EntityDtlsCustomer();

            oBMAST.CUSTOMER_ADDRESS = form["txt_shippingAddress"];
            oBMAST.CUSTOMER_CITY = form["txt_shippingCity"];
            oBMAST.CUSTOMER_COUNTRY = form["txt_shippingCountry"];
            oBMAST.CUSTOMER_PHONE = form["txt_shippingPhone"];
            oBMAST.CUSTOMER_PINCODE = form["txt_shippingPin"];


            oBMAST.BILLING_CUSTOMER_ADDRESS = form["txt_Address"];
            oBMAST.BILLING_CUSTOMER_CITY = form["txt_City"];
            oBMAST.BILLING_CUSTOMER_COUNTRY = form["txt_Country"];
            oBMAST.BILLING_CUSTOMER_PHONE = form["txt_Phone"];
            oBMAST.BILLING_CUSTOMER_PINCODE = form["txt_Pin"];
            oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);

            oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
            oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
            oBMAST.TAG_ACTIVE = 1;
            oBMAST.TAG_DELETE = 0;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");


            using (BAAddress oBMC = new BAAddress())
            {
                vRef = oBMC.SaveChanges<Object, Int32>("UPDATE_BILLING", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                if (vRef == 1)
                {


                }
                else if (vRef == 2)
                {
                    // TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                    //  MessageBox(2, Message.msgSaveDuplicate, errMsg);
                }

                else
                {
                    // TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                    //   MessageBox(2, Message.msgSaveErr, errMsg);
                }

            }

            return Redirect("~/CheckOutUI/Index");

        }

        [HttpPost]

        public ActionResult gotopayment(FormCollection form)
        {
            if (form["optradio"] == "No")
            {
              

                Session["USER_BILLING_ADDR"] = "0";

            }
            else
            {

                Session["USER_BILLING_ADDR"] = "1";
            }

            return Redirect("~/PaymentUI/Index");
        }


    }
}