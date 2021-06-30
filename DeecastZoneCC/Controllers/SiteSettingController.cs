using MyApp.db.MyAppBal;
using MyApp.Entity;
using MyApp.Entity.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class SiteSettingController : Controller
    {
        // GET: SiteSetting

        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;  //pagesetting
        DataTable dt1; //sitesetting
        DataTable dt2; //pagesetting
        DataTable dt3; //links
        DataTable dt4;  //links
        DataTable dt5; //testi
        DataTable dt6; //testi

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
                    FillSiteSettingEdit();
                    FillLinkGrid();
                    FillTestimonialGrid();
                    errMsg = FillDdPageName();
                    if (!String.IsNullOrEmpty(errMsg))
                    {
                        TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrDdlPop + "');");
                        //  MessageBox(1, "Page Name" + Message.msgErrDdlPop, errMsg);
                    }
                    dt = FillPageSettingGrid();
                    
                    ViewBag.hf_DTLS_PAGE_SETTING_KEY = "0";



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

        #region Site Setting
        private String ProcessHeaderLogo(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.HEADER_LOGO = Request["hf_WEBSITE_LOGO"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["HOME"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg" };


                    var Image_url = file.ToString(); //getting complete url
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-A" + ext; //appending the name with id  
                                                                                               // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.HEADER_LOGO = myfile;

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
        private String ProcessFooterLogo(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.BANNER_IMAGE = Request["hf_BANNER_IMAGE"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["HOME"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg" };


                    var Image_url = file.ToString(); //getting complete url
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-B" + ext; //appending the name with id  
                                                                                               // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.BANNER_IMAGE = myfile;

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

        [HttpPost]
        public ActionResult btn_Head_Save_Click(FormCollection form, HttpPostedFileBase fu_Logo, HttpPostedFileBase fu_BANNER_IMAGE)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String HEADER_LOGO = String.Empty;
            String FOOTER_LOGO = String.Empty;

            EntitySiteSetting oBMAST = null;

            try
            {
                if (ModelState.IsValid)
                {
                    errMsg = ProcessHeaderLogo(ref HEADER_LOGO, fu_Logo);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        errMsg = ProcessFooterLogo(ref FOOTER_LOGO, fu_BANNER_IMAGE);
                        if (!String.IsNullOrEmpty(errMsg))
                        {
                            TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                            // MessageBox(3, errMsg, "");
                            //return;
                        }

                    }
                    errMsg = String.Empty;
                    oBMAST = new EntitySiteSetting();

                    oBMAST.CONTACT_NO = form["txt_CONTACT_NO"];
                    oBMAST.MAIL = form["txt_MAIL"];
                    oBMAST.ADDRESS = form["txt_ADDRESS"];                   

                    oBMAST.FACEBOOK_LINK = form["txt_FACEBOOK_LINK"];
                    oBMAST.TWITTER_LINK = form["txt_TWITTER_LINK"];
                    
                    oBMAST.INSTAGRAM_LINK = form["txt_INSTAGRAM_LINK"];

                    oBMAST.LARGE_VIDEO_LINK = form["txt_LARGE_VIDEO"];
                    oBMAST.FOOTER_NOTE = form["txt_FOOTER_NOTE"];

                    oBMAST.BANNER_VIDEO_LINK = form["txt_BANNER_VIDEO_LINK"];
                    oBMAST.BANNER_DESC = form["txt_BANNER_DESC"];

                    oBMAST.HEADER_LOGO = ViewBag.HEADER_LOGO;
                    oBMAST.BANNER_IMAGE = ViewBag.BANNER_IMAGE;


                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                    {
                        vRef = oBMC.HomeSaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                        if (vRef == 1)
                        {
                            TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                            //MessageBox(2, Message.msgSaveEdit, "");
                            FillSiteSettingEdit();
                        }
                        else if (vRef == 2)
                        {
                            TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                            //  MessageBox(2, Message.msgSaveDuplicate, errMsg);
                        }

                        else
                        {
                            TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                            //   MessageBox(2, Message.msgSaveErr, errMsg);
                        }

                    }
                }
                // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                //   MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

            return Redirect("~/SiteSetting/Index");
        }

        private String FillSiteSettingEdit()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt1 = new DataTable();
                using (BAHeadSiteSetting oBME = new BAHeadSiteSetting())
                {
                    dt1 = oBME.GetHomeSetting("GET", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.txt_CONTACT_NO = Convert.ToString(dt1.Rows[0]["CONTACT_NO"]);
                    ViewBag.txt_MAIL = Convert.ToString(dt1.Rows[0]["MAIL_ID"]);
                    ViewBag.txt_ADDRESS = Convert.ToString(dt1.Rows[0]["OFFICE_ADDRESS"]);

                    ViewBag.txt_FACEBOOK_LINK = Convert.ToString(dt1.Rows[0]["FACEBOOK_LINK"]);
                    ViewBag.txt_TWITTER_LINK = Convert.ToString(dt1.Rows[0]["TWITTER_LINK"]);
                    
                    ViewBag.txt_INSTAGRAM_LINK = Convert.ToString(dt1.Rows[0]["INSTA_LINK"]);

                    ViewBag.txt_FOOTER_NOTE = Convert.ToString(dt1.Rows[0]["FOOTER_NOTE"]);


                    ViewBag.txt_LARGE_VIDEO_LINK = Convert.ToString(dt1.Rows[0]["LARGE_VIDEO_LINK"]);

                    ViewBag.txt_BANNER_VIDEO_LINK = Convert.ToString(dt1.Rows[0]["BANNER_VIDEO_LINK"]);

                    ViewBag.txt_BANNER_DESC = Convert.ToString(dt1.Rows[0]["BANNER_DESC"]);



                    ViewBag.img_WEBSITE_LOGO = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["WEBSITE_LOGO"]);
                    ViewBag.hf_WEBSITE_LOGO = Convert.ToString(dt1.Rows[0]["WEBSITE_LOGO"]);
                    ViewBag.img_BANNER_IMAGE = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["BANNER_IMAGE"]);
                    ViewBag.hf_BANNER_IMAGE = Convert.ToString(dt1.Rows[0]["BANNER_IMAGE"]);
                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                //dt1 = null;
            }
        }
        #endregion

        #region Page Setting

        private String FillDdPageName()
        {
            try
            {
                errMsg = String.Empty;

                using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                {
                    dt2 = oBMC.BindDdl(0, ref errMsg, null, 0);

                }
                List<EntitySiteSetting> page = new List<EntitySiteSetting>();
                if (dt2 != null || dt2.Rows.Count > 0)
                {
                    EntitySiteSetting oBmast = new EntitySiteSetting();
                    oBmast.MAST_PAGE_KEY = 0;
                    oBmast.PAGE_NAME = "-SELECT AN OPTION-";
                    page.Add(oBmast);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        oBmast = new EntitySiteSetting();
                        oBmast.MAST_PAGE_KEY = Convert.ToInt32(dt2.Rows[i]["MAST_PAGE_KEY"]);
                        oBmast.PAGE_NAME = dt2.Rows[i]["PAGE_NAME"].ToString();

                        page.Add(oBmast);

                    }

                    var getpagename = page.ToList();

                    SelectList list = new SelectList(getpagename, "MAST_PAGE_KEY", "PAGE_NAME", ViewBag.ddl_MAST_PAGE_KEY);
                    ViewBag.PageName = list;

                }
                else
                {
                    FillSiteSettingEdit();
                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private DataTable FillPageSettingGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                {
                    dt = oBMC.GetPageSetting<Int32>("GET", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }

            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            string edit = form[0];

            try
            {
                EntitySiteSetting oBMAST = null;
                errMsg = String.Empty;
                oBMAST = new EntitySiteSetting();
                oBMAST.DTLS_PAGE_SETTING_KEY = Convert.ToInt32(edit);
                ViewBag.hf_DTLS_PAGE_SETTING_KEY = edit;

                errMsg = FillPageSettingEdit(Convert.ToInt32(edit));
                if (String.IsNullOrEmpty(errMsg))
                {
                    FillSiteSettingEdit();
                    errMsg = FillDdPageName();
                    if (!String.IsNullOrEmpty(errMsg))
                    {
                        TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrDdlPop + "');");
                        //  MessageBox(1, "Page Name" + Message.msgErrDdlPop, errMsg);
                    }
                    dt = FillPageSettingGrid();
                    FillLinkGrid();
                    FillTestimonialGrid();
                    FillPageSettingGrid();
                    FillDdPageName();
                    // aPageName.InnerText = Message.modName21 + "(Edit)";
                    // MessageBox(2, "", "");
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrEditPop + "');");
                    //   MessageBox(1, Message.msgErrEditPop, errMsg);
                }

            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //  MessageBox(1, Message.msgErrCommon, ex.Message);
            }
            return View("Index", dt.Rows);




        }

        private String FillPageSettingEdit(Int32 pDTLS_PAGE_SETTING_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAHeadSiteSetting oBMG = new BAHeadSiteSetting())
                {
                    dt = oBMG.GetPageSetting<Int32>("SRH_KEY", pDTLS_PAGE_SETTING_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    //  ddl_MAST_PAGE_KEY.SelectedIndex = ddl_MAST_PAGE_KEY.Items.IndexOf(ddl_MAST_PAGE_KEY.Items.FindByValue(Convert.ToString(dt.Rows[0]["MAST_PAGE_KEY"])));

                    ViewBag.ddl_MAST_PAGE_KEY = Convert.ToString(dt.Rows[0]["MAST_PAGE_KEY"]);
                    ViewBag.txt_PAGE_TITLE = Convert.ToString(dt.Rows[0]["PAGE_TITLE"]);
                    ViewBag.txt_META_DESCRIPTION = Convert.ToString(dt.Rows[0]["META_DESCRIPTION"]);
                    ViewBag.txt_META_KEYWORD = Convert.ToString(dt.Rows[0]["META_KEYWORD"]);

                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }


        [HttpPost]
        public ActionResult btn_Page_Settings_Save_Click(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            EntitySiteSetting oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    errMsg = String.Empty;
                    oBMAST = new EntitySiteSetting();
                    oBMAST.DTLS_PAGE_SETTING_KEY = Convert.ToInt32(form["hf_DTLS_PAGE_SETTING_KEY"]);
                    oBMAST.MAST_PAGE_KEY = Convert.ToInt32(form["ddl_MAST_PAGE_KEY"]);
                    oBMAST.PAGE_TITLE = form["txt_PAGE_TITLE"];
                    oBMAST.META_DESCRIPTION = form["txt_META_DESCRIPTION"];
                    oBMAST.META_KEYWORD = form["txt_META_KEYWORD"];

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 0;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                    {
                        if (oBMAST.DTLS_PAGE_SETTING_KEY == 0)
                        {
                            vRef = oBMC.SavePageSetting<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                //MessageBox(2, Message.msgSaveNew, "");
                                FillPageSettingGrid();
                                ClearPageSetting();
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                //  MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }

                        }
                        else
                        {
                            vRef = oBMC.SavePageSetting<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                                FillPageSettingGrid();
                                ClearPageSetting();
                                //MessageBox(1, Message.msgSaveEdit, "");
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                //  MessageBox(1, Message.msgSaveDuplicate, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                //  MessageBox(1, Message.msgSaveErr, errMsg);
                            }

                        }
                    }
                }
                // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //   MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

            return Redirect("~/SiteSetting/Index");

        }

        private void ClearPageSetting()
        {

            ViewBag.ddl_MAST_PAGE_KEY = "0";
            ViewBag.txt_PAGE_TITLE = "";
            ViewBag.txt_META_DESCRIPTION = "";
            ViewBag.txt_META_KEYWORD = "";
            ViewBag.hf_DTLS_PAGE_SETTING_KEY = "0";
        }
        #endregion

        #region Useful Links

        private void ClearUsefulLinks()
        {
            ViewBag.txt_TITLE = "";
            ViewBag.txt_LINK = "";
            ViewBag.hf_DTLS_USEFULL_LINKS_KEY = "0";

        }


        [HttpPost]
        public ActionResult btn_Link_Save_Click(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            EntitySiteSetting oBMAST = null;

            try
            {
                if (ModelState.IsValid)
                {
                    
                    errMsg = String.Empty;
                    oBMAST = new EntitySiteSetting();

                    oBMAST.DTLS_USEFULL_LINKS_KEY = Convert.ToInt32(form["hf_DTLS_USEFULL_LINKS_KEY"]==""? "0": form["hf_DTLS_USEFULL_LINKS_KEY"]);
                    oBMAST.TITLE = form["txt_TITLE"];
                    oBMAST.LINK = form["txt_LINK"];
                    
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                    {
                        if (oBMAST.DTLS_USEFULL_LINKS_KEY == 0)
                        {
                            vRef = oBMC.SaveLink<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                //MessageBox(2, Message.msgSaveNew, "");
                                ClearUsefulLinks();
                                FillLinkGrid();
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                //MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }
                            
                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                //MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                              
                        }
                        else
                        {
                            vRef = oBMC.SaveLink<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                                FillLinkGrid();
                                ClearUsefulLinks();
                                //MessageBox(1, Message.msgSaveEdit, "");
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                // MessageBox(1, Message.msgSaveDuplicate, errMsg);
                            }
                               
                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                //MessageBox(1, Message.msgSaveErr, errMsg);
                            }
                              
                        }

                    }
                }
                // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //   MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

            return Redirect("~/SiteSetting/Index");
        }

        private DataTable FillLinkGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt3 = new DataTable();
                using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                {
                    dt3 = oBMC.Get<Int32>("ALL", 0, "", ref errMsg, "2019", 1);
                    TempData["modelLink"] = dt3;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }              
               
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
            return dt3;
        }


        [HttpPost]
        public ActionResult editlink(FormCollection form)
        {
            string editlink = form[0];

            try
            {
                EntitySiteSetting oBMAST = null;
                errMsg = String.Empty;
                oBMAST = new EntitySiteSetting();
                oBMAST.DTLS_USEFULL_LINKS_KEY = Convert.ToInt32(editlink);
                ViewBag.hf_DTLS_USEFULL_LINKS_KEY = editlink;

                errMsg = FillLinkEdit(Convert.ToInt32(editlink));
                if (String.IsNullOrEmpty(errMsg))
                {
                    dt3 = FillLinkGrid();
                    FillPageSettingGrid();
                    FillTestimonialGrid();
                    FillSiteSettingEdit();
                    FillDdPageName();
                    // aPageName.InnerText = Message.modName21 + "(Edit)";
                    // MessageBox(2, "", "");
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrEditPop + "');");
                    //   MessageBox(1, Message.msgErrEditPop, errMsg);
                }

            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //  MessageBox(1, Message.msgErrCommon, ex.Message);
            }
            return View("Index", dt.Rows);
        }

        private String FillLinkEdit(Int32 pDTLS_USEFULL_LINKS_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt4 = new DataTable();
                using (BAHeadSiteSetting oBMG = new BAHeadSiteSetting())
                {
                    dt4 = oBMG.GetUsefulLinks<Int32>("SRH_KEY", pDTLS_USEFULL_LINKS_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt4 != null && dt4.Rows.Count > 0)
                {                    
                    ViewBag.txt_TITLE = Convert.ToString(dt4.Rows[0]["TITLE"]);
                    ViewBag.txt_LINK = Convert.ToString(dt4.Rows[0]["LINK"]);

                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }

        [HttpPost]
        public ActionResult deletelink(FormCollection form)
        {
            string edit = form[0];
            try
            {
                errMsg = DeleteLink(Convert.ToInt32(edit));

                if (String.IsNullOrEmpty(errMsg))
                {
                    //  MessageBox(1, Message.msgSaveDelete, "");
                    dt3 = FillLinkGrid();
                    FillPageSettingGrid();
                    FillTestimonialGrid();
                    FillSiteSettingEdit();
                    FillDdPageName();
                    return Redirect("~/SiteSetting/Index");
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                    //  MessageBox(3, Message.msgErrCommon, errMsg);
                }


            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //  MessageBox(3, Message.msgErrCommon, ex.Message);
            }
            return View("Index", dt.Rows);
        }

        private String DeleteLink(Int32 pDTLS_USEFULL_LINKS_KEY)
        {
            try
            {
                errMsg = String.Empty;
                Int32 vKey = 0;

                using (BAHeadSiteSetting oBHH = new BAHeadSiteSetting())
                {
                    oBHH.Delete<Int32>("DELETE", pDTLS_USEFULL_LINKS_KEY, ref vKey, ref errMsg, 0, null, 1);
                }

                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Testimonials

        private void ClearTestimonial()
        {

            ViewBag.txt_CLIENT_NAME = "";
            ViewBag.txt_CLIENT_FEEDBACK= "";
            ViewBag.img_CLIENT_IMAGE = "/Content/admin-assets/images/no_image.jpg";
            ViewBag.CLIENT_IMAGE = "0";
            ViewBag.hf_MAST_TESTIMONIALS_KEY = "0";

        }

        private String ProcessTestimonialImage(ref String Doc_Name, HttpPostedFileBase file)
        {
            
            if (file == null)
            {
                ViewBag.CLIENT_IMAGE = Request["hf_CLIENT_IMAGE"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["CLIENT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg" };


                    var Image_url = file.ToString(); //getting complete url
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-C" + ext; //appending the name with id  
                                                                                                      // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;
                        var imgpath=  ConfigurationManager.AppSettings["CLIENT"].ToString() + myfile;
                        file.SaveAs(path);

                        ViewBag.CLIENT_IMAGE = imgpath;

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

        [HttpPost]
        public ActionResult btn_Testimonial_Save_Click(FormCollection form, HttpPostedFileBase fu_CLIENT_IMAGE)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String CLIENT_IMAGE = String.Empty;
            EntitySiteSetting oBMAST = null;
            
            try
            {
                if (ModelState.IsValid)
                {


                    errMsg = ProcessTestimonialImage(ref CLIENT_IMAGE, fu_CLIENT_IMAGE);
                    if (!String.IsNullOrEmpty(errMsg))
                    {
                        //MessageBox(1, errMsg, "");
                        return Redirect("~/SiteSetting/Index");
                    }

                    errMsg = String.Empty;
                    oBMAST = new EntitySiteSetting();
                    oBMAST.MAST_TESTIMONIALS_KEY = Convert.ToInt32(form["hf_MAST_TESTIMONIALS_KEY"] == "" ? "0" : form["hf_MAST_TESTIMONIALS_KEY"]);
                    oBMAST.CLIENT_NAME = form["txt_CLIENT_NAME"];
                    oBMAST.CLIENT_FEEDBACK = form["txt_CLIENT_FEEDBACK"];
                    oBMAST.CLIENT_IMAGE = ViewBag.CLIENT_IMAGE;
                    

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                    {
                        if (oBMAST.MAST_TESTIMONIALS_KEY == 0)
                        {
                            vRef = oBMC.SaveTestimonial<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                ClearTestimonial();
                                //MessageBox(2, Message.msgSaveNew, "");
                                FillTestimonialGrid();
                                FillPageSettingGrid();
                                FillSiteSettingEdit();
                                FillLinkGrid();
                                FillDdPageName();
                                return Redirect("~/SiteSetting/Index");
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                //MessageBox(2, Message.msgSaveDuplicate, errMsg);
                                return Redirect("~/SiteSetting/Index");
                            }
                                
                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                //MessageBox(2, Message.msgSaveErr, errMsg);
                                return Redirect("~/SiteSetting/Index");
                            }
                                
                        }
                        else
                        {
                            vRef = oBMC.SaveTestimonial<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                ClearTestimonial();
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                                //MessageBox(2, Message.msgSaveEdit, "");
                                FillTestimonialGrid();
                                FillPageSettingGrid();
                                FillSiteSettingEdit();
                                FillLinkGrid();
                                FillDdPageName();
                                return Redirect("~/SiteSetting/Index");
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                return Redirect("~/SiteSetting/Index");
                                //MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                return Redirect("~/SiteSetting/Index");
                                //MessageBox(2, Message.msgSaveErr, errMsg);
                            }

                        }


                    }

                    // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                    //ClearControl();
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                    //MessageBox(3, errMsg, "");
                    //return Redirect("~/SiteSetting/Index");
                }
                return Redirect("~/SiteSetting/Index");
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }
            return View("Index", dt.Rows);
        }


        private DataTable FillTestimonialGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt5 = new DataTable();
                using (BAHeadSiteSetting oBMC = new BAHeadSiteSetting())
                {
                    dt5 = oBMC.GetTestimonial<Int32>("GET", 0, "", ref errMsg, "2019", 1);
                    TempData["clientmodel"]= dt5;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                ViewData["dt5"] = dt5;
                return dt5;
                //return errMsg;
            }
            catch (Exception ex)
            {
                return dt5;
            }

        }

        [HttpPost]
        public ActionResult edittestimonials(FormCollection form)
        {
            string edit = form[0];
            try
            {
                EntitySiteSetting oBMAST = new EntitySiteSetting();
                errMsg = String.Empty;
                oBMAST.MAST_TESTIMONIALS_KEY = Convert.ToInt32(edit);
                ViewBag.hf_MAST_TESTIMONIALS_KEY = edit;

                errMsg = FillTestimonialEdit(Convert.ToInt32(edit));
                if (String.IsNullOrEmpty(errMsg))
                {

                    dt5 = FillTestimonialGrid();
                    FillSiteSettingEdit();
                    FillPageSettingGrid();
                    FillLinkGrid();
                    FillDdPageName();
                    // aPageName.InnerText = Message.modName21 + "(Edit)";
                    // MessageBox(2, "", "");
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrEditPop + "');");
                    // MessageBox(1, Message.msgErrEditPop, errMsg);
                }
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //MessageBox(1, Message.msgErrCommon, ex.Message);
            }
            return View("Index", dt.Rows);
        }

        private String FillTestimonialEdit(Int32 pMAST_TESTIMONIALS_KEY)
        {
            try
            {
                EntitySiteSetting oBMAST = null;
                errMsg = String.Empty;
                dt6 = new DataTable();
                
                using (BAHeadSiteSetting oBMG = new BAHeadSiteSetting())
                {
                    dt6 = oBMG.GetTestimonial<Int32>("SRH_KEY", pMAST_TESTIMONIALS_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt6 != null && dt6.Rows.Count > 0)
                {
                    ViewBag.txt_CLIENT_NAME = Convert.ToString(dt6.Rows[0]["CLIENT_NAME"]);
                    ViewBag.txt_CLIENT_FEEDBACK = Convert.ToString(dt6.Rows[0]["CLIENT_FEEDBACK"]);

                    ViewBag.img_CLIENT_IMAGE =  Convert.ToString(dt6.Rows[0]["CLIENT_IMAGE"]); 
                    ViewBag.CLIENT_IMAGE = Convert.ToString(dt6.Rows[0]["CLIENT_IMAGE"]);

                    // oBMAST.QUICK_FACT_DESC = Convert.ToString(dt.Rows[0]["QUICK_FACT_DESC"]);


                }
                return errMsg;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        [HttpPost]
        public ActionResult deletetestimonials(FormCollection form)
        {
            string edit = form[0];
            try
            {
                errMsg = GridTestimonialDelete(Convert.ToInt32(edit));

                if (String.IsNullOrEmpty(errMsg))
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDelete + "');");
                    //  MessageBox(1, Message.msgSaveDelete, "");
                    dt5 = FillTestimonialGrid();
                    FillPageSettingGrid();
                    FillLinkGrid();
                    FillSiteSettingEdit();
                    FillDdPageName();
                    return Redirect("~/SiteSetting/Index");


                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                    //  MessageBox(3, Message.msgErrCommon, errMsg);
                }


            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //  MessageBox(3, Message.msgErrCommon, ex.Message);
            }
            return View("Index", dt.Rows);
        }

        private String GridTestimonialDelete(Int32 pMAST_TESTIMONIALS_KEY)
        {
            try
            {
                errMsg = String.Empty;
                Int32 vKey = 0;

                using (BAHeadSiteSetting oBHH = new BAHeadSiteSetting())
                {
                    oBHH.DeleteTestimonial<Int32>("DELETE", pMAST_TESTIMONIALS_KEY, ref vKey, ref errMsg, 0, null, 1);
                }

                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
    }
}