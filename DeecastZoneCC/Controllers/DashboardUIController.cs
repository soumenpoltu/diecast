using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.db.Encryption;
using MyApp.Entity;
using System.Configuration;

namespace DeecastZoneCC.Controllers
{
    public class DashboardUIController : CommonController
    {
        // GET: DashboardUI
        DataSet ds;
        DataTable dt;
        DataTable dt1;
        String errMsg = String.Empty;
        public ActionResult Index()
        {
            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {
                getcart( Encrypted.Decryptdata(objCookie["a"]));
                getinvoicehistroy(Encrypted.Decryptdata(objCookie["a"]));
                getAddress(Encrypted.Decryptdata(objCookie["a"]));
                GetUser(Encrypted.Decryptdata(objCookie["a"]));
                FillAboutUsEdit();
                layoutpopulate();
                return View();
            }
            else
            {
                return Redirect("~/LoginUI/Index");
            }
        }


        private void getcart(string email)
        {
            ds = new DataSet();
            String productname = string.Empty;
            using (BAInvoice oBMC = new BAInvoice())
            {
                ds = oBMC.Get<Int32>("GET_DASH", 0, email, ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (ds.Tables[0] != null)
            {
                ViewBag.totalinv = ds.Tables[0].Rows[0]["TOTAL"].ToString();
            }

            if (ds.Tables[1] != null)
            {
                ViewBag.proinv = ds.Tables[1].Rows[0]["NOT_DELIVERED"].ToString();

            }

            if (ds.Tables[2] != null)
            {

                ViewBag.cominv = ds.Tables[2].Rows[0]["DELIVERED"].ToString();
            }


        }
        private void getinvoicehistroy(string email)
        {
            ds = new DataSet();
            String productname = string.Empty;
            using (BAInvoice oBMC = new BAInvoice())
            {
                ds = oBMC.Get<Int32>("GET_ALL_INVOICE", 0, email, ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (ds.Tables[0] != null)
            {
                ViewData["dt"] = ds.Tables[0];
            }

          

        }

        private void getAddress(string email)
        {
            using (BAAddress oBMC = new BAAddress())
            {
                ds = oBMC.GetData("GET_ADDR", email, ref errMsg, "2019", 1);
               // System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count>0)
                {
                    ViewBag.shippingaddress = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString();
                    ViewBag.shippingcity = ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString();
                    ViewBag.shippingcountry = ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                    ViewBag.shippingphone = ds.Tables[0].Rows[0]["CUSTOMER_PHONE"].ToString();
                    ViewBag.shippingpin = ds.Tables[0].Rows[0]["CUSTOMER_PINCODE"].ToString();

                }
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
        public ActionResult SaveAddr(FormCollection form)
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
                vRef = oBMC.SaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
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

            return Redirect("~/DashboardUI/Index");

        }

        public void GetUser(string email)
        {
             dt = new DataTable();
                using (BADtlsCustomer oBSU = new BADtlsCustomer())
                {
                    dt = oBSU.Get<Int16>("TYPE_4", 0, email, 1, ref errMsg, null, 1);
                }
                if(dt != null)
            {
                ViewBag.FirstName = dt.Rows[0]["CUSTOMER_FNAME"];
                ViewBag.LastName = dt.Rows[0]["CUSTOMER_LNAME"];
                ViewBag.Email = dt.Rows[0]["CUSTOMER_EMAIL"];
            }
        }


        [HttpPost]
        public ActionResult SaveUser(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            EntityDtlsCustomer oBMAST = null;
            HttpCookie objCookie = Request.Cookies["__asd"];

            errMsg = String.Empty;
            oBMAST = new EntityDtlsCustomer();
            oBMAST.CUSTOMER_KEY = Convert.ToInt32(ViewBag.hf_CUSTOMER_KEY);
            oBMAST.CUSTOMER_FNAME = form["txt_FirstName"];
            oBMAST.CUSTOMER_LNAME = form["txt_LastName"];
         
            oBMAST.CUSTOMER_EMAIL = form["txt_Email"];

            oBMAST.CUSTOMER_CURRENTPASSWORD = Encrypted.Encryptdata(form["txt_cPassword"]);
            oBMAST.CUSTOMER_PASSWORD = Encrypted.Encryptdata(form["txt_nPassword"]);
            oBMAST.CUSTOMER_CPASSWORD = Encrypted.Encryptdata(form["txt_cnPassword"]);

            oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
            oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);

          

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

            using (BADtlsCustomer oBMC = new BADtlsCustomer())
            {

                if(oBMAST.CUSTOMER_CURRENTPASSWORD != "")
                {
                    if (oBMAST.CUSTOMER_CURRENTPASSWORD == Encrypted.DecryptASCII(objCookie["p"]))
                    {
                        if (form["txt_nPassword"] == form["txt_cnPassword"])
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("UPDATE_USER", oBMAST, null, ref vKey, ref errMsg, "", 1);
                        }
                    }
                   

                }
               else
                {
                    vRef = oBMC.SaveChanges<Object, Int32>("UPDATE_USER", oBMAST, null, ref vKey, ref errMsg, "", 1);
                }
                
            }




                        return Redirect("~/DashboardUI/Index");

        }


        private DataTable FillAboutUsEdit()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt1 = new DataTable();
                using (BAAboutUs oBME = new BAAboutUs())
                {
                    dt1 = oBME.GetData("GET", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.ABOUT_US_DESC = Convert.ToString(dt1.Rows[0]["ABOUT_US_DESC"]);
                    ViewBag.REMARKS = Convert.ToString(dt1.Rows[0]["REMARKS"]);
                    ViewBag.ICON_IMG_1 = ConfigurationManager.AppSettings["ABOUT"].ToString() + Convert.ToString(dt1.Rows[0]["LOGO_1"]);
                    ViewBag.ICON_IMG_2 = ConfigurationManager.AppSettings["ABOUT"].ToString() + Convert.ToString(dt1.Rows[0]["LOGO_2"]);
                    ViewBag.ICON_IMG_3 = ConfigurationManager.AppSettings["ABOUT"].ToString() + Convert.ToString(dt1.Rows[0]["LOGO_3"]);
                }
                return dt1;
            }
            catch (Exception ex)
            {
                return dt1;
            }

        }

    }
}