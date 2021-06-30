using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class IndexUIController : CommonController
    {
        // GET: IndexUI
        // GET: Common
        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        String vString = String.Empty;
        DataTable dt;  //pagesetting
        DataTable dt1; //sitesetting
        DataTable dt2;  //latestproduct
        DataTable dt3; //recentproduct
        DataTable dt4; //popularproduct


        public ActionResult Index()
        {
            FillHomeSettingEdit();
            FillProductTableGrid();
            FillMastLatestProduct();
            FillMastRecenttProduct();
            FillMastPopularProduct();
            bindBlogList();
            layoutpopulate();
            return View();
        }



        private DataTable FillProductTableGrid()
        {
            try
            {

                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BAHomeSettings oBMC = new BAHomeSettings())
                {
                    dt = oBMC.GetBanner<Int32>("GET_BANNER", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                if(dt != null && dt.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);



                    ViewBag.BannerImg1 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[0]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId1 = Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt.Rows[0]["HEAD_PRODUCT_KEY"]));

                    ViewBag.BannerImg2 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[1]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId2 = Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata( Convert.ToString(dt.Rows[1]["HEAD_PRODUCT_KEY"]));

                    ViewBag.BannerImg3 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[2]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId3 = Domain + "/ProductNewtUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt.Rows[2]["HEAD_PRODUCT_KEY"]));

                    ViewBag.BannerImg4 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[3]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId4 = Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata( Convert.ToString(dt.Rows[3]["HEAD_PRODUCT_KEY"]));

                    ViewBag.BannerImg5 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[4]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId5 = Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt.Rows[4]["HEAD_PRODUCT_KEY"]));

                    ViewBag.BannerImg6 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[5]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId6 = Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt.Rows[5]["HEAD_PRODUCT_KEY"]));

                    ViewBag.BannerImg7 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[6]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId7 = Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt.Rows[6]["HEAD_PRODUCT_KEY"]));

                    ViewBag.BannerImg8 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[7]["BANNER_SETTING_IMG"]);
                    ViewBag.BannerImgProductId8 = Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt.Rows[7]["HEAD_PRODUCT_KEY"]));
                }

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }

        }


        private String FillHomeSettingEdit()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt1 = new DataTable();
                using (BAHomeSettings oBME = new BAHomeSettings())
                {
                    dt1 = oBME.GetData("GET", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.ICON_IMG_1 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["ICON_IMG_1"]);
                    ViewBag.ICON_IMG_2 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["ICON_IMG_2"]);
                    ViewBag.ICON_IMG_3 = ConfigurationManager.AppSettings["HOME"].ToString() +  Convert.ToString(dt1.Rows[0]["ICON_IMG_3"]);
                    ViewBag.ICON_IMG_4 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["ICON_IMG_4"]);
                    ViewBag.ICON_IMG_5 = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["ICON_IMG_5"]);

                    ViewBag.ICON_IMG_LINK_1 = Convert.ToString(dt1.Rows[0]["ICON_LINK_1"]);
                    ViewBag.ICON_IMG_LINK_2 = Convert.ToString(dt1.Rows[0]["ICON_LINK_2"]);
                    ViewBag.ICON_IMG_LINK_3 = Convert.ToString(dt1.Rows[0]["ICON_LINK_3"]);
                    ViewBag.ICON_IMG_LINK_4 = Convert.ToString(dt1.Rows[0]["ICON_LINK_4"]);
                    ViewBag.ICON_IMG_LINK_5 = Convert.ToString(dt1.Rows[0]["ICON_LINK_5"]);


                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                dt1 = null;
            }
        }


        private String FillMastLatestProduct()
        {
            try
            {
                errMsg = String.Empty;
                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt2 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt2 = oBMC.Get<Int32>("GET_LATESTPRO", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                if(dt2 != null && dt2.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for(int i = 0; i < dt2.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";

                        }
                        else
                        {
                            vString += "<div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                        }

                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt2.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div>";


                    }

                    ViewBag.LatestProduct = vString;


                }

                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private String FillMastRecenttProduct()
        {
            try
            {
                errMsg = String.Empty;
                vString = string.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt3 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt3 = oBMC.Get<Int32>("GET_RECENTPRO", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                if (dt3 != null && dt3.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt3.Rows[i]["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt3.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt3.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";

                        }
                        else
                        {
                            vString += "<div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt3.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                        }
                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt3.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt3.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div>";


                    }

                    ViewBag.RecentProduct = vString;


                }


                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        private String FillMastPopularProduct()
        {
            try
            {
                errMsg = String.Empty;
                vString = string.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt4 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt4 = oBMC.Get<Int32>("GET_POPULARPRO", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                if (dt4 != null && dt4.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt4.Rows[i]["PRODUCT_IMAGE"])!="")
                        {
                            vString += "<div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt4.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt4.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";

                        }
                        else
                        {
                            vString += "<div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt4.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                        }

                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt4.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt4.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div>";


                    }

                    ViewBag.Popularproduct = vString;


                }

                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void bindBlogList()
        {
            dt = new DataTable();
            vString = string.Empty;
            string blogheading;
            using (BABlog oBME = new BABlog())
            {
                dt = oBME.Get("ALL_BLOG", 0, "", ref errMsg, Convert.ToString(DateTime.Now), 1);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StringBuilder sb = new StringBuilder(dt.Rows[i]["BLOG_HEADING"].ToString());
                    sb.Replace(" ", "");
                    sb.Replace(",", "");
                    sb.Replace(".", "");
                    blogheading = Convert.ToString(sb);
                    vString += "<div class='col-lg-12'><div class='card card-blog'><div class='blog-shape'>"
                        + "<a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt=''></a>"
                        + "<span class='newtag'>Latest News</span></div><span class='blog-date'>" + dt.Rows[i]["BLOG_DATE"] + "</span>" +
                        "<div class='blog-info'><h3><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'>" + dt.Rows[i]["BLOG_HEADING"] + "</a></h3>"
                        + " <p>" + dt.Rows[i]["DESCRIPTION"] + "...</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "' class='readmore'>Read more<i class='fa fa-arrow-right'></i></a></div></div></div>";
                       
                }
                ViewBag.blogRender = vString;

            }
        }

    }
}