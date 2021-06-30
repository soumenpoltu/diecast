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
    public class TestimonialsController : Controller
    {
        // GET: Testimonials

        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;  
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
                  

                    FillTestimonialGrid();
                    ViewBag.hf_MAST_TESTIMONIALS_KEY = "0";



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
                ViewBag.hf_MAST_TESTIMONIALS_KEY = "0";

                ViewBag.JavaScriptFunction = string.Format("OpenTab2();");
                ViewBag.img_CLIENT = "/Content/admin-assets/images/no_image.jpg";
                dt = FillTestimonialGrid();

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


        private DataTable FillTestimonialGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BATestimonials oBMC = new BATestimonials())
                {
                    dt = oBMC.GetTestimonial<Int32>("GET", 0, "", ref errMsg, "2019", 1);
              
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
             
                return dt;
                //return errMsg;
            }
            catch (Exception ex)
            {
                return dt;
            }

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
                        var imgpath = myfile;
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
        public ActionResult btn_Testimonial_Save_Click(FormCollection form, HttpPostedFileBase fu_Blog)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String CLIENT_IMAGE = String.Empty;
            EntitySiteSetting oBMAST = null;

            try
            {
                if (ModelState.IsValid)
                {


                    errMsg = ProcessTestimonialImage(ref CLIENT_IMAGE, fu_Blog);
                    if (!String.IsNullOrEmpty(errMsg))
                    {
                        return Redirect("~/SiteSetting/Index");
                    }

                    errMsg = String.Empty;
                    oBMAST = new EntitySiteSetting();
                    oBMAST.MAST_TESTIMONIALS_KEY = Convert.ToInt32(form["hf_MAST_TESTIMONIALS_KEY"] == "" ? "0" : form["hf_MAST_TESTIMONIALS_KEY"]);
                    oBMAST.CLIENT_NAME = form["txt_CLIENT_NAME"];
                    oBMAST.CLIENT_FEEDBACK = form["txt_DESCRIPTION"];
                    oBMAST.CLIENT_IMAGE = ViewBag.CLIENT_IMAGE;
                    DateTime dtBlog_Date = Convert.ToDateTime(form["txt_DATE"].Trim());
                    oBMAST.POSTING_DATE = Convert.ToDateTime(dtBlog_Date);
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BATestimonials oBMC = new BATestimonials())
                    {
                        if (oBMAST.MAST_TESTIMONIALS_KEY == 0)
                        {
                            vRef = oBMC.SaveTestimonial<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                return Redirect("~/Testimonials/Index");
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                             
                                return Redirect("~/Testimonials/Index");
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                   
                                return Redirect("~/Testimonials/Index");
                            }

                        }
                        else
                        {
                            vRef = oBMC.SaveTestimonial<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                               
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                           
                              
                                return Redirect("~/Testimonials/Index");
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                return Redirect("~/Testimonials/Index");
                            
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                return Redirect("~/Testimonials/Index");
              
                            }

                        }


                    }

                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                  
                }
                return Redirect("~/Testimonials/Index");
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
              
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }
            return View("Index", dt.Rows);
        }

        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            try
            {
                string edit = form[0];
                errMsg = String.Empty;

                ViewBag.hf_MAST_TESTIMONIALS_KEY = edit.ToString();
                errMsg = FillTestimonialEdit(Convert.ToInt32(edit));
                FillTestimonialGrid();
             
                if (String.IsNullOrEmpty(errMsg))
                {

                    ViewBag.JavaScriptFunction = string.Format("OpenTab2();");
                }
                else
                {
                    
                    TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgErrEditPop + "');");
                }

            }
            catch (Exception ex)
            {
             
                TempData["JavaScriptMsg"] = string.Format("OpenTab2('" + Message.msgErrCommon + "');");
            }

            return View("Index", dt.Rows);
        }

        private String FillTestimonialEdit(Int32 pMAST_TESTIMONIALS_KEY)
        {
            try
            {
                EntitySiteSetting oBMAST = null;
                errMsg = String.Empty;
                dt = new DataTable();

                using (BATestimonials oBMG = new BATestimonials())
                {
                    dt = oBMG.GetTestimonial<Int32>("SRH_KEY", pMAST_TESTIMONIALS_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.txt_CLIENT_NAME = Convert.ToString(dt.Rows[0]["CUST_NAME"]);
                    ViewBag.txt_DESCRIPTION = Convert.ToString(dt.Rows[0]["REMARKS"]);
                    ViewBag.txt_DATE = Convert.ToDateTime(dt.Rows[0]["POSTING_DATE"]).ToString("d");
                    ViewBag.img_CLIENT = ConfigurationManager.AppSettings["CLIENT"].ToString() +  Convert.ToString(dt.Rows[0]["CUST_IMAGE"]);
                    ViewBag.CLIENT_IMAGE = Convert.ToString(dt.Rows[0]["CUST_IMAGE"]);


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
            Int32 vKey = 0; Byte vRef = 0; String vDelMsg = String.Empty;
            EntitySiteSetting oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string edit = form[0];
                    errMsg = String.Empty;
                    oBMAST = new EntitySiteSetting();
       
                    oBMAST.MAST_TESTIMONIALS_KEY = Convert.ToInt32(edit);
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_DELETE = 0;

                    using (BATestimonials oBMC = new BATestimonials())
                    {
                        vRef = oBMC.DeleteTestimonial<Int32>("DELETE", Convert.ToInt32(edit), ref errMsg, Convert.ToInt32(Session["USER_KEY"]),"2019", 1);
                        if (vRef > 0)
                        {
                            if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                          
                            }
                            else if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDelete + "');");
                                return Redirect("~/Testimonials/Index");

                            }
                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                            
                            }
                        }
                    }
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgPageInvalid + "');");
                
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

            return Redirect("~/Testimonials/Index");
        }



    }
}