using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.Entity;
using System.IO;
using System.Web.UI.WebControls;
using MyApp.Entity.Message;


namespace DiecastDecals.Controllers
{
    public class AboutUsUIController : CommonController
    {
        // GET: AboutUsUI

        DataTable dt;
        DataTable dt1;
        String errMsg = String.Empty;
        String vString = String.Empty, vString1 = String.Empty, vString2 = String.Empty, vString3 = String.Empty, vString4 = String.Empty;
        public ActionResult Index()
        {
            layoutpopulate();
        
            FillAboutUs();
            FillLinks();
            FillTestimonial();
            return View();
        }

        #region Site Setting
        private String FillSiteSetting()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAHeadSiteSetting oBME = new BAHeadSiteSetting())
                {
                    dt = oBME.GetData("ALL", "", ref errMsg, null, 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.CONTACT_NO = Convert.ToString(dt.Rows[0]["CONTACT_NO"]);
                    ViewBag.MAIL = Convert.ToString(dt.Rows[0]["MAIL"]);
                    ViewBag.ADDRESS = Convert.ToString(dt.Rows[0]["ADDRESS"]);
                    ViewBag.HEADER_LOGO = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[0]["HEADER_LOGO"]);
                    ViewBag.FACEBOOK_LINK = Convert.ToString(dt.Rows[0]["FACEBOOK_LINK"]);
                    ViewBag.TWITTER_LINK = Convert.ToString(dt.Rows[0]["TWITTER_LINK"]);
                    ViewBag.INSTAGRAM_LINK = Convert.ToString(dt.Rows[0]["INSTAGRAM_LINK"]);
                    ViewBag.PRINTEREST_LINK = Convert.ToString(dt.Rows[0]["PRINTEREST_LINK"]);


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

        #endregion

        #region About us Section
        private String FillAboutUs()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt = new DataTable();
                dt1 = new DataTable();
                using (BAAboutUs oBME = new BAAboutUs())
                {
                    dt = oBME.GetData("GET", "", ref errMsg, null, 1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ViewBag.ABOUT_US_IMAGE = ConfigurationManager.AppSettings["ABOUT"].ToString() + Convert.ToString(dt.Rows[0]["ABOUT_US_IMAGE"]);
                        ViewBag.ABOUT_US_DESC = Convert.ToString(dt.Rows[0]["ABOUT_US_DESC"]);
                    }
                }
               
                using (BAHeadSiteSetting oBME = new BAHeadSiteSetting())
                {
                    dt1 = oBME.GetData("GET", "", ref errMsg, null, 1);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        ViewBag.HEADER_LOGO= ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["HEADER_LOGO"]);
                    }
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
        #endregion

        #region Icon Link
        private DataTable FillLinks()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAHomeSettings oBME = new BAHomeSettings())
                {
                    dt = oBME.Get("GET_ICON", 0, "", ref errMsg, null, 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.ICON_IMG_1 = "" + ConfigurationManager.AppSettings["HOME"].ToString() + dt.Rows[0]["ICON_IMG_1"] + "";
                    ViewBag.ICON_IMG_2 = "" + ConfigurationManager.AppSettings["HOME"].ToString() + dt.Rows[0]["ICON_IMG_2"] + "";
                    ViewBag.ICON_IMG_3 = "" + ConfigurationManager.AppSettings["HOME"].ToString() + dt.Rows[0]["ICON_IMG_3"] + "";
                    ViewBag.ICON_IMG_4 = "" + ConfigurationManager.AppSettings["HOME"].ToString() + dt.Rows[0]["ICON_IMG_4"] + "";
                    ViewBag.ICON_IMG_5 = "" + ConfigurationManager.AppSettings["HOME"].ToString() + dt.Rows[0]["ICON_IMG_5"] + "";


                    ViewBag.ICON_DESC_1 = Convert.ToString(dt.Rows[0]["ICON_LINK_1"]);
                    ViewBag.ICON_DESC_2 = Convert.ToString(dt.Rows[0]["ICON_LINK_2"]);
                    ViewBag.ICON_DESC_3 = Convert.ToString(dt.Rows[0]["ICON_LINK_3"]);
                    ViewBag.ICON_DESC_4 = Convert.ToString(dt.Rows[0]["ICON_LINK_4"]);
                    ViewBag.ICON_DESC_5 = Convert.ToString(dt.Rows[0]["ICON_LINK_5"]);

                }
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
        
            return dt;
        }


        #endregion

        #region Testimonials
        private DataTable FillTestimonial()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAMastTestimonials oBME = new BAMastTestimonials())
                {
                    dt = oBME.Get("GET", 0, "", ref errMsg, null, 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vString += "<div class='single_testimonial item'><div class='row'><div class='customer-img col-lg-3 m-auto'>";
                        if(Convert.ToString(dt.Rows[i]["CLIENT_IMAGE"]) != "")
                        {
                            vString += "<img src='" + Convert.ToString(dt.Rows[i]["CLIENT_IMAGE"]) + "' alt=''></div><div class='col-lg-9 comments'>";
                        }
                     
                       else
                        {
                            vString += "<img src='/documents/client/blank.png' alt=''></div><div class='col-lg-9 comments'>";
                        }
                        vString += "<p>" + dt.Rows[i]["CLIENT_FEEDBACK"] + "</p>";
                        vString += "<div class='name'>" + dt.Rows[i]["CLIENT_NAME"] + "</div></div></div></div>";
                        
                    }
                    ViewBag.DIV_TESTIMONIALS = vString;

                }

            }
            catch (Exception ex)
            {
                //return ex.Message;
            }

            return dt;
        }
        #endregion
    }
}