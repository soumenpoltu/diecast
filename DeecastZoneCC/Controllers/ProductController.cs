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
    public class ProductController : Controller
    {
        // GET: Product
        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;
        DataTable dt1;
        DataTable dt2;

        public ActionResult Index()
        {

            if (Session["oSysUser"] != null)
            {
                oSysUser = (EntitySysUser)Session["oSysUser"];
                oSysUser.USER_KEY = Convert.ToInt32(Session["USER_KEY"]);

                oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                errMsg = String.Empty;
                //  FillHomeSettingEdit();
                errMsg = FillMastProductGrid();
                FillDtlsProductGrid("0");
                // ViewBag.QUICK_FACT_KEY = "0";
                //  dt = FillQuickFactTableGrid();
                FillDdPageName();
                ViewBag.hf_DTLS_PRODUCT_KEY = "0";
              
                return View(dt1.Rows);

            }
            else
            {
                return Redirect("~/Admin/Index");
            }

          
        }


        private String FillMastProductGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt = oBMC.Get("GET", "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                if (dt != null)
                {
                    ViewBag.txt_PRODUCT_NAME = dt.Rows[0]["PRODUCT_NAME"].ToString();
                    ViewBag.txt_DESCRIPTION = dt.Rows[0]["PRODUCT_DESC"].ToString();

                    ViewBag.img_SUB_IMAGE_2 = ConfigurationManager.AppSettings["PRODUCT"].ToString() + dt.Rows[0]["PRODUCT_IMAGE_2"].ToString();
                    ViewBag.img_SUB_IMAGE_1 = ConfigurationManager.AppSettings["PRODUCT"].ToString() + dt.Rows[0]["PRODUCT_IMAGE_1"].ToString();
                    ViewBag.img_MAIN_IMAGE = ConfigurationManager.AppSettings["PRODUCT"].ToString()+ dt.Rows[0]["PRODUCT_IMAGE"].ToString();

                    ViewBag.hf_MAIN_IMAGE = dt.Rows[0]["PRODUCT_IMAGE"].ToString();
                    ViewBag.hf_SUB_IMAGE_1 = dt.Rows[0]["PRODUCT_IMAGE_1"].ToString();
                    ViewBag.hf_SUB_IMAGE_2 = dt.Rows[0]["PRODUCT_IMAGE_2"].ToString();
                }
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private String FillDtlsProductGrid(String pSearchKey)
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt1 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt1 = oBMC.GetDatadtls("GET", pSearchKey, ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                TempData["dt1"] = dt1;

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
            EntityProduct oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string edit = form[0];
                    errMsg = String.Empty;
                    oBMAST = new EntityProduct();
                    //GridViewRow gvr = (GridViewRow)((HtmlAnchor)sender).NamingContainer;
                    oBMAST.DTLS_PRODUCT_KEY = Convert.ToInt32(edit);
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_DELETE = 0;

                    using (BAProduct oBMC = new BAProduct())
                    {
                        vRef = oBMC.SaveDeleteDTLS<Int32>("DELETE", oBMAST, ref vKey, ref errMsg, ref vDelMsg, "2019", 1);
                        if (vRef > 0)
                        {
                            if (vRef == 2)
                            {
                                //  MessageBox(1, "Data exists in We Do Master", errMsg);
                            }
                            else if (vRef == 1)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveDelete + "');");
                                return Redirect("~/Product/Index");
                                //MessageBox(1, Message.msgSaveDelete, "");
                                // FillMastBenefitGrid();
                            }
                            else
                            {
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }

                        }
                    }
                }
                else
                {
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

            return Redirect("~/Product/Index");
        }

        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            try
            {
                string edit = form[0];
                errMsg = String.Empty;

                ViewBag.hf_DTLS_PRODUCT_KEY = edit.ToString();
                errMsg = FillMastProductEdit(Convert.ToInt32(edit));
           
                FillDtlsProductGrid("0");
                FillMastProductGrid();
                if (String.IsNullOrEmpty(errMsg))
                {
                    //  aPageName.InnerText = Message.modName21 + "(Edit)";
                
                }
                else
                {
                    //   MessageBox(1, Message.msgErrEditPop, errMsg);
                }

            }
            catch (Exception ex)
            {
                //  MessageBox(1, Message.msgErrCommon, ex.Message);
            }

            return View("Index", dt1.Rows);
        }

        private String FillMastProductEdit(Int32 pDTLS_WE_DO_IT_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAProduct oBMG = new BAProduct())
                {
                    dt = oBMG.GetDatadtls("SRH_KEY", Convert.ToString(pDTLS_WE_DO_IT_KEY), ref errMsg, "2019", 1);

                }
                if (dt != null && dt.Rows.Count > 0)
                {

                    ViewBag.txt_QUANTITY = Convert.ToString(dt.Rows[0]["QTY"]);
                    //ddl_MAST_PRODUCT_CATG_KEY.SelectedValue = Convert.ToString(dt.Rows[0]["MAST_CATEGORY_KEY"]);
                    ViewBag.txt_PRICE = Convert.ToString(dt.Rows[0]["PRICE"]);
               
                }
                FillDdPageName();
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

        private String FillDdPageName()
        {
            try
            {
                errMsg = String.Empty;

                using (BAProduct oBMC = new BAProduct())
                {
                    dt2 = oBMC.ProductBindDdl(0, ref errMsg, null, 0);

                }
                List<EntityProduct> Productcategory = new List<EntityProduct>();
                if (dt2 !=  null)
                {
                    EntityProduct oBmast = new EntityProduct();
                    oBmast.HEAD_PRODUCT_KEY = 0;
                    oBmast.PRODUCT_NAME = "-SELECT AN OPTION-";
                    Productcategory.Add(oBmast);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        oBmast = new EntityProduct();
                        oBmast.HEAD_PRODUCT_KEY = Convert.ToInt32(dt2.Rows[i]["HEAD_PRODUCT_KEY"]);
                        oBmast.PRODUCT_NAME = dt2.Rows[i]["PRODUCT_NAME"].ToString();

                        Productcategory.Add(oBmast);

                    }

                    var Productcategoryname = Productcategory.ToList();

                    SelectList list = new SelectList(Productcategoryname, "HEAD_PRODUCT_KEY", "PRODUCT_NAME", "1");
                    ViewBag.ProductCategoryName = list;

                }
               
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult btn_Head_Save_Click(FormCollection form, HttpPostedFileBase fu_MAIN_IMAGE, HttpPostedFileBase fu_SUB_IMAGE_1, HttpPostedFileBase fu_SUB_IMAGE_2)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String LABELS = String.Empty;
            EntityProduct oBMAST = null;
            try
            {

                if (ModelState.IsValid)
                {
                    errMsg = HeadProcessImage(ref LABELS, fu_MAIN_IMAGE);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        errMsg = ProcessImage(ref LABELS, fu_SUB_IMAGE_1);
                        if (String.IsNullOrEmpty(errMsg))
                        {
                            errMsg = String.Empty;
                            errMsg = ProcessImage2(ref LABELS, fu_SUB_IMAGE_2);
                            if (!String.IsNullOrEmpty(errMsg))
                            {

                            }

                        }

                    }
                  
                    errMsg = String.Empty;
                    oBMAST = new EntityProduct();
                    oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(0);
                    oBMAST.PRODUCT_NAME = form["txt_PRODUCT_NAME"];
                    oBMAST.PRODUCT_DESC = form["txt_DESCRIPTION"];
            
                    oBMAST.MAIN_IMAGE = ViewBag.MAIN_IMG;
                    oBMAST.PRODUCT_IMAGE = ViewBag.SUB_IMAGE_1;
                    oBMAST.PRODUCT_IMAGE_2 = ViewBag.SUB_IMAGE_2;

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 0;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAProduct oBMC = new BAProduct())
                    {
                           vRef = oBMC.SaveHeadProduct<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                            TempData["JavaScriptFunction"] = string.Format("OpenTab1('"+ Message.msgSaveNew + "');");
                         
                                return Redirect("~/Product/Index");
                            }
                            else if (vRef == 2)
                            {
                                return Redirect("~/Product/Index");
                                
                            }
                              
                            else
                            {
                                return Redirect("~/Product/Index");
                              
                            }
                               
                      
                    }
                
                }
                else
                {
                    return Redirect("~/Product/Index");
              
                }
                   
            }
            catch (Exception ex)
            {
                return Redirect("~/Product/Index");
               
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

          

        }

        [HttpPost]
        public ActionResult btn_DTLS_Product_Save_Click(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String LABELS = String.Empty;
            EntityProduct oBMAST = null;
            try
            {

                if (ModelState.IsValid)
                {
                   
                    errMsg = String.Empty;
                    oBMAST = new EntityProduct();
                    oBMAST.DTLS_PRODUCT_KEY = Convert.ToInt32(form["hf_DTLS_PRODUCT_KEY"]);
                    oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(form["ddl_HEAD_PRODUCT_KEY"]);
                    oBMAST.PRICE = Convert.ToString(form["txt_PRICE"]);
                    oBMAST.QTY = Convert.ToString(form["txt_QUANTITY"]);

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 0;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAProduct oBMC = new BAProduct())
                    {
                        if (oBMAST.DTLS_PRODUCT_KEY == 0)
                        {
                            vRef = oBMC.SaveDTlsProduct<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                            }
                            else if (vRef == 2)
                            {
                                //MessageBox(2, Message.msgValidation, errMsg);
                            }
                               
                            else
                            {
                               // MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                                
                        }
                        else
                        {
                            vRef = oBMC.SaveDTlsProduct<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                            }
                            else if (vRef == 2)
                            {
                               // MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }
                                
                            else
                            {
                               // MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                                
                        }
                    }
                   // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                }
                else
                {
                  //  MessageBox(2, Message.msgPageInvalid, "");
                }

              
            }
            catch (Exception ex)
            {
               // MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }
            return Redirect("~/Product/Index");
        }

        private String ProcessImage(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.SUB_IMAGE_1 = Request["hf_SUB_IMAGE_1"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
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

                        file.SaveAs(path);

                        ViewBag.SUB_IMAGE_1 = myfile;

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

        private String ProcessImage2(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.SUB_IMAGE_2 = Request["hf_SUB_IMAGE_2"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
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

                        file.SaveAs(path);

                        ViewBag.SUB_IMAGE_2 = myfile;

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


        private String HeadProcessImage(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.MAIN_IMG = Request["hf_MAIN_IMAGE"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
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

                        file.SaveAs(path);

                        ViewBag.MAIN_IMG = myfile;

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

    }
}