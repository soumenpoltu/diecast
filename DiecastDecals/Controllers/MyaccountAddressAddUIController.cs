using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace DiecastDecals.Controllers
{
    public class MyaccountAddressAddUIController : CommonController
    {
        String errMsg = String.Empty;
        DataSet ds;
        // GET: MyaccountAddressAddUI

        public ActionResult Index(string q= null)
        {
            if (q != null)
            {
                getbillingaddr(Encrypted.DecryptASCII(q));
                layoutpopulate();
                return View();
            }
            else
            {
                ViewBag.CUSTOMER_ADDRESS_KEY = 0;
                layoutpopulate();
                return View();
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
            oBMAST.CUSTOMER_ADRESS_KEY = Convert.ToInt32( form["hf_address_key"]);
            oBMAST.CUSTOMER_FNAME = form["txt_firstname"];
            oBMAST.CUSTOMER_LNAME = form["txt_lastname"];
            oBMAST.CUSTOMER_ADDRESS = form["txt_address"];
            oBMAST.CUSTOMER_CITY = form["txt_city"];
            oBMAST.CUSTOMER_COUNTRY = form["txt_country"];
            oBMAST.CUSTOMER_PHONE = form["txt_phone"];
            oBMAST.CUSTOMER_PINCODE = form["txt_pin"];
            oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);

            oBMAST.ADDRESS_CUSTOMER_EMAIL = form["txt_email"];

            oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
            oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
            oBMAST.TAG_ACTIVE = 1;
            oBMAST.TAG_DELETE = 0;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");


            using (BAAddress oBMC = new BAAddress())
            {
                if (oBMAST.CUSTOMER_ADRESS_KEY==0)
                {
                    vRef = oBMC.SaveChanges("INSERT", oBMAST, ref errMsg, "2019", 1);
                }
                else
                {
                    vRef = oBMC.SaveChanges("UPDATE", oBMAST, ref errMsg, "2019", 1);
                }
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

            return Redirect("~/MyAccountAddressUI/Index");

        }


        private void getbillingaddr(string adresskey)
        {
            ds = new DataSet();
            using (BAAddress oBMC = new BAAddress())
            {
                ds = oBMC.GetData("GET_DTLS", adresskey, ref errMsg, "2019", 1);

            }

            if (ds != null)
            {


                if (ds.Tables[0].Rows.Count > 0)
                {

                    ViewBag.CUSTOMER_FNAME = ds.Tables[0].Rows[0]["CUSTOMER_FNAME"].ToString();
                    ViewBag.CUSTOMER_LNAME = ds.Tables[0].Rows[0]["CUSTOMER_LNAME"].ToString();
                    ViewBag.CUSTOMER_ADDRESS = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString();
                    ViewBag.CUSTOMER_CITY = ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString();
                    ViewBag.CUSTOMER_COUNTRY = ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                    ViewBag.CUSTOMER_PINCODE = ds.Tables[0].Rows[0]["CUSTOMER_PINCODE"].ToString();
                    ViewBag.CUSTOMER_EMAIL = ds.Tables[0].Rows[0]["CUSTOMER_EMAIL"].ToString();
                    ViewBag.CUSTOMER_PHONE = ds.Tables[0].Rows[0]["CUSTOMER_PHONE"].ToString();
                    ViewBag.CUSTOMER_ADDRESS_KEY = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS_KEY"].ToString();

                }

            }
        }


        }
}