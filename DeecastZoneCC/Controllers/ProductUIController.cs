using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class ProductUIController : CommonController
    {
        // GET: ProductUI

        EntityProduct oEP = null;
        EntityProductCatg oEPC = null;
        String errMsg = String.Empty;
        String vString = String.Empty;
        DataTable dt;  //pagesetting
        DataTable dt1; //sitesetting
        DataTable dt2;  //latestproduct
        DataTable dt3; //recentproduct
        DataTable dt4; //popularproduct
        public ActionResult Index()
        {
            layoutpopulate();
            FillHeadProduct();
            return View();
        }

        private void FillHeadProduct()
        {
            try
            {
                String errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                dt1 = new DataTable();
                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                using (BAProduct oBMC = new BAProduct())
                {
                    dt = oBMC.Get("GET_DETIALS", "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                    ViewBag.head_product = dt.Rows[0]["HEAD_PRODUCT_KEY"];
                  ViewBag.Price = dt.Rows[0]["1STQTY"];

                }
                if(dt.Rows.Count>0)
                {
                    ViewBag.PRODUCT_IMAGE = Domain +  ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["PRODUCT_IMAGE"]);
                    ViewBag.PRODUCT_IMAGE_1 = Domain +  ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["PRODUCT_IMAGE_1"]);
                    ViewBag.PRODUCT_IMAGE_2 = Domain +  ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["PRODUCT_IMAGE_2"]);
                    ViewBag.PRODUCT_DESC = dt.Rows[0]["PRODUCT_DESC"].ToString();
                }
                //using (BAHomeSettings oBHS = new BAHomeSettings())
                //{
                //    dt1 = oBHS.GetData("GET", "", ref errMsg, "2019", 1);
                //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                    
                //}
                //if (dt1.Rows.Count > 0)
                //{
                //    ViewBag.LARGE_VIDEO_LINK =  Convert.ToString(dt1.Rows[0]["LARGE_VIDEO_LINK"]);
                   
                //}

            }
            catch(Exception ex)
            {

            }
        }

        [HttpPost]
        public ActionResult addtocart(FormCollection form)
        {

            HttpCookie objCookie = Request.Cookies["__asd"];
            if (objCookie != null)
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

                try
                {
                    if (ModelState.IsValid)
                    {

                        errMsg = String.Empty;
                        oBMAST = new EntityCart();
                        oBMAST.DTLS_CART_KEY = 0;
                        oBMAST.DTLS_PRODUCT_KEY = Convert.ToInt32(0);
                        oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(form["head_product_key"]);
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

                            vRef = oBMC.SaveChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
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
                    }


                }
                catch (Exception ex)
                {

                    //MessageBox(2, Message.msgErrCommon, ex.Message);
                }
                finally
                {
                    if (oBMAST != null)
                        oBMAST = null;
                }
                return Redirect("~/CartUI/Index");
            }
            else
            {
                return Redirect("~/LoginUI/Index");
            }

        }



        public JsonResult filter(string QTY)
        {
            EntityJavaScriptPopulate jspopulate = new EntityJavaScriptPopulate();


            if (QTY == "")
            {
                jspopulate.PRODUCT_PRICE = 0;
            }
            else
            {
                
                using (BAProduct oBMC = new BAProduct())
                {
                    dt1 = oBMC.GetDatadtls("GET_PRICE", QTY, ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }


                jspopulate.PRODUCT_PRICE = Convert.ToDecimal(dt1.Rows[0]["PRICE"]);

            }

            return Json(jspopulate);

        }

    }
}