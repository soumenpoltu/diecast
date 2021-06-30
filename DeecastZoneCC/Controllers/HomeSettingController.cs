using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.Entity;
using System.Data;
using MyApp.db.MyAppBal;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using MyApp.Entity.Message;

namespace DeecastZoneCC.Controllers
{
    public class HomeSettingController : Controller
    {
        // GET: HomeSetting
        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;
        DataTable dt1;
        public ActionResult Index()
        {

            if (Session["oSysUser"] != null)
            {
                oSysUser = (EntitySysUser)Session["oSysUser"];
                oSysUser.USER_KEY = Convert.ToInt32(Session["USER_KEY"]);

                oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                errMsg = String.Empty;
                FillHomeSettingEdit();
                FillDdProductCat(0);
                //ViewBag.QUICK_FACT_KEY = "0";
                dt = FillProductTableGrid();
                ViewBag.hf_BANNER_KEY = "0";
                ViewBag.BANNER_IMG = "../Content/admin-assets/images/no_image.jpg";
                return View(dt.Rows);

            }
            else
            {
                return Redirect("~/Admin/Index");
            }
            //return View();
        }


        #region homesetting
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
                    ViewBag.ICON_IMG_1 = Convert.ToString(dt1.Rows[0]["ICON_IMG_1"]);
                    ViewBag.ICON_IMG_2 = Convert.ToString(dt1.Rows[0]["ICON_IMG_2"]);
                    ViewBag.ICON_IMG_3 = Convert.ToString(dt1.Rows[0]["ICON_IMG_3"]);
                    ViewBag.ICON_IMG_4 = Convert.ToString(dt1.Rows[0]["ICON_IMG_4"]);
                    ViewBag.ICON_IMG_5 = Convert.ToString(dt1.Rows[0]["ICON_IMG_5"]);

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

