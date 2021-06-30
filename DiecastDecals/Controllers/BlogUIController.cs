using MyApp.db;
using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
using System.Text;
using MyApp.db.Encryption;

namespace DiecastDecals.Controllers
{
    public class BlogUIController : CommonController
    {
        // GET: BlogUI
        String errMsg = String.Empty, vString = String.Empty;
        DataTable dt = null;
        DataTable dt1 = null;
        DataSet ds = null;
        string[] planname;
        string year;
        string month;
        public ActionResult Index(string id = null, string desc = null,string catid = null)
        {
            try
            {

                Allcatagory();
                bindBlogList();
                layoutpopulate();
                if(id != null)
                {
                   if(id == "ARCHIVE")
                    {

                        //planname = catid.Split('-');
                        //ViewBag.heading = desc.ToString();
                        month = desc;
                        year = catid;
                        //Checking Query String
                        if (month != null)
                        {
                            if (year != null)
                            {
                            
                                vString = string.Empty;
                                dt = new DataTable();
                                string blogheading;
                                using (BABlog oBME = new BABlog())
                                {
                                    dt = oBME.Get("SRH_ARCHIVE", Convert.ToInt32(year), month, ref errMsg, Convert.ToString(DateTime.Now), 1);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            StringBuilder sb = new StringBuilder(dt.Rows[i]["BLOG_HEADING"].ToString());
                                            sb.Replace(" ", "");
                                            sb.Replace(",", "");
                                            sb.Replace(".", "");
                                            blogheading = Convert.ToString(sb);
                                            vString += "<div class='und-blle'>"
                                                + "<h2>" + dt.Rows[i]["BLOG_HEADING"] + "</h2><p>" + dt.Rows[i]["BLOG_DATE"] + "</p>"
                                                + "<div class='undIm'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' /></div>"
                                                + "<p>" + dt.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "' class='rmm'>"
                                                + "Read More</a></div>";
                                        }
                                        ViewBag.blogRender = vString;

                                    }
                                    else
                                    {
                                        ViewBag.blogRender = "<div class='und-blle'><h2>No Blog in this category</p></div>";
                                    }
                                }

                            }


                        }
                    }
                    else
                    {
                        if (catid != null)
                        {


                            planname = catid.Split('-');
                            ViewBag.heading = desc.ToString();
                            //Checking Query String
                            if (planname != null)
                            {
                                if (planname != null)
                                {
                                    string plan = Convert.ToString(planname[0]);
                                    Int32 postKey = Convert.ToInt32(Encrypted.DecryptASCII(plan));
                                    vString = string.Empty;
                                    dt = new DataTable();
                                    string blogheading;
                                    using (BABlog oBME = new BABlog())
                                    {
                                        dt = oBME.Get("GET_CAT_DTLS", postKey, "", ref errMsg, Convert.ToString(DateTime.Now), 1);
                                        if (dt != null && dt.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                StringBuilder sb = new StringBuilder(dt.Rows[i]["BLOG_HEADING"].ToString());
                                                sb.Replace(" ", "");
                                                sb.Replace(",", "");
                                                sb.Replace(".", "");
                                                blogheading = Convert.ToString(sb);
                                                vString += "<div class='und-blle'>"
                                                    + "<h2>" + dt.Rows[i]["BLOG_HEADING"] + "</h2><p>" + dt.Rows[i]["BLOG_DATE"] + "</p>"
                                                    + "<div class='undIm'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' /></div>"
                                                    + "<p>" + dt.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "' class='rmm'>"
                                                    + "Read More</a></div>";
                                            }
                                            ViewBag.blogRender = vString;

                                        }
                                        else
                                        {
                                            ViewBag.blogRender = "<div class='und-blle'><h2>No Blog in this category</p></div>";
                                        }
                                    }

                                }


                            }

                        }
                        else if (id != null)
                        {


                            planname = id.Split('-');
                            ViewBag.heading = desc.ToString();
                            //Checking Query String
                            if (planname != null)
                            {
                                if (planname != null)
                                {
                                    string plan = Convert.ToString(planname[0]);
                                    Int32 postKey = Convert.ToInt32(Encrypted.DecryptASCII(plan));
                                    dt = new DataTable();
                                    using (BABlog oBME = new BABlog())
                                    {
                                        dt = oBME.Get("SRH_KEY", postKey, "", ref errMsg, Convert.ToString(DateTime.Now), 1);
                                        if (dt != null && dt.Rows.Count > 0)
                                        {
                                            ViewBag.title = dt.Rows[0]["BLOG_HEADING"].ToString();
                                            ViewBag.MetaDescription = dt.Rows[0]["META_DESC"].ToString();
                                            ViewBag.MetaKeywords = dt.Rows[0]["META_KEYWORD"].ToString();
                                        }
                                    }
                                }


                            }

                            else
                            {
                                dt = new DataTable();
                                using (BABlog oBME = new BABlog())
                                {
                                    dt = oBME.Get("ALL_BLOG", 0, "", ref errMsg, Convert.ToString(DateTime.Now), 1);
                                }
                            }

                            if (planname != null)
                            {
                                if (planname != null)
                                {
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        vString =
                                              "<h1>" + dt.Rows[0]["BLOG_HEADING"] + "</h1><p>" + dt.Rows[0]["BLOG_DATE"] + "</p>"
                                            //+ "<p>" + dt.Rows[0]["BLOG_QUESTION"] + "</p>"
                                            + "<div class='undIm'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[0]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' /></div>"
                                            + "<div class='blog-content'>" + dt.Rows[0]["DESCRIPTION"] + "</div>"
                                             + "<div class='a2a_kit a2a_kit_size_32 a2a_default_style a2a_style'>"
                                            + "<a class='a2a_button_facebook'></a><a class='a2a_button_twitter'></a><a class='a2a_button_pinterest'></a>"
                                            + "<a class='a2a_dd' href='https://www.addtoany.com/share'></a></div>"
                                            + "<a class='blgBk' href='/blogui'>Back</a>";

                                        TempData["bloginnerrender"] = vString;
                                        TempData["jsf"] = String.Format("callingfromjs();");


                                    }
                                }

                            }
                            else
                            {
                                //bindBlogList(dt);
                            }
                        }
                    }
                }

              
            }
            catch (Exception ex)
            {
                // Page.ClientScript.RegisterStartupScript(GetType(), "err", "alert('" + ex.Message + "');", true);
            }

            return View();
        }


        private void bindBlogList()
        {
            dt = new DataTable();
            
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
                    sb.Replace(" ","");
                    sb.Replace(",", "");
                    sb.Replace(".","");
                    blogheading = Convert.ToString(sb);
                    vString += "<div class='und-blle'>"
                        + "<h2>" + dt.Rows[i]["BLOG_HEADING"] + "</h2><p>" + dt.Rows[i]["BLOG_DATE"] + "</p>"
                        + "<div class='undIm'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' /></div>"
                        + "<p>" + dt.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) +"/"+ blogheading + "' class='rmm'>"
                        + "Read More</a></div>";
                }
                ViewBag.blogRender = vString;

            }
        }

        private void Allcatagory()
        {
            try
            {
                ds = new DataSet();
                using (BABlog oBME = new BABlog())

                {
                    ds = oBME.GetData("SIDEBAR", 0, "", ref errMsg, Convert.ToString(DateTime.Now), 1);
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = new DataTable();
                    dt = ds.Tables[0]; //Recent Posts
                    string heading;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string li = String.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            StringBuilder sb = new StringBuilder(dt.Rows[i]["BLOG_HEADING"].ToString());
                            sb.Replace(" ", "");
                            sb.Replace(",", "");
                            sb.Replace(".", "");
                            heading = Convert.ToString(sb);
                            li += "<li><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + heading + "'><span class='fa fa-check'></span> " + dt.Rows[i]["BLOG_HEADING"] + "</a></li>";
                        }
                        ViewBag.ulRecentPosts = li;
                    }

                    dt = new DataTable();
                    dt = ds.Tables[1]; //Category
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string ulhtml = String.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ulhtml += "<li><a href='" + "/BlogUI/Index/CATG/" + dt.Rows[i]["BLOG_CAT_DESC"].ToString().Replace(" ","") + "/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_CATEGORY_KEY"].ToString()) + "'><span class='fa fa-check'></span> " + dt.Rows[i]["BLOG_CAT_DESC"] + "</a></li>";
                        }
                        ViewBag.categoryul = ulhtml;
                    }

                    dt = new DataTable();
                    dt = ds.Tables[2]; //Archive
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string li = String.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            li += "<li><a href='" + "/BlogUI/Index/ARCHIVE/" + dt.Rows[i]["MONTH"].ToString() + "/" + dt.Rows[i]["YEAR"].ToString() + "'><span class='fa fa-check'></span> " + dt.Rows[i]["DESC"] + "</a></li>";
                        }
                        ViewBag.ulArchive = li;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        [HttpPost]
        public ActionResult btn_Search_click(FormCollection form)
        {

            string blogheading;
            String search = Convert.ToString(form["txt_Search"]);
            DataTable dt2 = new DataTable();
            using (BABlog oBME = new BABlog())
            {
                dt2 = oBME.Get("GET_SEARCH", 0, search, ref errMsg, Convert.ToString(DateTime.Now), 1);
            }
            if (dt2.Rows.Count > 0)
            {


                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    StringBuilder sb = new StringBuilder(dt2.Rows[i]["BLOG_HEADING"].ToString());
                    sb.Replace(" ", "");
                    sb.Replace(",", "");
                    sb.Replace(".", "");
                    blogheading = Convert.ToString(sb);

                    vString += "<div class='und-blle'>"
                        + "<h2>" + dt2.Rows[i]["BLOG_HEADING"] + "</h2><p>" + dt2.Rows[i]["BLOG_DATE"] + "</p>"
                        + "<div class='undIm'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt2.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt2.Rows[0]["BLOG_HEADING"] + "' title='Dejabrew Blog' /></div>"
                        + "<p>" + dt2.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt2.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'>"
                        + "Read More</a></div>";
                }
                ViewBag.blogRender = vString;
                form["txt_Search"] = "";
                Allcatagory();
            }
            else
            {
                vString += "<div class='und-blle'><h2>Sorry No Record Found !!!!!</h2><a href=' /BlogUI/Index/'>"
                        + "Back</a></div>";
                ViewBag.blogRender = vString;
             
                Allcatagory();
            }
            layoutpopulate();
            return View("Index");
           
        }

        [HttpPost]
        public ActionResult btn_Searching_click(FormCollection form)
        {
            String search = Convert.ToString(form["txt_Search1"]);
            DataTable dt2 = new DataTable();
            using (BABlog oBME = new BABlog())
            {
                dt2 = oBME.Get("GET_SEARCH", 0, search, ref errMsg, Convert.ToString(DateTime.Now), 1);
            }
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    vString += "<div class='und-blle'>"
                        + "<h2>" + dt2.Rows[i]["BLOG_HEADING"] + "</h2><p>" + dt2.Rows[i]["BLOG_DATE"] + "</p>"
                        + "<div class='undIm'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt2.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt2.Rows[0]["BLOG_HEADING"] + "' title='Dejabrew Blog' /></div>"
                        + "<p>" + dt2.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt2.Rows[i]["MAST_BLOG_KEY"].ToString()) + "'>"
                        + "Read More</a></div>";
                }
                ViewBag.blogRender = vString;
                form["txt_Search1"] = "";
                Allcatagory();
            }
            return View("Index");

        }
    }

}