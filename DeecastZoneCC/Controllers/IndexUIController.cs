using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class IndexUIController : CommonController
    {
        // GET: IndexUI
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        String errMsg = String.Empty;
        String vString = String.Empty, vString1=String.Empty;
        public ActionResult Index()
        {
            layoutpopulate();
            FillHomeSetting();
            getcart();
            return View(dt.Rows);
        }

        public void FillHomeSetting()
        {
            try
            {
                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                //banner
                using (BAHomeSettings oBHS = new BAHomeSettings())
                {
                    dt = oBHS.GetData("GET", "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.BANNER_IMAGE = Domain + ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[0]["BANNER_IMAGE"]);
                        ViewBag.BANNER_DESC = Convert.ToString(dt.Rows[0]["BANNER_DESC"]);
                        ViewBag.BANNER_VIDEO_LINK = Convert.ToString(dt.Rows[0]["BANNER_VIDEO_LINK"]);
                    }
                }
                
                //product
                using (BAProduct oBMC = new BAProduct())
                {
                    dt1 = oBMC.Get("GET_DETIALS", "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    if (dt1.Rows.Count > 0)
                    {
                        //ViewBag.txt_quantity = 1;
                        FormCollection form = new FormCollection();
                        ViewBag.PRODUCT_IMAGE = Domain + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["PRODUCT_IMAGE"]);
                        ViewBag.PRODUCT_IMAGE_1 = Domain + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt1.Rows[0]["PRODUCT_IMAGE_1"]);
                        ViewBag.PRICE = Convert.ToString(dt1.Rows[0]["PRICE"]);
                        //double totalprice= Convert.ToInt16(form["txt_quantity"]) * Convert.ToDouble(ViewBag.PRICE);
                        //ViewBag.TOTALPRICE = totalprice;

                        for (int i=0; i< dt1.Rows.Count; i++)
                        {
                            vString1 += "<li><span>"+ dt1.Rows[i]["QTY"] +"</span><div class='decp'><a href='/CartUI/Index'>";
                            vString1 += "<i class='fas fa-chevron-double-right'></i> $ " + dt1.Rows[i]["PRICE"] + " Each <i class='fas fa-shopping-cart'></i></a></div></li>";
                           
                        }
                        ViewBag.PRODUCT_DETAILS = vString1;
                    }
                }

                //testimonial
                using (BATestimonials oBMC = new BATestimonials())
                {
                    dt2 = oBMC.GetTestimonial("GET",0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    if (dt2.Rows.Count > 0)
                    {
                       for(int i=0; i< dt2.Rows.Count; i++)
                       {
                            vString += "<li><img src='"+ Domain + ConfigurationManager.AppSettings["CLIENT"].ToString() + Convert.ToString(dt2.Rows[i]["CUST_IMAGE"]) + "' alt=''><h4>"+ dt2.Rows[i]["CUST_NAME"] + "</h4>";
                            vString += "<i class='fas fa-star'></i><i class='fas fa-star'></i><i class='fas fa-star'></i><i class='fas fa-star'></i><i class='fas fa-star'></i>";
                            vString += "<p>" + dt2.Rows[i]["REMARKS"] + "</p><h5>" + dt2.Rows[i]["POSTING_DATE"] + "</h5></li>";
                       }
                        ViewBag.TESTIMONIAL_LIST = vString;
                    }
                }

               

            }
            catch (Exception ex)
            {

            }
        }

        private void getcart()
        {
            dt = new DataTable();
            HttpCookie objCookie = Request.Cookies["__asd"];

            if (objCookie != null)
            {


                using (BACart oBMC = new BACart())
                {
                    dt = oBMC.Get<Int32>("GET_CART", 0, Encrypted.Decryptdata(objCookie["a"].ToString()).ToString(), ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
            }
            else
            {

                using (BACart oBMC = new BACart())
                {
                    dt = oBMC.Get<Int32>("GET_CART", 0, 0.ToString(), ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {

                ViewBag.subtotal = dt.Rows[0]["SUB_TOTAL"];
                ViewBag.PRODUCT_IMAGE1 = ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["PRODUCT_IMAGE"]);

            }


        }

    }
}