        private String ProcessIconImage1(ref String Doc_Name, HttpPostedFileBase file)
        {

            if (file == null)
            {
                ViewBag.ICON_IMG_1 = Request["hf_ICON_IMG_1"];
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
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-ICON1" + ext; //appending the name with id  
                                                                                                      // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.ICON_IMG_1 = myfile;
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

        private String ProcessIconImage2(ref String Doc_Name, HttpPostedFileBase file)
        {

            if (file == null)
            {
                ViewBag.ICON_IMG_2 = Request["hf_ICON_IMG_2"];
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
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-ICON2" + ext; //appending the name with id  
                                                                                                          // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.ICON_IMG_2 = myfile;
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

        private String ProcessIconImage3(ref String Doc_Name, HttpPostedFileBase file)
        {

            if (file == null)
            {
                ViewBag.ICON_IMG_3 = Request["hf_ICON_IMG_3"];
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
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-ICON3" + ext; //appending the name with id  
                                                                                                          // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.ICON_IMG_3 = myfile;
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

        private String ProcessIconImage4(ref String Doc_Name, HttpPostedFileBase file)
        {

            if (file == null)
            {
                ViewBag.ICON_IMG_4 = Request["hf_ICON_IMG_4"];
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
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-ICON4" + ext; //appending the name with id  
                                                                                                          // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.ICON_IMG_4 = myfile;
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

        private String ProcessIconImage5(ref String Doc_Name, HttpPostedFileBase file)
        {

            if (file == null)
            {
                ViewBag.ICON_IMG_5 = Request["hf_ICON_IMG_5"];
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
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-ICON5" + ext; //appending the name with id  
                                                                                                          // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.ICON_IMG_5 = myfile;
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
        public ActionResult btn_Head_Save_Click(FormCollection form, HttpPostedFileBase fu_Icon_Image1, HttpPostedFileBase fu_Icon_Image2, HttpPostedFileBase fu_Icon_Image3, HttpPostedFileBase fu_Icon_Image4, HttpPostedFileBase fu_Icon_Image5)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String ICON1 = String.Empty, ICON2 = String.Empty, ICON3 = String.Empty, ICON4 = String.Empty, ICON5 = String.Empty;
            EntityHomeSetting oBMAST = null;

            try
            {
                if (ModelState.IsValid)
                {

                    errMsg = ProcessIconImage1(ref ICON1, fu_Icon_Image1);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        errMsg = ProcessIconImage2(ref ICON2, fu_Icon_Image2);
                        if (String.IsNullOrEmpty(errMsg))
                        {
                            errMsg = ProcessIconImage3(ref ICON3, fu_Icon_Image3);
                            if (String.IsNullOrEmpty(errMsg))
                            {
                                errMsg = ProcessIconImage4(ref ICON4, fu_Icon_Image4);
                                if (String.IsNullOrEmpty(errMsg))
                                {
                                    errMsg = ProcessIconImage5(ref ICON5, fu_Icon_Image5);
                                    if (!String.IsNullOrEmpty(errMsg))
                                    {

                                        return Redirect("~/HomeSetting/Index");

                                    }
                                }
                            }
                        }
                    }
                    errMsg = String.Empty;
                    oBMAST = new EntityHomeSetting();

                    oBMAST.ICON_IMG_LINK_1 = form["txt_ICON_LINK1"];
                    oBMAST.ICON_IMG_LINK_2 = form["txt_ICON_LINK2"];
                    oBMAST.ICON_IMG_LINK_3 = form["txt_ICON_LINK3"];
                    oBMAST.ICON_IMG_LINK_4 = form["txt_ICON_LINK4"];
                    oBMAST.ICON_IMG_LINK_5 = form["txt_ICON_LINK5"];

                    oBMAST.ICON_IMG_1 = ViewBag.ICON_IMG_1;
                    oBMAST.ICON_IMG_2 = ViewBag.ICON_IMG_2;
                    oBMAST.ICON_IMG_3 = ViewBag.ICON_IMG_3;
                    oBMAST.ICON_IMG_4 = ViewBag.ICON_IMG_4;
                    oBMAST.ICON_IMG_5 = ViewBag.ICON_IMG_5;

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAHomeSettings oBMC = new BAHomeSettings())
                    {
                        if (oBMAST.ENT_USER_KEY == 0)
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                // MessageBox(2, Message.msgSaveNew, "");
                                FillHomeSettingEdit();

                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgValidation + "');");
                                // MessageBox(2, Message.msgValidation, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }


                        }
                        else
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                                //MessageBox(2, Message.msgSaveEdit, "");
                                FillHomeSettingEdit();
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


                    }

                    // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());

                }
                else
                {
                    //    MessageBox(3, errMsg, "");
                }

            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                // MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;

            }
            return Redirect("~/HomeSetting/Index");
        }

        #endregion

        #region productsetting

        private DataTable FillProductTableGrid()
        {
            try
            {

                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BAHomeSettings oBMC = new BAHomeSettings())
                {
                    dt = oBMC.GetBanner<Int32>("GET", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                ViewData["dt"] = dt;
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }

        }

        private String FillDdProductCat(int productid)
        {
            try
            {


                ViewBag.ddl_HEAD_PRODUCT_KEY = productid;

                errMsg = String.Empty;
                DataTable dt2 = new DataTable();
                using (BAProduct oBDTP = new BAProduct())
                {
                    dt2 = oBDTP.BindDdl(0,ref errMsg, "2019", 1);
                }
                List<EntityProduct> page = new List<EntityProduct>();
                if (dt2.Rows.Count > 0)
                {
                    EntityProduct oBmast = new EntityProduct();
                    oBmast.HEAD_PRODUCT_KEY = 0;
                    oBmast.PRODUCT_NAME = "-- Select Your Product --";
                    page.Add(oBmast);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        oBmast = new EntityProduct();
                        oBmast.HEAD_PRODUCT_KEY = Convert.ToInt32(dt2.Rows[i]["HEAD_PRODUCT_KEY"]);
                        oBmast.PRODUCT_NAME = dt2.Rows[i]["PRODUCT_NAME"].ToString();

                        page.Add(oBmast);
                        Session["packagename"] = oBmast.PRODUCT_NAME;
                    }

                    var getpackage = page.ToList();

                    SelectList list = new SelectList(getpackage, "HEAD_PRODUCT_KEY", "PRODUCT_NAME", ViewBag.ddl_HEAD_PRODUCT_KEY);
                    ViewBag.Package = list;

                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        private String ProcessBannerImage(ref String Doc_Name, HttpPostedFileBase file)
        {

            if (file == null)
            {
                ViewBag.BANNER_IMG = Request["hf_BANNER_IMG"];
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
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-BANNER" + ext; //appending the name with id  
                                                                                                          // store the file inside ~/project folder(Img)  
                        var path = Server.MapPath(DOC_PATH + myfile);
                        Image_url = path;

                        file.SaveAs(path);

                        ViewBag.BANNER_IMG = myfile;
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

        public ActionResult btn_Head_Banner_Save_Click(FormCollection form, HttpPostedFileBase fu_banner_img)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String BANNER = String.Empty;
            EntityHomeSetting oBMAST = null;

            try
            {
                if (ModelState.IsValid)
                {

                    errMsg = ProcessBannerImage(ref BANNER, fu_banner_img);
                    if (!String.IsNullOrEmpty(errMsg))
                    {
                        return Redirect("~/HomeSetting/Index");
                    }
                    errMsg = String.Empty;
                    oBMAST = new EntityHomeSetting();
                    
                    oBMAST.BANNER_KEY = Convert.ToInt32(form["hf_BANNER_KEY"]);
                    oBMAST.PRODUCT_KEY = Convert.ToInt32(form["ddl_HEAD_PRODUCT_KEY"]);
                    oBMAST.BANNER_IMG = ViewBag.BANNER_IMG;

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAHomeSettings oBMC = new BAHomeSettings())
                    {
                        if (oBMAST.BANNER_KEY == 0)
                        {
                            vRef = oBMC.SaveBannerChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                // MessageBox(2, Message.msgSaveNew, "");
                                FillHomeSettingEdit();

                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgValidation + "');");
                                // MessageBox(2, Message.msgValidation, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }


                        }
                        else
                        {
                            vRef = oBMC.SaveBannerChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                                //MessageBox(2, Message.msgSaveEdit, "");
                                FillHomeSettingEdit();
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


                    }

                    // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());

                }
                else
                {
                    //    MessageBox(3, errMsg, "");
                }

            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                // MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;

            }
            return Redirect("~/HomeSetting/Index");
        }



        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            string edit = form[0];

            try
            {

                ViewBag.hf_BANNER_KEY = edit;
                EntityHomeSetting oBMAST = null;
                errMsg = String.Empty;
                oBMAST = new EntityHomeSetting();
                oBMAST.BANNER_KEY = Convert.ToInt32(edit);
                ViewBag.PATENT_KEY = edit;

                errMsg = FillProductBannerEdit(Convert.ToInt32(edit));
                if (String.IsNullOrEmpty(errMsg))
                {
                    dt = FillProductTableGrid();
                    FillHomeSettingEdit();
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


        private String FillProductBannerEdit(Int32 pBANNER_KEY)
        {
            try
            {
                EntityHomeSetting oBMAST = null;
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAHomeSettings oBMG = new BAHomeSettings())
                {
                    dt = oBMG.GetBanner<Int32>("SRH_KEY", pBANNER_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    string HEADPRODUCTKEY = Convert.ToString(dt.Rows[0]["HEAD_PRODUCT_KEY"]);
                    FillDdProductCat( Convert.ToInt32( HEADPRODUCTKEY));
                    ViewBag.BANNER_IMG = Convert.ToString(dt.Rows[0]["BANNER_SETTING_IMG"]);
                    ViewBag.banner_img = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt.Rows[0]["BANNER_SETTING_IMG"]);

                    // oBMAST.QUICK_FACT_DESC = Convert.ToString(dt.Rows[0]["QUICK_FACT_DESC"]);


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

        private String FillDdProductCatID( string productname)
        {
            try
            {
                errMsg = String.Empty;
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                List<EntityProduct> page = new List<EntityProduct>();
                EntityProduct oBmast = new EntityProduct();
                using (BAProduct oBDTP = new BAProduct())
                {
                    dt2 = oBDTP.Get<Int32>("GET_PRODUCT", 0, productname, ref errMsg, "2019", 1);
                }
                
                if (dt2.Rows.Count > 0)
                {
                    oBmast.HEAD_PRODUCT_KEY = Convert.ToInt32(dt2.Rows[0]["HEAD_PRODUCT_KEY"]);
                    oBmast.PRODUCT_NAME = dt2.Rows[0]["PRODUCT_NAME"].ToString();
                    page.Add(oBmast);
                }
                using (BAProduct oBDTP = new BAProduct())
                {
                    dt3 = oBDTP.Get<Int32>("GET", 0, "", ref errMsg, "2019", 1);
                }
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {

                        oBmast = new EntityProduct();
                        oBmast.HEAD_PRODUCT_KEY = Convert.ToInt32(dt3.Rows[i]["HEAD_PRODUCT_KEY"]);
                        oBmast.PRODUCT_NAME = dt3.Rows[i]["PRODUCT_NAME"].ToString();

                        page.Add(oBmast);
                        Session["packagename"] = oBmast.PRODUCT_NAME;
                    }

                    var getpackage = page.ToList();

                    SelectList list = new SelectList(getpackage, "HEAD_PRODUCT_KEY", "PRODUCT_NAME", ViewBag.ddl_HEAD_PRODUCT_KEY);
                    ViewBag.Package = list;

                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public ActionResult delete(FormCollection form)
        {
            string edit = form[0];
            try
            {

                errMsg = DeleteQuickFact(Convert.ToInt32(edit));

                if (String.IsNullOrEmpty(errMsg))
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDelete + "');");
                    //  MessageBox(1, Message.msgSaveDelete, "");
                    dt = FillProductTableGrid();
                    return Redirect("~/HomeSetting/Index");


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
                // MessageBox(3, Message.msgErrCommon, ex.Message);
            }

            return View("Index", dt);
        }

        private String DeleteQuickFact(Int32 pBANNER_KEY)
        {
            try
            {
                errMsg = String.Empty;
                Int32 vKey = 0;
                EntityHomeSetting OBMST = new EntityHomeSetting();
                using (BAHomeSettings oBHH = new BAHomeSettings())
                {
                    oBHH.SaveDelete<Int32>("DELETE",pBANNER_KEY, ref vKey, ref errMsg, Convert.ToInt32(Session["USER_KEY"]), null, 1);
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