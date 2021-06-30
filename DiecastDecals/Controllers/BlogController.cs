using MyApp.Entity;
using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using MyApp.Entity.Message;

namespace DiecastDecals.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog

        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;
        DataTable dt1;
        DataTable dt2;

        public ActionResult Index()
        {
            try
            {
                if (Session["oSysUser"] != null)
                {
                    oSysUser = (EntitySysUser)Session["oSysUser"];
                    oSysUser.USER_KEY = Convert.ToInt32(Session["USER_KEY"]);

                    oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                    errMsg = String.Empty;
                    
                    dt=FillBlogGrid();
                    dt2=FillDdBlogCategory();
                    if (!String.IsNullOrEmpty(errMsg))
                    {
                        TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrDdlPop + "');");
                        //  MessageBox(1, "Activity " + Message.msgErrDdlPop, errMsg);
                    }
                    ViewBag.JavaScriptFunction = string.Format("OpenTab1();");


                }
                else
                {
                    return Redirect("~/Admin/Index");
                }

            }

            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                // MessageBox(1, Message.msgErrCommon, ex.Message);
            }
            return View(dt.Rows);
        }

        [HttpPost]
        public ActionResult Add()
        {
            try
            {
                // errMsg = String.Empty;
                ViewBag.hf_MAST_BLOG_KEY = "0";

                ViewBag.JavaScriptFunction = string.Format("OpenTab2();");
                ViewBag.img_Blog = "/Content/admin-assets/images/no_image.jpg";
                dt = FillBlogGrid();
                
                FillDdBlogCategory();
                //TempData["JavaScriptFunction"] = string.Format("OpenTab2();");
                //ClearControl();
                //MessageBox(2, "", "");
            }
            catch (Exception ex)
            {
                //  MessageBox(1, Message.msgErrCommon, ex.Message);
                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgErrCommon + "');");
            }

            return View("Index", dt.Rows);
        }

        private DataTable FillBlogGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                
                using (BABlog oBMC = new BABlog())
                {
                    dt = oBMC.Get<Int32>("ALL", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                ViewData["dt"] = dt;
            }
            catch (Exception ex)
            {
                // return ex.Message;
            }
            return dt;

        }

        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            try
            {
                string edit = form[0];
                errMsg = String.Empty;

                ViewBag.hf_MAST_BLOG_KEY = edit.ToString();
                errMsg = FillBlogEdit(Convert.ToInt32(edit));
                FillBlogGrid();
                dt2=FillDdBlogCategory();
                if (String.IsNullOrEmpty(errMsg))
                {
                    //  aPageName.InnerText = Message.modName21 + "(Edit)";
                    ViewBag.JavaScriptFunction = string.Format("OpenTab2();");
                }
                else
                {
                    //   MessageBox(1, Message.msgErrEditPop, errMsg);
                    TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgErrEditPop + "');");
                }

            }
            catch (Exception ex)
            {
                //  MessageBox(1, Message.msgErrCommon, ex.Message);
                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgErrCommon + "');");
            }

            return View("Index", dt.Rows);
        }

        private String FillBlogEdit(Int32 pMAST_BLOG_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt = new DataTable();
                using (BABlog oBMG = new BABlog())
                {
                    dt = oBMG.Get<Int32>("SRH_KEY", pMAST_BLOG_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.txt_BLOG_HEADING = Convert.ToString(dt.Rows[0]["BLOG_HEADING"]);
                    ViewBag.txt_BLOG_DATE = Convert.ToDateTime(dt.Rows[0]["BLOG_DATE"]).ToString("d");
                    ViewBag.img_Blog = ConfigurationManager.AppSettings["BLOG"].ToString() + Convert.ToString(dt.Rows[0]["BLOG_IMAGE"]);
                    ViewBag.BLOG_IMAGE = Convert.ToString(dt.Rows[0]["BLOG_IMAGE"]);
                    ViewBag.txt_BLOG_QUESTION = Convert.ToString(dt.Rows[0]["BLOG_QUESTION"]);
                    ViewBag.txt_DESCRIPTION = Convert.ToString(dt.Rows[0]["DESCRIPTION"]);
                    ViewBag.txt_META_DESC = Convert.ToString(dt.Rows[0]["META_DESC"]);
                    ViewBag.txt_META_KEYWORD = Convert.ToString(dt.Rows[0]["META_KEYWORD"]);

                    ViewBag.ddl_MAST_BLOG_CATEGORY_KEY = Convert.ToString(dt.Rows[0]["MAST_BLOG_CATEGORY_KEY"]);
                    ViewBag.hf_MAST_BLOG_CATEGORY_KEY = Convert.ToString(dt.Rows[0]["MAST_BLOG_CATEGORY_KEY"]);
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
        public ActionResult delete(FormCollection form)
        {
            Int32 vKey = 0; Byte vRef = 0; String vDelMsg = String.Empty;
            EntityBlog oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string edit = form[0];
                    errMsg = String.Empty;
                    oBMAST = new EntityBlog();
                    //GridViewRow gvr = (GridViewRow)((HtmlAnchor)sender).NamingContainer;
                    oBMAST.MAST_BLOG_KEY = Convert.ToInt32(edit);
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_DELETE = 0;

                    using (BABlog oBMC = new BABlog())
                    {
                        vRef = oBMC.SaveDelete<Object, Int32>("DELETE", oBMAST, null, ref vKey, ref errMsg, ref vDelMsg, "2019", 1);
                        if (vRef > 0)
                        {
                            if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                //  MessageBox(1, "Data exists in We Do Master", errMsg);
                            }
                            else if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDelete + "');");
                                return Redirect("~/Blog/Index");

                                //MessageBox(1, Message.msgSaveDelete, "");
                            }
                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                        }
                    }
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgPageInvalid + "');");
                    //   MessageBox(2, Message.msgPageInvalid, "");
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

            return Redirect("~/Blog/Index");
        }

        private String ProcessBlogImage(ref String Doc_Name, HttpPostedFileBase file)
        {


            if (file == null)
            {
                ViewBag.BLOG_IMAGE = Request["hf_BLOG_IMAGE"];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["BLOG"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg" };
                    var Image_url = file.ToString(); //getting complete url
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss")  + ext; //appending the name with id  
                                                                                                      // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.BLOG_IMAGE = myfile;
                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                    }
                    return String.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    ImageAcceptedExtensions = null;
                }
            }
        }

        private DataTable FillDdBlogCategory()
        {
            try
            {
                errMsg = String.Empty;

                using (BABlogCat oBMC = new BABlogCat())
                {
                    dt2 = oBMC.BindLB(0, ref errMsg, null, 0);

                }
                List<EntityBlogCat> page = new List<EntityBlogCat>();
                if (dt2.Rows.Count > 0)
                {
                    EntityBlogCat oBmast = new EntityBlogCat();
                    oBmast.MAST_BLOG_CATEGORY_KEY = 0;
                    oBmast.BLOG_CAT_DESC = "-- Select Blog Category --";
                    page.Add(oBmast);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        oBmast = new EntityBlogCat();
                        oBmast.MAST_BLOG_CATEGORY_KEY = Convert.ToInt32(dt2.Rows[i]["MAST_BLOG_CATEGORY_KEY"]);
                        oBmast.BLOG_CAT_DESC = dt2.Rows[i]["BLOG_CAT_DESC"].ToString();

                        page.Add(oBmast);

                    }

                    var getblogcat = page.ToList();

                    SelectList list = new SelectList(getblogcat, "MAST_BLOG_CATEGORY_KEY", "BLOG_CAT_DESC");
                    ViewBag.BlogCategoryName = list;


                    if (ViewBag.ddl_MAST_BLOG_CATEGORY_KEY != null)
                    {


                        ViewBag.ddl_MAST_BLOG_CATEGORY_KEY = lbSelection(ViewBag.ddl_MAST_BLOG_CATEGORY_KEY);
                    }
                }
                ViewData["dt2"] = dt2;
                FillBlogGrid();
                return dt2;
            }
            catch (Exception ex)
            {
                return dt2;
            }
        }

        public ActionResult btn_Blog_Save_Click(FormCollection form, HttpPostedFileBase fu_Blog)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String LABELS = String.Empty;
            EntityBlog oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    errMsg = ProcessBlogImage(ref LABELS, fu_Blog);
                    if (!String.IsNullOrEmpty(errMsg))
                    {
                        TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgErr401 + "');");
                        //MessageBox(3, errMsg, "");
                        //return;
                    }
                    errMsg = String.Empty;
                    oBMAST = new EntityBlog();
                    

                    oBMAST.MAST_BLOG_KEY = Convert.ToInt32(form["hf_MAST_BLOG_KEY"]);
                    DateTime dtBlog_Date = Convert.ToDateTime(form["txt_BLOG_DATE"].Trim());
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    oBMAST.BLOG_DATE = Convert.ToDateTime(dtBlog_Date);
                    oBMAST.BLOG_HEADING = Convert.ToString(form["txt_BLOG_HEADING"] == "" ? null : form["txt_BLOG_HEADING"]);
                    oBMAST.BLOG_QUESTION = Convert.ToString(form["txt_BLOG_QUESTION"] == "" ? null : form["txt_BLOG_QUESTION"].ToString());
                    oBMAST.DESCRIPTION = Convert.ToString(form["txt_DESCRIPTION.Text)"] == "" ? null : form["txt_DESCRIPTION"].ToString());
                    oBMAST.META_DESC = Convert.ToString(form["txt_META_DESC"] == "" ? null : form["txt_META_DESC"].ToString());
                    oBMAST.META_KEYWORD = Convert.ToString(form["txt_META_KEYWORD"] == "" ? null : form["txt_META_KEYWORD"].ToString());
                    oBMAST.MAST_BLOG_TAG_KEY = "0";//String.IsNullOrEmpty(LB_MAST_BLOG_TAG_KEY.SelectedValue.Trim()) ? "" : hf_MAST_BLOG_TAG_KEY"];
                    
                    //oBMAST.MAST_BLOG_CATEGORY_KEY = String.IsNullOrEmpty(form["ddl_MAST_BLOG_CATEGORY_KEY"].Trim()) ? "" : form["ddl_MAST_BLOG_CATEGORY_KEY"];
                    
                    oBMAST.MAST_BLOG_CATEGORY_KEY = string.Join(",", form["ddl_MAST_BLOG_CATEGORY_KEY"]);


                    oBMAST.BLOG_IMAGE = ViewBag.BLOG_IMAGE;

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 0;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BABlog oBMC = new BABlog())
                    {
                        if (oBMAST.MAST_BLOG_KEY == 0)
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                dt2 = FillDdBlogCategory();
                               
                                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgSaveNew + "');");
                                FillBlogGrid();
                                //MessageBox(2, Message.msgSaveNew, "");
                                //errMsg = FillMastBenefitGrid();
                                //hf_MAST_BENEFITS_KEY"] = Convert.ToString(vKey);
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgValidation + "');");
                                //  MessageBox(2, Message.msgValidation, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgSaveErr + "');");
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }

                        }
                        else
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                dt2 = FillDdBlogCategory();
                                
                                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgSaveEdit + "');");
                                FillBlogGrid();
                                // MessageBox(2, Message.msgSaveEdit, "");
                                // FillMastBenefitEdit(oBMAST.MAST_BENEFITS_KEY);
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgSaveDuplicate + "');");
                                //  MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgSaveErr + "');");
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }

                        }
                    }
                    //  oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgPageInvalid + "');");
                    //  MessageBox(2, Message.msgPageInvalid, "");
                }

            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgErrCommon + "');");
                //  MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

            return Redirect("~/Blog/Index");

        }



        private List<string> lbSelection(String value)
        {
            String[] sepValues = value.Split(',');
            List<string> list = new List<string>();
            foreach (String val in sepValues)
            {
                list.Add(val);
            }
            return list;
        }

    }
}