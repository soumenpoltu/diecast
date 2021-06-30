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

namespace DiecastDecals.Controllers
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
               
                // ViewBag.QUICK_FACT_KEY = "0";
                //  dt = FillQuickFactTableGrid();

                ViewBag.hf_HEAD_PRODUCT_KEY = "0";
                ViewBag.hf_DTLS_PRODUCT_KEY = "0";
                FillDdPageName();
                GetSubCat(0);
                GetSubSubCat(0);
                return View(dt.Rows);

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
                    dt = oBMC.Get<Int32>("GET", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
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
            EntityProduct oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string edit = form[0];
                    errMsg = String.Empty;
                    oBMAST = new EntityProduct();
                    //GridViewRow gvr = (GridViewRow)((HtmlAnchor)sender).NamingContainer;
                    oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(edit);
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_DELETE = 0;

                    using (BAProduct oBMC = new BAProduct())
                    {
                        vRef = oBMC.SaveDelete<Object, Int32>("DELETE", oBMAST, null, ref vKey, ref errMsg, ref vDelMsg, "2019", 1);
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
                ViewBag.DownloadStyle = "style='display:none;'";
                ViewBag.hf_HEAD_PRODUCT_KEY = edit.ToString();
            
                errMsg = FillMastProductEdit(Convert.ToInt32(edit));
                FillMastProductGrid();


                if (String.IsNullOrEmpty(errMsg))
                {
                    //  aPageName.InnerText = Message.modName21 + "(Edit)";
                    ViewBag.JavaScriptFunction = string.Format("OpenTab3();");
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

            return View("Index", dt.Rows);
        }


        [HttpPost]
        public ActionResult download(FormCollection form)
        {
            try
            {
                string edit = form[0];
                errMsg = String.Empty;
                ViewBag.DownloadStyle = "style='display:block;'";
                ViewBag.hf_HEAD_PRODUCT_KEY = edit.ToString();

                errMsg = FillMastProductEdit(Convert.ToInt32(edit));
                FillMastProductGrid();


                if (String.IsNullOrEmpty(errMsg))
                {
                    //  aPageName.InnerText = Message.modName21 + "(Edit)";
                    ViewBag.JavaScriptFunction = string.Format("OpenTab3();");
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

            return View("Index", dt.Rows);
        }

        private String FillMastProductEdit(Int32 pDTLS_WE_DO_IT_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAProduct oBMG = new BAProduct())
                {
                    dt = oBMG.Get<Int32>("SRH_KEY", pDTLS_WE_DO_IT_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {

                    ViewBag.txt_IMAGE_NAME = Convert.ToString(dt.Rows[0]["PRODUCT_NAME"]);
                    //ddl_MAST_PRODUCT_CATG_KEY.SelectedValue = Convert.ToString(dt.Rows[0]["MAST_CATEGORY_KEY"]);
                    ViewBag.ddl_MAST_PRODUCT_CATG_KEY = Convert.ToString(dt.Rows[0]["MAST_CATEGORY_KEY"]);
                    ViewBag.ddl_MAST_PRODUCT_SUB_CATG_KEY = Convert.ToString(dt.Rows[0]["SUB_CATEGORY_KEY"]);
                    ViewBag.ddl_MAST_PRODUCT_SUB_SUB_CATG_KEY = Convert.ToString(dt.Rows[0]["SUB_SUB_CATEGORY_KEY"]);

                    ViewBag.txt_PRICE = Convert.ToString(dt.Rows[0]["PRICE"]);
               
                    ViewBag.hf_HEAD_PRODUCT_IMG = Convert.ToString(dt.Rows[0]["PRODUCT_IMAGE"]);
                    ViewBag.hf_HEAD_PRODUCT_WATERMARK_IMG = Convert.ToString(dt.Rows[0]["PRODUCT_WATER_MARK_IMAGE"]);
                    ViewBag.hf_HEAD_PRODUCT_AI_IMG = Convert.ToString(dt.Rows[0]["PRODUCT_FILE"]);


                    ViewBag.txt_SHEET_CODE_ONE = Convert.ToString(dt.Rows[0]["SHEET_CODE_1"]);
                    ViewBag.hf_SHEET_CODE_1_IMG = Convert.ToString(dt.Rows[0]["SHEET_CODE_1_IMAGE"]);
                    ViewBag.hf_SHEET_CODE_1_FILE = Convert.ToString(dt.Rows[0]["SHEET_CODE_1_FILE"]);


                    ViewBag.txt_SHEET_CODE_TWO = Convert.ToString(dt.Rows[0]["SHEET_CODE_2"]);
                    ViewBag.hf_SHEET_CODE_2_IMG = Convert.ToString(dt.Rows[0]["SHEET_CODE_2_IMAGE"]);
                    ViewBag.hf_SHEET_CODE_2_FILE = Convert.ToString(dt.Rows[0]["SHEET_CODE_2_FILE"]);


                    ViewBag.txt_SHEET_CODE_THREE = Convert.ToString(dt.Rows[0]["SHEET_CODE_3"]);
                    ViewBag.hf_SHEET_CODE_3_IMG = Convert.ToString(dt.Rows[0]["SHEET_CODE_3_IMAGE"]);
                    ViewBag.hf_SHEET_CODE_3_FILE = Convert.ToString(dt.Rows[0]["SHEET_CODE_3_FILE"]);


                    ViewBag.txt_SHEET_CODE_FOUR = Convert.ToString(dt.Rows[0]["SHEET_CODE_4"]);
                    ViewBag.hf_SHEET_CODE_4_IMG = Convert.ToString(dt.Rows[0]["SHEET_CODE_4_IMAGE"]);
                    ViewBag.hf_SHEET_CODE_4_FILE = Convert.ToString(dt.Rows[0]["SHEET_CODE_4_FILE"]);



                    //ViewBag.hf_HEAD_PRODUCT_KEY.Value = Convert.ToString(dt.Rows[0]["HEAD_PRODUCT_KEY"]);

                    //img_business.Src = "~" + ConfigurationManager.AppSettings["SOLUTION"].ToString() + Convert.ToString(dt.Rows[0]["BUSINESS_IMAGE"]);
                    //hf_BUSINESS_IMAGE.Value = Convert.ToString(dt.Rows[0]["BUSINESS_IMAGE"]);
                }
                FillDdPageName();
                GetSubCat(Convert.ToInt32(ViewBag.ddl_MAST_PRODUCT_CATG_KEY));
                GetSubSubCat(Convert.ToInt32(ViewBag.ddl_MAST_PRODUCT_SUB_CATG_KEY));
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

                using (BAProductCat oBMC = new BAProductCat())
                {
                    dt2 = oBMC.BindDdl(0, ref errMsg, null, 0);

                }
                List<EntityProductCatg> Productcategory = new List<EntityProductCatg>();
                if (dt2.Rows.Count > 0)
                {
                    EntityProductCatg oBmast = new EntityProductCatg();
                    oBmast.MAST_CATEGORY_KEY = 0;
                    oBmast.CATEGORY_NAME = "-SELECT AN OPTION-";
                    Productcategory.Add(oBmast);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        oBmast = new EntityProductCatg();
                        oBmast.MAST_CATEGORY_KEY = Convert.ToInt32(dt2.Rows[i]["MAST_CATEGORY_KEY"]);
                        oBmast.CATEGORY_NAME = dt2.Rows[i]["CATEGORY_NAME"].ToString();

                        Productcategory.Add(oBmast);

                    }

                    var Productcategoryname = Productcategory.ToList();

                    SelectList list = new SelectList(Productcategoryname, "MAST_CATEGORY_KEY", "CATEGORY_NAME", ViewBag.ddl_MAST_PRODUCT_CATG_KEY);
                    ViewBag.ProductCategoryName = list;

                }
               
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult Add()
        {
            try
            {
                FillMastProductGrid();
                FillDdPageName();
                GetSubCat(0);
                GetSubSubCat(0);

                // errMsg = String.Empty;
                ViewBag.hf_HEAD_PRODUCT_KEY = "0";
                ViewBag.hf_DTLS_PRODUCT_KEY = "0";
                ViewBag.DownloadStyle = "style='display:none;'";
                ViewBag.JavaScriptFunction = string.Format("OpenTab2();");
               // FillMastProductCatgGrid();
                //TempData["JavaScriptFunction"] = string.Format("OpenTab2();");
                //ClearControl();
                //MessageBox(2, "", "");
            }
            catch (Exception ex)
            {
                //  MessageBox(1, Message.msgErrCommon, ex.Message);
            }

            return View("Index", dt.Rows);
        }

        [HttpPost]
        public ActionResult btn_Head_Save_Click(FormCollection form, HttpPostedFileBase fu_head_Product_img,HttpPostedFileBase fu_head_Watermark_Product_img, HttpPostedFileBase fu_head_ai_Product_img, HttpPostedFileBase fu_sheet_code_one_img, HttpPostedFileBase fu_sheet_code_one_pdf, HttpPostedFileBase fu_sheet_code_two_img, HttpPostedFileBase fu_sheet_code_two_pdf, HttpPostedFileBase fu_sheet_code_three_img, HttpPostedFileBase fu_sheet_code_three_pdf, HttpPostedFileBase fu_sheet_code_four_img, HttpPostedFileBase fu_sheet_code_four_pdf)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String LABELS = String.Empty;
            EntityProduct oBMAST = null;
            try
            {

                if (ModelState.IsValid)
                {
                   
                    errMsg = HeadProcessImage(ref LABELS, fu_head_Product_img);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        errMsg = ProcessImage(ref LABELS, fu_head_Watermark_Product_img);
                        if (String.IsNullOrEmpty(errMsg))
                        {
                            
                               errMsg = String.Empty;
                            errMsg = ProcessImage2(ref LABELS, fu_head_ai_Product_img);
                            if (String.IsNullOrEmpty(errMsg))
                            {
                                errMsg = String.Empty;
                                errMsg = SheetCode1Image(ref LABELS, fu_sheet_code_one_img);
                                if (String.IsNullOrEmpty(errMsg))
                                {

                                    errMsg = String.Empty;
                                    errMsg = SheetCode1Pdf(ref LABELS, fu_sheet_code_one_pdf);
                                    if (String.IsNullOrEmpty(errMsg))
                                    {

                                        errMsg = String.Empty;
                                        errMsg = SheetCode2Image(ref LABELS, fu_sheet_code_two_img);
                                        if (String.IsNullOrEmpty(errMsg))
                                        {

                                            errMsg = String.Empty;
                                            errMsg = SheetCode2Pdf(ref LABELS, fu_sheet_code_two_pdf);
                                            if (String.IsNullOrEmpty(errMsg))
                                            {
                                                errMsg = String.Empty;
                                                errMsg = SheetCode3Image(ref LABELS, fu_sheet_code_three_img);
                                                if (String.IsNullOrEmpty(errMsg))
                                                {

                                                    errMsg = String.Empty;
                                                    errMsg = SheetCode3Pdf(ref LABELS, fu_sheet_code_three_pdf);
                                                    if (String.IsNullOrEmpty(errMsg))
                                                    {
                                                        errMsg = String.Empty;
                                                        errMsg = SheetCode4Image(ref LABELS, fu_sheet_code_four_img);
                                                        if (String.IsNullOrEmpty(errMsg))
                                                        {

                                                            errMsg = String.Empty;
                                                            errMsg = SheetCode4Pdf(ref LABELS, fu_sheet_code_four_pdf);
                                                            if (!String.IsNullOrEmpty(errMsg))
                                                            {

                                                            }

                                                        }

                                                    }

                                                }

                                            }

                                        }

                                    }

                                }

                            }

                        }
                    }
                    errMsg = String.Empty;
                    oBMAST = new EntityProduct();
                    oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(form["hf_HEAD_PRODUCT_KEY"]);
                    oBMAST.PRODUCT_NAME = form["txt_IMAGE_NAME"];

                    oBMAST.PRICE = form["txt_PRICE"];
                    oBMAST.MAST_CATEGORY_KEY = Convert.ToInt32(form["ddl_MAST_PRODUCT_CATG_KEY"]);
                    oBMAST.SUB_CATEGORY_KEY = Convert.ToInt32(form["ddl_MAST_PRODUCT_SUB_CATG_KEY"]);
                    oBMAST.SUB_SUB_CATEGORY_KEY = Convert.ToInt32(form["ddl_MAST_PRODUCT_SUB_SUB_CATG_KEY"]);

                    oBMAST.MAIN_IMAGE = ViewBag.hf_HEAD_PRODUCT_IMG;
                    oBMAST.PRODUCT_IMAGE = ViewBag.hf_HEAD_PRODUCT_WATERMARK_IMG;
                    oBMAST.PRODUCT_IMAGE_2 = ViewBag.hf_HEAD_PRODUCT_AI_IMG;


                    oBMAST.SHEET_CODE_1 = form["txt_SHEET_CODE_ONE"];
                    oBMAST.SHEET_CODE_1_IMG = ViewBag.hf_SHEET_CODE_1_IMG;
                    oBMAST.SHEET_CODE_1_PDF = ViewBag.hf_SHEET_CODE_1_FILE;


                    oBMAST.SHEET_CODE_2 = form["txt_SHEET_CODE_TWO"];
                    oBMAST.SHEET_CODE_2_IMG = ViewBag.hf_SHEET_CODE_2_IMG;
                    oBMAST.SHEET_CODE_2_PDF = ViewBag.hf_SHEET_CODE_2_FILE;


                    oBMAST.SHEET_CODE_3 = form["txt_SHEET_CODE_THREE"];
                    oBMAST.SHEET_CODE_3_IMG = ViewBag.hf_SHEET_CODE_3_IMG;
                    oBMAST.SHEET_CODE_3_PDF = ViewBag.hf_SHEET_CODE_3_FILE;


                    oBMAST.SHEET_CODE_4 = form["txt_SHEET_CODE_FOUR"];
                    oBMAST.SHEET_CODE_4_IMG = ViewBag.hf_SHEET_CODE_4_IMG;
                    oBMAST.SHEET_CODE_4_PDF = ViewBag.hf_SHEET_CODE_4_FILE;




                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 0;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAProduct oBMC = new BAProduct())
                    {
                        if (oBMAST.HEAD_PRODUCT_KEY == 0)
                        {
                            vRef = oBMC.ProductSaveChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('"+ Message.msgSaveNew + "');");
                                //  MessageBox(3, Message.msgSaveNew, "");

                                ViewBag.hf_HEAD_PRODUCT_KEY = Convert.ToString(vKey);
                               errMsg = FillMastProductEdit(Convert.ToInt16(vKey));
                         
                                FillMastProductGrid();
                                return Redirect("~/Product/Index");
                            }
                            else if (vRef == 2)
                            {
                                return Redirect("~/Product/Index");
                                // MessageBox(2, Message.msgValidation, errMsg);

                            }
                              
                            else
                            {
                                return Redirect("~/Product/Index");
                                // MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                               
                        }
                        else
                        {
                            vRef = oBMC.ProductSaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {

                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                ViewBag.hf_HEAD_PRODUCT_KEY = Convert.ToString(vKey);
                                errMsg = FillMastProductEdit(Convert.ToInt16(vKey));
                          
                                FillMastProductGrid();
                                return Redirect("~/Product/Index");
                                //  MessageBox(3, Message.msgSaveEdit, "");
                                //errMsg = FillMastProductEdit(oBMAST.HEAD_PRODUCT_KEY);
                                //hf_HEAD_PRODUCT_KEY.Value = Convert.ToString(vKey);
                            }
                            else if (vRef == 2)
                            {
                                return Redirect("~/Product/Index");
                                //MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }

                            else
                            {
                                return Redirect("~/Product/Index");
                                //  MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                               
                        }
                    }
                   // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                }
                else
                {
                    return Redirect("~/Product/Index");
                    //  MessageBox(2, Message.msgPageInvalid, "");
                }
                   
            }
            catch (Exception ex)
            {
                return Redirect("~/Product/Index");
                // MessageBox(2, Message.msgErrCommon, ex.Message);
            }
            finally
            {
                if (oBMAST != null)
                    oBMAST = null;
            }

           

        }


        private String FillMastProductDtlsEdit(String pDTLS_WE_DO_IT_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAProduct oBMG = new BAProduct())
                {
                    dt = oBMG.GetData("SRH_KEY", pDTLS_WE_DO_IT_KEY, ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {

                   ViewBag.txt_SHEETCODE = Convert.ToString(dt.Rows[0]["SHEET_CODE"]);

                    ViewBag.hf_DTLS_PRODUCT_KEY = Convert.ToString(dt.Rows[0]["DTLS_PRODUCT_KEY"]);
                    ViewBag.hf_HEAD_PRODUCT_KEY = Convert.ToString(dt.Rows[0]["HEAD_PRODUCT_KEY"]);
                    ViewBag.hf_PRODUCT_IMG = Convert.ToString(dt.Rows[0]["FILE_UPLOAD_1"]);
                    ViewBag.hf_PRODUCT_IMG_2 = Convert.ToString(dt.Rows[0]["FILE_UPLOAD_2"]);

                    ViewBag.product_img = ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["FILE_UPLOAD_1"]);
                    ViewBag.product_img_2 = ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt.Rows[0]["FILE_UPLOAD_2"]);
                    FillMastProductEdit(Convert.ToInt16(ViewBag.hf_HEAD_PRODUCT_KEY));
                  
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

        
        private String ProcessImage(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_HEAD_PRODUCT_WATERMARK_IMG = Request["hf_HEAD_PRODUCT_WATERMARK_IMG"];
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

                        ViewBag.hf_HEAD_PRODUCT_WATERMARK_IMG = myfile;

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
                ViewBag.hf_HEAD_PRODUCT_AI_IMG = Request["hf_HEAD_PRODUCT_AI_IMG"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".svg",".ai",".pdf" };


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

                        ViewBag.hf_HEAD_PRODUCT_AI_IMG = myfile;

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
                ViewBag.hf_HEAD_PRODUCT_IMG = Request["hf_HEAD_PRODUCT_IMG"];
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

                        ViewBag.hf_HEAD_PRODUCT_IMG = myfile;

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


        public JsonResult GetSubCat(Int32 categorykey)
        {

            try
            {
                errMsg = String.Empty;

                using (BAProductCat oBMC = new BAProductCat())
                {
                    dt2 = oBMC.SubCatBindDdl(categorykey, ref errMsg, null, 0);

                }
                List<EntityProductCatg> Productcategory = new List<EntityProductCatg>();

                EntityProductCatg oBmast = new EntityProductCatg();
                oBmast.SUB_CATEGORY_KEY = 0;
                oBmast.SUB_CATEGORY_NAME = "-SELECT AN OPTION-";
                Productcategory.Add(oBmast);

                if (dt2.Rows.Count > 0 && dt2 != null)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        oBmast = new EntityProductCatg();
                        oBmast.SUB_CATEGORY_KEY = Convert.ToInt32(dt2.Rows[i]["SUB_CATEGORY_KEY"]);
                        oBmast.SUB_CATEGORY_NAME = dt2.Rows[i]["SUB_CATEGORY_NAME"].ToString();

                        Productcategory.Add(oBmast);

                    }

                    var Productcategoryname = Productcategory.ToList();

                    SelectList list = new SelectList(Productcategoryname, "SUB_CATEGORY_KEY", "SUB_CATEGORY_NAME", ViewBag.ddl_MAST_PRODUCT_SUB_CATG_KEY);
                    ViewBag.ProductSubCategoryName = list;

                }
                else
                {
                    var Productcategoryname = Productcategory.ToList();


                    SelectList list = new SelectList(Productcategoryname, "SUB_CATEGORY_KEY", "SUB_CATEGORY_NAME", ViewBag.ddl_MAST_PRODUCT_SUB_CATG_KEY);
                    ViewBag.ProductSubCategoryName = list;
                }

                return Json(Productcategory);

            }
            catch (Exception ex)
            {
                return Json(0);
            }




        }



        public JsonResult GetSubSubCat(Int32 Subcategorykey)
        {

            try
            {
                errMsg = String.Empty;

                using (BAProductCat oBMC = new BAProductCat())
                {
                    dt2 = oBMC.SubSubCatBindDdl(Subcategorykey, ref errMsg, null, 0);

                }
                List<EntityProductCatg> Productcategory = new List<EntityProductCatg>();
                EntityProductCatg oBmast = new EntityProductCatg();
                oBmast.SUB_SUB_CATEGORY_KEY = 0;
                oBmast.SUB_SUB_CATEGORY_NAME = "-SELECT AN OPTION-";
                Productcategory.Add(oBmast);

                if (dt2.Rows.Count > 0 && dt2 != null)
                {
                    
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        oBmast = new EntityProductCatg();
                        oBmast.SUB_SUB_CATEGORY_KEY = Convert.ToInt32(dt2.Rows[i]["SUB_SUB_CATEGORY_KEY"]);
                        oBmast.SUB_SUB_CATEGORY_NAME = dt2.Rows[i]["SUB_SUB_CATEGORY_NAME"].ToString();

                        Productcategory.Add(oBmast);

                    }

                    var Productcategoryname = Productcategory.ToList();

                    SelectList list = new SelectList(Productcategoryname, "SUB_SUB_CATEGORY_KEY", "SUB_SUB_CATEGORY_NAME", ViewBag.ddl_MAST_PRODUCT_SUB_SUB_CATG_KEY);
                    ViewBag.ProductSubSubCategoryName = list;

                }
                else
                {
                    var Productcategoryname = Productcategory.ToList();


                    SelectList list = new SelectList(Productcategoryname, "SUB_SUB_CATEGORY_KEY", "SUB_SUB_CATEGORY_NAME", ViewBag.ddl_MAST_PRODUCT_SUB_SUB_CATG_KEY);
                    ViewBag.ProductSubSubCategoryName = list;
                }


                return Json(Productcategory);

            }
            catch (Exception ex)
            {
                return Json(0);
            }




        }



        private String SheetCode1Image(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_1_IMG = Request["hf_SHEET_CODE_1_IMG"];
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

                        ViewBag.hf_SHEET_CODE_1_IMG = myfile;

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


        private String SheetCode1Pdf(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_1_FILE = Request["hf_SHEET_CODE_1_FILE"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".ai", ".pdf", ".png", ".bmp", ".gif", ".webp", ".svg" };


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

                        ViewBag.hf_SHEET_CODE_1_FILE = myfile;

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




        private String SheetCode2Image(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_2_IMG = Request["hf_SHEET_CODE_2_IMG"];
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

                        ViewBag.hf_SHEET_CODE_2_IMG = myfile;

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


        private String SheetCode2Pdf(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_2_FILE = Request["hf_SHEET_CODE_2_FILE"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".ai", ".pdf", ".png", ".bmp", ".gif", ".webp", ".svg" };


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

                        ViewBag.hf_SHEET_CODE_2_FILE = myfile;

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


        private String SheetCode3Image(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_3_IMG = Request["hf_SHEET_CODE_3_IMG"];
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

                        ViewBag.hf_SHEET_CODE_3_IMG = myfile;

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


        private String SheetCode3Pdf(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_3_FILE = Request["hf_SHEET_CODE_3_FILE"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".ai", ".pdf", ".png", ".bmp", ".gif", ".webp", ".svg" };


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

                        ViewBag.hf_SHEET_CODE_3_FILE = myfile;

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


        private String SheetCode4Image(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_4_IMG = Request["hf_SHEET_CODE_4_IMG"];
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

                        ViewBag.hf_SHEET_CODE_4_IMG = myfile;

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


        private String SheetCode4Pdf(ref String Doc_Name, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.hf_SHEET_CODE_4_FILE = Request["hf_SHEET_CODE_4_FILE"];
                //ViewBag.LOGO_NAME = Request[ViewBag.hf_LOGO_NAME];
                return String.Empty;
            }
            else
            {
                String[] ImageAcceptedExtensions = null;
                String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
                try
                {

                    var allowedExtensions = new[] { ".ai", ".pdf", ".png", ".bmp", ".gif", ".webp", ".svg" };


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

                        ViewBag.hf_SHEET_CODE_4_FILE = myfile;

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
        public FileResult DownloadPDF(FormCollection form)
        {
            string filename = form[0];
            FileInfo fi = new FileInfo(filename);
            string extn = fi.Extension;
            String DOC_PATH = "~" + ConfigurationManager.AppSettings["PRODUCT"].ToString();
            string extension = extn.Substring(1);
            return File(""+ DOC_PATH + filename + "", "application/"+ extension + "", "File."+ extension + "");
        }


    }
}