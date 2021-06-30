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
    public class CartUIController : CommonController
    {
        // GET: CartUI
        DataTable dt;
        DataTable dt1;
        String errMsg = String.Empty;
        String vString = String.Empty;
        public ActionResult Index()
        {
            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
            {



                getcart(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
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
                dt = oBMC.Get<Int32>("GET_CART", 0, useremail, ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (dt != null && dt.Rows.Count > 0)
            {
              
                ViewBag.subtotal = dt.Rows[0]["SUB_TOTAL"];
                ViewBag.PRODUCT_IMAGE = ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["PRODUCT_IMAGE"]);

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

            Byte vRef = 0; Int32 vKey = 0;

            EntityCart oBMAST = null;
            HttpCookie objCookie = Request.Cookies["__asd"];
           
                errMsg = String.Empty;
                string check = Request["hf_cart_key"];
                oBMAST = new EntityCart();
                oBMAST.DTLS_CART_KEY = Convert.ToInt32(form["hf_cart_key"]);
                oBMAST.DTLS_PRODUCT_KEY = 0;
                oBMAST.HEAD_PRODUCT_KEY = 0;
                oBMAST.PRICE = Convert.ToDecimal(form["txt_price"]);
                oBMAST.QUANTITY = Convert.ToInt32(form["txt_quantity"]);
                oBMAST.TAX = tax * oBMAST.QUANTITY;
                oBMAST.GROSS_AMOUNT = 0;
                oBMAST.TOTAL_AMOUNT = 0;
                oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);


                oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
                oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
                oBMAST.TAG_ACTIVE = 1;
                oBMAST.TAG_DELETE = 0;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                using (BACart oBMC = new BACart())
                {

                    vRef = oBMC.SaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                    if (vRef == 1)
                    {

                        //MessageBox(2, Message.msgSaveNew, "");


                    }
                    else if (vRef == 2)
                    {
                        //MessageBox(2, Message.msgSaveDuplicate, errMsg);

                    }
                    else
                    {

                    }




                }

                // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                //ClearControl();
            

            return Redirect("~/CartUI/Index");

        }


    }
}