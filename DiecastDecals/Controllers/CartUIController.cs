using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;


using System.Web.Mvc;
using System.Configuration;

namespace DiecastDecals.Controllers
{
    public class CartUIController : CommonController
    {
        // GET: CartUI
        DataTable dt;
        DataTable dt1;
        DataTable dt2;
        DataSet ds;
        String errMsg = String.Empty;
        String vString = String.Empty;
        String Vstring = String.Empty;
        String selectedaddresskey = String.Empty;
        public ActionResult Index()
        {
            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {



                getcart(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                getbillingaddr(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                getallbillingaddr(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                layoutpopulate();
                return View(dt.Rows);
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
                ds = oBMC.GetData("GET_ADDR_DEFAULT", email, ref errMsg, "2019", 1);

            }

            if (ds != null)
            {


                if (ds.Tables[0].Rows.Count > 0)
                {

                    ViewBag.CUSTOMER_BILLING_FNAME = ds.Tables[0].Rows[0]["CUSTOMER_FNAME"].ToString();
                    ViewBag.CUSTOMER_BILLING_LNAME = ds.Tables[0].Rows[0]["CUSTOMER_LNAME"].ToString();
                    ViewBag.CUSTOMER_BILLING_ADDRESS = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString();
                    ViewBag.CUSTOMER_BILLING_CITY = ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString();
                    ViewBag.CUSTOMER_BILLING_COUNTRY = ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                    ViewBag.CUSTOMER_BILLING_PINCODE = ds.Tables[0].Rows[0]["CUSTOMER_PINCODE"].ToString();
                    ViewBag.CUSTOMER_BILLING_EMAIL = ds.Tables[0].Rows[0]["CUSTOMER_EMAIL"].ToString();
                    ViewBag.CUSTOMER_BILLING_PHONE = ds.Tables[0].Rows[0]["CUSTOMER_PHONE"].ToString();
                    ViewBag.CUSTOMER_BILLING_ADDRESS_KEY = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS_KEY"].ToString();


                    ViewBag.CUSTOMER_SHIPPING_FNAME = ds.Tables[0].Rows[0]["CUSTOMER_FNAME"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_LNAME = ds.Tables[0].Rows[0]["CUSTOMER_LNAME"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_ADDRESS = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_CITY = ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_COUNTRY = ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_PINCODE = ds.Tables[0].Rows[0]["CUSTOMER_PINCODE"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_EMAIL = ds.Tables[0].Rows[0]["CUSTOMER_EMAIL"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_PHONE = ds.Tables[0].Rows[0]["CUSTOMER_PHONE"].ToString();
                    ViewBag.CUSTOMER_SHIPPING_ADDRESS_KEY = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS_KEY"].ToString();
                    selectedaddresskey = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS_KEY"].ToString();
                }

            }
        }

        private void getallbillingaddr(string email)
        {
            ds = new DataSet();
            using (BAAddress oBMC = new BAAddress())
            {
                ds = oBMC.GetData("GET_ADDR", email, ref errMsg, "2019", 1);

            }

            if (ds != null)
            {

                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                Session["Address"] = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if(selectedaddresskey == ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString())
                        {
                            Vstring += "<div class='form-check'><input class='form-check-input' checked='checked' type='radio' value='" + ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString() + "' name='billingaddressopt' onclick='selectedbillingaddress(this.id);' id='billingaddressopt" + i + "'><label class='form-check-label' for='billingaddressopt" + i + "'>";

                        }
                        else
                        {
                            Vstring += "<div class='form-check'><input class='form-check-input' type='radio' value='" + ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString() + "' name='billingaddressopt' onclick='selectedbillingaddress(this.id);' id='billingaddressopt" + i + "'><label class='form-check-label' for='billingaddressopt" + i + "'>";

                        }

                        Vstring += "<p>" + ds.Tables[0].Rows[i]["CUSTOMER_FNAME"].ToString() + " " + " " + "  " + ds.Tables[0].Rows[i]["CUSTOMER_LNAME"].ToString() + " " + ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_COUNTRY"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_PINCODE"].ToString() + "</p>";
                        Vstring += "<p>" + ds.Tables[0].Rows[i]["CUSTOMER_EMAIL"].ToString() + "</p><p>" + ds.Tables[0].Rows[i]["CUSTOMER_PHONE"].ToString() + "</p></label>";
                        

                        Vstring += "</div>";

                    }
                }

                ViewBag.billingaddress = Vstring;

                Vstring = String.Empty;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (selectedaddresskey == ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString())
                        {
                            Vstring += "<div class='form-check'><input class='form-check-input' checked='checked' onclick='selectedshippingaddress(this.id);' type='radio' value='" + ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString() + "' name='shippingaddressopt' id='shippinggaddressopt" + i + "'><label class='form-check-label' for='shippinggaddressopt" + i + "'>";

                        }
                        else
                        {
                            Vstring += "<div class='form-check'><input class='form-check-input' type='radio' onclick='selectedshippingaddress(this.id);' value='" + ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS_KEY"].ToString() + "' name='shippingaddressopt' id='shippinggaddressopt" + i + "'><label class='form-check-label' for='shippinggaddressopt" + i + "'>";

                        }

                        Vstring += "<p>" + ds.Tables[0].Rows[i]["CUSTOMER_FNAME"].ToString() + " " + " " + "  " + ds.Tables[0].Rows[i]["CUSTOMER_LNAME"].ToString() + " " + ds.Tables[0].Rows[i]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_COUNTRY"].ToString() + ", " + ds.Tables[0].Rows[i]["CUSTOMER_PINCODE"].ToString() + "</p>";
                        Vstring += "<p>" + ds.Tables[0].Rows[i]["CUSTOMER_EMAIL"].ToString() + "</p><p>" + ds.Tables[0].Rows[i]["CUSTOMER_PHONE"].ToString() + "</p></label>";


                        Vstring += "</div>";

                    }
                }

                ViewBag.shippingaddress = Vstring;

            }






        }

        private void getcart(string useremail)
        {
            dt = new DataTable();
            

            using (BACart oBMC = new BACart())
            {
                dt = oBMC.Get<Int32>("GET_CART", 0, useremail, ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if(dt != null && dt.Rows.Count > 0)
            {
                ViewBag.tax = dt.Rows[0]["TAX"];
                ViewBag.totalamnt = dt.Rows[0]["TOTAL_AMOUNT"];
                ViewBag.subtotal = dt.Rows[0]["SUB_TOTAL"];
                ViewBag.shipping = dt.Rows[0]["SHIPPING_AMOUNT"];
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    if (i == 0)
                    {
                        TempData["Cart_key"] = dt.Rows[i]["DTLS_CART_KEY"].ToString();
                    }
                    else
                    {
                        TempData["Cart_key"] = TempData["Cart_key"] + "," +dt.Rows[i]["DTLS_CART_KEY"].ToString();
                    }
                   
                }
            

            }


        }


        [HttpPost]
        public ActionResult updatecart(FormCollection form)
        {
            String errMsg = String.Empty;
            Decimal tax = 0;
            using (BATax oBME = new BATax())
            {
                dt1 = oBME.GetData("ALL", "", ref errMsg, null, 1);
            }
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                tax = Convert.ToDecimal(dt1.Rows[0]["DTLS_TAX"]);

            }
            string[] cartkey;
            cartkey = TempData["Cart_key"].ToString().Split(',');
            Byte vRef = 0; Int32 vKey = 0;
          
            EntityCart oBMAST = null;
            HttpCookie objCookie = Request.Cookies["__asd"];
            for (int i = 0; i < cartkey.Length; i++)
            {
                int returnkey;
                dt = new DataTable();
                if (i+1 == cartkey.Length)
                {
                    returnkey = 5;
                }
                else
                {
                    returnkey = 1;
                }
                using (BACart oBMC = new BACart())
                {
                    dt = oBMC.Get<Int32>("GET_SINGLE_CART", Convert.ToInt32(cartkey[i]), cartkey[i], ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                
                errMsg = String.Empty;
                string check = Request[cartkey[i]];
                oBMAST = new EntityCart();
                oBMAST.DTLS_CART_KEY = Convert.ToInt32(cartkey[i]);
                oBMAST.DTLS_PRODUCT_KEY = 0;
                oBMAST.HEAD_PRODUCT_KEY = 0;
                oBMAST.PRICE = Convert.ToDecimal(dt.Rows[0]["PRICE"]);
                oBMAST.QUANTITY = Convert.ToInt32(form[cartkey[i]]);
                oBMAST.TAX = Convert.ToDecimal(((Convert.ToDecimal(oBMAST.PRICE) * tax) / 100) * oBMAST.QUANTITY);
                oBMAST.GROSS_AMOUNT = 0;
                oBMAST.TOTAL_AMOUNT = 0;
                oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);


                if (Convert.ToDecimal(oBMAST.PRICE) * Convert.ToInt32(oBMAST.QUANTITY) <= 120)
                {

                    oBMAST.SHIPPING_AMOUNT = Convert.ToDecimal(10.82);

                }
                else
                {

                    oBMAST.SHIPPING_AMOUNT = 0;

                }

                oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
                oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
                oBMAST.TAG_ACTIVE = 1;
                oBMAST.TAG_DELETE = 0;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                using (BACart oBMC = new BACart())
                {

                    vRef = oBMC.SaveChangesCart<Object, Int32>("UPDATE", oBMAST, returnkey, ref vKey, ref errMsg, "2019", 1);
                    if (vRef == 1)
                    {


                    }
                    else if (vRef == 2)
                    {


                    }
                    else
                    {

                    }




                }


            }

            return Redirect("~/CartUI/Index");

        }



        [HttpPost]
        public string delete(string id)
        {
            Int32 vKey = 0; Byte vRef = 0; String vDelMsg = String.Empty;
            EntityBlog oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string edit = id;
                    errMsg = String.Empty;
                    oBMAST = new EntityBlog();
                    //GridViewRow gvr = (GridViewRow)((HtmlAnchor)sender).NamingContainer;
                    oBMAST.MAST_BLOG_KEY = Convert.ToInt32(edit);
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
                    oBMAST.TAG_DELETE = 0;

                    using (BACart oBMC = new BACart())
                    {
                        vRef = oBMC.SaveDelete<Object, Int32>("DELETE", oBMAST, null, ref vKey, ref errMsg, ref vDelMsg, "2019", 1);
                        if (vRef > 0)
                        {
                            if (vRef == 2)
                            {
                              
                                //  MessageBox(1, "Data exists in We Do Master", errMsg);
                            }
                            else if (vRef == 1)
                            {
                               
                                return "1";

                                //MessageBox(1, Message.msgSaveDelete, "");
                            }
                            else
                            {
                              
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                        }
                    }
                }
                else
                {
                
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

            return "1";
        }

        [HttpPost]
        public JsonResult filter(string addresskey)
        {

            EntityJavaScriptPopulate jspopulate = new EntityJavaScriptPopulate();

            if (addresskey != null && addresskey != "0")
            {

                DataTable Address = new DataTable();
                Address = (DataTable)Session["Address"];
              
                foreach (DataRow o in Address.Rows)
                {
                    

                        if (o["CUSTOMER_ADDRESS_KEY"].ToString() == addresskey)
                        {

                        jspopulate.CUSTOMER_ADRESS_KEY = Convert.ToInt32( o["CUSTOMER_ADDRESS_KEY"]);
                        jspopulate.CUSTOMER_FNAME = o["CUSTOMER_FNAME"].ToString();
                        jspopulate.CUSTOMER_LNAME = o["CUSTOMER_LNAME"].ToString();
                        jspopulate.CUSTOMER_ADDRESS = o["CUSTOMER_ADDRESS"].ToString();
                        jspopulate.CUSTOMER_CITY = o["CUSTOMER_CITY"].ToString();
                        jspopulate.CUSTOMER_COUNTRY = o["CUSTOMER_COUNTRY"].ToString();
                        jspopulate.CUSTOMER_PINCODE = o["CUSTOMER_PINCODE"].ToString();
                        jspopulate.CUSTOMER_PHONE = o["CUSTOMER_PHONE"].ToString();
                        jspopulate.ADDRESS_CUSTOMER_EMAIL = o["CUSTOMER_EMAIL"].ToString();

                        break;

                        }
                      
                }
                
                

            }
          

            return Json(jspopulate);




        }

        [HttpPost]
        public ActionResult SaveshippingAddr(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;


            HttpCookie objCookie = Request.Cookies["__asd"];
            EntityDtlsCustomer oBMAST = null;

            errMsg = String.Empty;
            oBMAST = new EntityDtlsCustomer();
            oBMAST.CUSTOMER_ADRESS_KEY = Convert.ToInt32(form["hf_shipping_address"]);
            oBMAST.CUSTOMER_FNAME = form["txt_shipping_fname"];
            oBMAST.CUSTOMER_LNAME = form["txt_shipping_lname"];
            oBMAST.CUSTOMER_ADDRESS = form["txt_shipping_addr"];
            oBMAST.CUSTOMER_CITY = form["txt_shipping_city"];
            oBMAST.CUSTOMER_COUNTRY = form["txt_shipping_country"];
            oBMAST.CUSTOMER_PHONE = form["txt_shipping_phone"];
            oBMAST.CUSTOMER_PINCODE = form["txt_shipping_pincode"];
            oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);

            oBMAST.ADDRESS_CUSTOMER_EMAIL = form["txt_shipping_email"];

            oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
            oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
            oBMAST.TAG_ACTIVE = 1;
            oBMAST.TAG_DELETE = 0;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");


            using (BAAddress oBMC = new BAAddress())
            {
                if (oBMAST.CUSTOMER_ADRESS_KEY == 0)
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

                }

                else
                {

                }

            }

            return Redirect("~/CartUI/Index");

        }

        [HttpPost]
        public ActionResult SaveBillingAddr(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;


            HttpCookie objCookie = Request.Cookies["__asd"];
            EntityDtlsCustomer oBMAST = null;

            errMsg = String.Empty;
            oBMAST = new EntityDtlsCustomer();
            oBMAST.CUSTOMER_ADRESS_KEY = Convert.ToInt32(form["hf_billing_address"]);
            oBMAST.CUSTOMER_FNAME = form["txt_billing_fname"];
            oBMAST.CUSTOMER_LNAME = form["txt_billing_lname"];
            oBMAST.CUSTOMER_ADDRESS = form["txt_billing_address"];
            oBMAST.CUSTOMER_CITY = form["txt_billing_city"];
            oBMAST.CUSTOMER_COUNTRY = form["txt_billing_country"];
            oBMAST.CUSTOMER_PHONE = form["txt_billing_phone"];
            oBMAST.CUSTOMER_PINCODE = form["txt_billing_pincode"];
            oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);

            oBMAST.ADDRESS_CUSTOMER_EMAIL = form["txt_billing_email"];

            oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
            oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
            oBMAST.TAG_ACTIVE = 1;
            oBMAST.TAG_DELETE = 0;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");


            using (BAAddress oBMC = new BAAddress())
            {
                if (oBMAST.CUSTOMER_ADRESS_KEY == 0)
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

            return Redirect("~/CartUI/Index");

        }


        [HttpPost]

        public ActionResult gotopayment(FormCollection form, string update)
        {

            switch (update)
            {
                case "Update Billing":
                    return (SaveBillingAddr(form));
                case "Update Shipping":
                    return (SaveshippingAddr(form));
                  
            }
            EntityDtlsCustomer oBMAST = null;
            oBMAST = new EntityDtlsCustomer();
            oBMAST.CUSTOMER_ADRESS_KEY = Convert.ToInt32(form["hf_shipping_address"]);
            oBMAST.BILLING_CUSTOMER_ADRESS_KEY = Convert.ToInt32(form["hf_billing_address"]);
            Session["oBMAST"] = oBMAST;


            return Redirect("~/PaymentUI/Index");
        }


    }
}