using MyApp.Entity;
using MyApp.db;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.db.MyAppBal;
using MyApp.Entity.Message;

namespace DeecastZoneCC.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: AboutUs

        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;  
        DataTable dt1; 
        DataTable dt2; 
        DataTable dt3; 
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
                    
                    FillAboutUsEdit();

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
            return View();
        }

        #region About Us

        private String ProcessAboutImage(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.ICON_IMG_1 = Request["hf_ICON_IMG_1"];               
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["ABOUT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg" };


                    var Image_url = file.ToString(); //getting complete url
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") +  ext; //appending the name with id  
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


        private String ProcessAboutImage1(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.ICON_IMG_2 = Request["hf_ICON_IMG_2"];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["ABOUT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg" };


                    var Image_url = file.ToString(); //getting complete url
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + ext; //appending the name with id  
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


        private String ProcessAboutImage2(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.ICON_IMG_3 = Request["hf_ICON_IMG_3"];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["ABOUT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg" };


                    var Image_url = file.ToString(); //getting complete url
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + ext; //appending the name with id  
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

        public ActionResult btn_Head_Save_Click(FormCollection form,HttpPostedFileBase fu_Icon_Image1, HttpPostedFileBase fu_Icon_Image2, HttpPostedFileBase fu_Icon_Image3)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String ABOUT_IMG = String.Empty;
            

            EntityAboutUs oBMAST = null;

            try
            {
                if (ModelState.IsValid)
                {
                    errMsg = ProcessAboutImage(ref ABOUT_IMG, fu_Icon_Image1);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        errMsg = String.Empty;
                        errMsg = ProcessAboutImage1(ref ABOUT_IMG, fu_Icon_Image1);
                        if (String.IsNullOrEmpty(errMsg))
                        {

                            errMsg = String.Empty;
                            errMsg = ProcessAboutImage2(ref ABOUT_IMG, fu_Icon_Image1);

                            if (!String.IsNullOrEmpty(errMsg))
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                                // MessageBox(3, errMsg, "");
                                //return;
                            }
                        }
                    }

                    
                    errMsg = String.Empty;
                    oBMAST = new EntityAboutUs();

                    oBMAST.ABOUT_US_DESC = form["txt_ABOUT_US_DESC"];
                    oBMAST.REMARKS = form["txt_REMARKS"];
                    oBMAST.ICON_IMG_1 = ViewBag.ICON_IMG_1;
                    oBMAST.ICON_IMG_2 = ViewBag.ICON_IMG_2;
                    oBMAST.ICON_IMG_3 = ViewBag.ICON_IMG_3;

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAAboutUs oBMC = new BAAboutUs())
                    {
                        vRef = oBMC.AboutSaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                        if (vRef == 1)
                        {
                            TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                            //MessageBox(2, Message.msgSaveEdit, "");
                            dt1 =FillAboutUsEdit();
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
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //   MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

            return Redirect("~/AboutUs/Index");
        }

        private DataTable FillAboutUsEdit()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt1 = new DataTable();
                using (BAAboutUs oBME = new BAAboutUs())
                {
                    dt1 = oBME.GetData("GET", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.txt_ABOUT_US_DESC = Convert.ToString(dt1.Rows[0]["ABOUT_US_DESC"]);
                    ViewBag.txt_REMARKS = Convert.ToString(dt1.Rows[0]["REMARKS"]);
                    ViewBag.ICON_IMG_1 = Convert.ToString(dt1.Rows[0]["LOGO_1"]);
                    ViewBag.ICON_IMG_2 = Convert.ToString(dt1.Rows[0]["LOGO_2"]);
                    ViewBag.ICON_IMG_3 = Convert.ToString(dt1.Rows[0]["LOGO_3"]);
                }
                return dt1;
            }
            catch (Exception ex)
            {
                return dt1;
            }
           
        }

        #endregion

    }
}