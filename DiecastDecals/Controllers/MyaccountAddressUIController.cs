using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MyApp.Entity;

namespace DiecastDecals.Controllers
{
    public class MyaccountAddressUIController : CommonController
    {
        // GET: MyaccountAddressUI
        DataSet ds;
        String errMsg = String.Empty;
        String Vstring = String.Empty;
        public ActionResult Index()
        {
            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {

                getbillingaddr(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                layoutpopulate();

                return View();
            }
            else
            {
                return Redirect("~/LoginUI/Index");
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

                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                if (ds.Tables[0].Rows.Count > 0)
                {
                 for(int i=0;i< ds.Tables[0].Rows.Count; i++)
                    {
                        Vstring += "<div class='col-md-6'><div class='addritem'>";
                        Vstring += "<h5>"+ ds.Tables[0].Rows[i]["CUSTOMER_FNAME"].ToString() +" "+ " " + "  "+ ds.Tables[0].Rows[i]["CUSTOMER_LNAME"].ToString() + "</h5><p>"+ ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_COUNTRY"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_PINCODE"].ToString() + "</p>";
                        Vstring += "<p>" + ds.Tables[0].Rows[i]["CUSTOMER_EMAIL"].ToString() + "</p><p>" + ds.Tables[0].Rows[i]["CUSTOMER_PHONE"].ToString() + "</p><a href='" + Domain + "/MyaccountAddressAddUI?q="+ Encrypted.EncryptASCII(ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString()) +"' class='checkPageBtn'>Edit</a>";
                        if (ds.Tables[0].Rows[i]["TAG_ACTIVE"].ToString() == "1")
                        {
                            Vstring += "<a class='checkPageBtn green'>Default Address</a>";
                        }
                        else
                        {
                            Vstring += "<a data-d='"+ ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString() + "' id='def"+ i + "' onclick='defaultaddress(this.id)' class='checkPageBtn'>Use it as default</a>";
                        }
                     
                        
                        Vstring += "</div></div>";
                     
                    }
                }

                ViewBag.address = Vstring;

            }






         }

        [HttpPost]
        public JsonResult updatedefault(string addresskey)
        {
            Byte vRef = 0; Int32 vKey = 0;


            HttpCookie objCookie = Request.Cookies["__asd"];
            EntityDtlsCustomer oBMAST = null;

            errMsg = String.Empty;
            oBMAST = new EntityDtlsCustomer();
            oBMAST.CUSTOMER_ADRESS_KEY = Convert.ToInt32( addresskey);
            oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
            oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
            oBMAST.TAG_ACTIVE = 1;
            oBMAST.TAG_DELETE = 0;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");


            using (BAAddress oBMC = new BAAddress())
            {
                vRef = oBMC.SaveChanges("UPDATE_DEFAULT", oBMAST, ref errMsg, "2019", 1);
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

            Redirect("~/MyAccountAddressUI/Index");


            return Json(1);


        }


    }
}