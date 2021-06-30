using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.db.Encryption;
using MyApp.Entity;

namespace DiecastDecals.Controllers
{
    public class ProductNewUIController : CommonController
    {
        // GET: ProductNewUI
        String errMsg = String.Empty;
        DataTable dt;
        DataTable dt1;
        String vString = String.Empty;
        String vString1 = String.Empty;
        public ActionResult Index(string id)
        {
            if(id!= null)
            {

                FillDtlsProductGrid(Convert.ToInt32(Encrypted.Decryptdata(id)));
                FillMastProductEdit(Convert.ToInt32(Encrypted.Decryptdata(id)));
               
            }
            layoutpopulate();

            return View();
            

        }



        private String FillDtlsProductGrid(Int32 pSearchKey)
        {
            try
            {
                Session["HEAD_PRODUCT"] = pSearchKey;
                errMsg = String.Empty;
                vString = string.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt1 = new DataTable();
                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                using (BAProduct oBMC = new BAProduct())
                {
                    dt1 = oBMC.Get<Int32>("SRH_KEY", pSearchKey,"", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }

                if (dt1 != null && dt1.Rows.Count>0)
                {

                

                for (int i = 0; i < dt1.Rows.Count; i++)
                {


                        vString += "<div class='carousel-item productzoomcont active'> <img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_1_IMAGE"]) + "' id='zoom_03' data-zoom-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_1_IMAGE"]) + "' alt='Hills'></div>";

                        vString1 += "<li class='list-inline-item custom-li-active active'  id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_1"]) + "1'>";
                        vString1 += "<a href='#' onclick='click1(this.id);' id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_1"]) + "' class='elevatezoom-gallery active' data-update='' data-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_1_IMAGE"]) + "' data-zoom-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_1_IMAGE"]) + "'>";
                        vString1 += "<img src='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_1_IMAGE"]) + "' class='img-fluid'></a></li>";



                        vString1 += "<li class='list-inline-item custom-li-active' id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_2"]) + "1'>";
                        vString1 += "<a href='#'  onclick='click1(this.id);' id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_2"]) + "' class='elevatezoom-gallery active' data-update='' data-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_2_IMAGE"]) + "' data-zoom-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_2_IMAGE"]) + "'>";
                        vString1 += "<img src='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_2_IMAGE"]) + "' class='img-fluid'></a></li>";

                        vString1 += "<li class='list-inline-item custom-li-active' id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_3"]) + "1'>";
                        vString1 += "<a href='#'  onclick='click1(this.id);' id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_3"]) + "' class='elevatezoom-gallery active' data-update='' data-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_3_IMAGE"]) + "' data-zoom-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_3_IMAGE"]) + "'>";
                        vString1 += "<img src='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_3_IMAGE"]) + "' class='img-fluid'></a></li>";

                        vString1 += "<li class='list-inline-item custom-li-active' id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_4"]) + "1'>";
                        vString1 += "<a href='#' onclick='click1(this.id);' id='" + Convert.ToString(dt1.Rows[0]["SHEET_CODE_4"]) + "' class='elevatezoom-gallery active' data-update='' data-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_4_IMAGE"]) + "' data-zoom-image='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_4_IMAGE"]) + "'>";
                        vString1 += "<img src='" + Domain + "/" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["SHEET_CODE_4_IMAGE"]) + "' class='img-fluid'></a></li>";



                }

                    ViewBag.Sheetcode = Convert.ToString(dt1.Rows[0]["SHEET_CODE_1"]);
                    ViewBag.slide = vString;
                    ViewBag.slideshow = vString1;

                }

                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private String FillMastProductEdit(Int32 pDTLS_WE_DO_IT_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAProduct oBMG = new BAProduct())
                {
                    dt = oBMG.Get<Int32>("SRH_KEY", pDTLS_WE_DO_IT_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {

                    ViewBag.txt_PRODUCT_NAME = Convert.ToString(dt.Rows[0]["PRODUCT_NAME"]);
                    
                    ViewBag.ddl_MAST_PRODUCT_CATG_KEY = Convert.ToString(dt.Rows[0]["MAST_CATEGORY_KEY"]);
                  TempData["txt_PRICE"] =  ViewBag.txt_PRICE = Convert.ToString(dt.Rows[0]["PRICE"]);
                    if (Convert.ToString(dt.Rows[0]["PRODUCT_WATER_MARK_IMAGE"]) != "")
                    {
                        ViewBag.head_product_img = ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["PRODUCT_WATER_MARK_IMAGE"]);
                    }
                    else
                    {
                        ViewBag.head_product_img = ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg";

                    }
                   
                    ViewBag.hf_HEAD_PRODUCT_IMG = Convert.ToString(dt.Rows[0]["PRODUCT_WATER_MARK_IMAGE"]);
                   
                }
                FillMastProductCATG(Convert.ToInt32(ViewBag.ddl_MAST_PRODUCT_CATG_KEY));
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                dt = null;
            }
        }


        private String FillMastProductCATG(Int32 pDTLS_WE_DO_IT_KEY)
        {
            try
            {
                errMsg = String.Empty;
                vString = string.Empty;
                dt = new DataTable();
                using (BAProduct oBMG = new BAProduct())
                {
                    dt = oBMG.Get<Int32>("GET_PROCATG", pDTLS_WE_DO_IT_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {




                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vString += "<div class='row mb-3'><div class='col-md-4 col-4 col-xl-3'>";
                        vString += "<img src='"+ ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[i]["PRODUCT_IMAGE"]) + "' alt=''>";
                        vString += "</div><div class='col-md-8 col-8'><h6>"+ Convert.ToString(dt.Rows[i]["PRODUCT_NAME"]) + "</h6>";
                        vString += "</div></div>";
                       
                    }

                    ViewBag.relatedproduct = vString;

                }

                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                dt = null;
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
                        oBMAST.SHEET_CODE = form["hf_sheet_code"];
                        oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(Session["HEAD_PRODUCT"]);
                        oBMAST.PRICE = 0;

                        decimal price = Convert.ToDecimal(TempData["txt_PRICE"]);

                        oBMAST.QUANTITY = Convert.ToInt32(form["txt_quantity"]);
                        oBMAST.TAX = Convert.ToDecimal(((Convert.ToDecimal(price) * tax) / 100)* oBMAST.QUANTITY);
                        oBMAST.GROSS_AMOUNT = 0;
                        oBMAST.TOTAL_AMOUNT = 0;
                        oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);

                        if (Convert.ToDecimal(TempData["txt_PRICE"]) * Convert.ToInt32(form["txt_quantity"]) <= 120)
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

                            vRef = oBMC.SaveChangesCart<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
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
                using (BATax oBME = new BATax())
                {
                    dt1 = oBME.GetData("ALL", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    TempData["tax"] = Convert.ToDecimal(dt1.Rows[0]["DTLS_TAX"]);

                }


                //Session["Price"] = TempData["txt_PRICE"];
                //TempData["JavaScriptFunction"] = string.Format("savetolocalstorage();");
                //return Redirect("~/ProductNewUI/Index");
                return Redirect("~/LoginUI/Index");
            }

        }


    }
}