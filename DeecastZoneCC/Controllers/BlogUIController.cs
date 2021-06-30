using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
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
        public ActionResult Index(string id = null, string desc = null, string catid = null)
        {
            Allcatagory();
            bindBlogList();
            layoutpopulate();
            if (id != null)
            {
                if (id == "ARCHIVE")
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
                                        vString += "<div class='sec1'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' />"

                       + "<div class='wrap'><h3><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'>" + dt.Rows[i]["BLOG_HEADING"] + "</a></h3><h4>Posted on " + dt.Rows[i]["BLOG_DATE"] + "</h4>"
                       + "<p>" + dt.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "' class='more-but'>"
                       + "Read More</a></div></div>";
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
                                            vString += "<div class='sec1'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' />"

                        + "<div class='wrap'><h3><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'>" + dt.Rows[i]["BLOG_HEADING"] + "</a></h3><h4>Posted on " + dt.Rows[i]["BLOG_DATE"] + "</h4>"
                        + "<p>" + dt.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "' class='more-but'>"
                        + "Read More</a></div></div>";
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

                                vString = string.Empty;
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

                                    vString = string.Empty;
                                    string blogheading;
                                    StringBuilder sb = new StringBuilder(dt.Rows[0]["BLOG_HEADING"].ToString());
                                    sb.Replace(" ", "");
                                    sb.Replace(",", "");
                                    sb.Replace(".", "");
                                    blogheading = Convert.ToString(sb);


                                    vString += "<div class='sec1'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[0]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' />"

                        + "<div class='wrap'><h3><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[0]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'>" + dt.Rows[0]["BLOG_HEADING"] + "</a></h3><h4>Posted on " + dt.Rows[0]["BLOG_DATE"] + "</h4>"
                        + "<p>" + dt.Rows[0]["DESCRIPTION"] + "</p><a href='/BlogUI/Index' class='more-but'>"
                        + "Back</a></div></div>";

                                    ViewBag.blogRender = vString;
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
                    sb.Replace(" ", "");
                    sb.Replace(",", "");
                    sb.Replace(".", "");
                    blogheading = Convert.ToString(sb);
                    vString += "<div class='sec1'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt.Rows[0]["BLOG_HEADING"] + "' />"
                   
                        + "<div class='wrap'><h3><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'>" + dt.Rows[i]["BLOG_HEADING"] + "</a></h3><h4>Posted on " + dt.Rows[i]["BLOG_DATE"] + "</h4>"
                        + "<p>" + dt.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "' class='more-but'>"
                        + "Read More</a></div></div>";
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
                            li += "<li><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + heading + "'><img src='"+ ConfigurationManager.AppSettings["BLOG"].ToString() + dt.Rows[i]["BLOG_IMAGE"] + "' alt=''><h4> "+ dt.Rows[i]["BLOG_HEADING"].ToString() + "</h4><h5>— "+ dt.Rows[i]["BLOG_DATE"] + "</h5></a></li>";
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
                            ulhtml += "<li><a href='" + "/BlogUI/Index/CATG/" + dt.Rows[i]["BLOG_CAT_DESC"].ToString().Replace(" ", "") + "/" + Encrypted.EncryptASCII(dt.Rows[i]["MAST_BLOG_CATEGORY_KEY"].ToString()) + "'>" + dt.Rows[i]["BLOG_CAT_DESC"] + "</a></li>";
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
            String search = Convert.ToString(form["txt_Search"]);
            DataTable dt2 = new DataTable();
            string blogheading;
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
                    vString += "<div class='sec1'><img src='" + ConfigurationManager.AppSettings["BLOG"].ToString() + dt2.Rows[i]["BLOG_IMAGE"] + "' alt='" + dt2.Rows[0]["BLOG_HEADING"] + "' />"

                        + "<div class='wrap'><h3><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt2.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "'>" + dt2.Rows[i]["BLOG_HEADING"] + "</a></h3><h4>Posted on " + dt2.Rows[i]["BLOG_DATE"] + "</h4>"
                        + "<p>" + dt2.Rows[i]["DESCRIPTION"] + "</p><a href='" + "/BlogUI/Index/" + Encrypted.EncryptASCII(dt2.Rows[i]["MAST_BLOG_KEY"].ToString()) + "/" + blogheading + "' class='more-but'>"
                        + "Read More</a></div></div>";
                }
                ViewBag.blogRender = vString;
                Allcatagory();
            }
            else
            {
                ViewBag.blogRender = "<h2>No search result found</h2>";
                Allcatagory();
            }
            return View("Index");

        }
    }
}