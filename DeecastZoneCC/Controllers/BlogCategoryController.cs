using MyApp.db.MyAppBal;
using MyApp.Entity;
using MyApp.Entity.Message;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeecastZoneCC.Controllers
{
    public class BlogCategoryController : Controller
    {
        // GET: BlogCategory
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
                    dt=FillBlogTag();
                  
                }
                else
                {
                    return Redirect("~/Admin/Index");
                }
            }
            catch (Exception ex)
            {
                // MessageBox(1, Message.msgErrCommon, ex.Message);
                ViewBag.JavaScriptFunction = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
            }
            
            return View("Index",dt.Rows);
        }

        #region Blog Category
        private void ClearBlogCat()
        {

            ViewBag.txt_BLOG_CAT_DESC = "";
            ViewBag.hf_MAST_BLOG_CATEGORY_KEY = "0";

        }

        [HttpPost]
        public ActionResult btn_Head_Save_Click(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String errMsg = String.Empty;
            EntityBlogCat oBMAST = null;

            try
            {
                if (ModelState.IsValid)
                {

                    errMsg = String.Empty;
                    oBMAST = new EntityBlogCat();
                    oBMAST.MAST_BLOG_CATEGORY_KEY = Convert.ToInt32(form["hf_MAST_BLOG_CATEGORY_KEY"] == "" ? "0" : form["hf_MAST_BLOG_CATEGORY_KEY"]);
                    oBMAST.BLOG_CAT_DESC = form["txt_BLOG_CAT_DESC"]== "" ? null : form["txt_BLOG_CAT_DESC"].ToString();
                    

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BABlogCat oBMC = new BABlogCat())
                    {
                        if (oBMAST.MAST_BLOG_CATEGORY_KEY == 0)
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                ClearBlogCat();
                                //MessageBox(2, Message.msgSaveNew, "");
                                dt = FillBlogTag();

                                return Redirect("~/BlogCategory/Index");
                            }
                            else if (vRef == 2)
                            {
                                //MessageBox(2, Message.msgSaveDuplicate, errMsg);
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                return Redirect("~/BlogCategory/Index");
                            }
                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                //MessageBox(2, Message.msgSaveErr, errMsg);
                                return Redirect("~/BlogCategory/Index");
                            }
                               
                        }
                        else
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                ClearBlogCat();
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                                //MessageBox(2, Message.msgSaveEdit, "");
                                dt = FillBlogTag();
                                
                               // return Redirect("~/BlogCategory/Index");
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                return Redirect("~/BlogCategory/Index");
                                //MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                return Redirect("~/BlogCategory/Index");
                                //MessageBox(2, Message.msgSaveErr, errMsg);
                            }

                        }


                    }

                    // oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                    //ClearControl();
                }
               
                return Redirect("~/BlogCategory/Index");
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

        private DataTable FillBlogTag()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BABlogCat oBMC = new BABlogCat())
                {
                    dt = oBMC.Get<Int32>("GET", 0, "", ref errMsg, "2019", 1);                   
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
                ViewData["dt"] = dt;
                return dt;
                //return errMsg;
            }
            catch (Exception ex)
            {
                return dt;
            }

        }

        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            string edit = form[0];
            try
            {
                EntityBlogCat oBMAST = new EntityBlogCat();
                errMsg = String.Empty;
                oBMAST.MAST_BLOG_CATEGORY_KEY = Convert.ToInt32(edit);
                ViewBag.hf_MAST_BLOG_CATEGORY_KEY = edit;

                errMsg = FillBlogCatEdit(Convert.ToInt32(edit));
                if (String.IsNullOrEmpty(errMsg))
                {

                    dt = FillBlogTag();
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

        private String FillBlogCatEdit(Int32 pMAST_BLOG_CATEGORY_KEY)
        {
            try
            {
                EntitySiteSetting oBMAST = null;
                errMsg = String.Empty;
                dt1 = new DataTable();

                using (BABlogCat oBMG = new BABlogCat())
                {
                    dt1 = oBMG.Get<Int32>("SRH_KEY", pMAST_BLOG_CATEGORY_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.txt_BLOG_CAT_DESC = Convert.ToString(dt1.Rows[0]["BLOG_CAT_DESC"]);
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
                errMsg = GridBlogCategoryDelete(Convert.ToInt32(edit));
                if (String.IsNullOrEmpty(errMsg))
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDelete + "');");
                    //  MessageBox(1, Message.msgSaveDelete, "");
                    dt = FillBlogTag();

                    return Redirect("~/BlogCategory/Index");
                }
                else
                {
                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                    //  MessageBox(3, Message.msgErrCommon, errMsg);
                }
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"]= string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                //  MessageBox(3, Message.msgErrCommon, ex.Message);
            }
            return View("Index", dt.Rows);
        }

        private String GridBlogCategoryDelete(Int32 pMAST_BLOG_CATEGORY_KEY)
        {
            try
            {
                errMsg = String.Empty;
                Int32 vKey = 0;
                using (BABlogCat oBHH = new BABlogCat())
                {
                    oBHH.Delete<Int32>("DELETE", pMAST_BLOG_CATEGORY_KEY, ref vKey, ref errMsg, 0, null, 1);
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