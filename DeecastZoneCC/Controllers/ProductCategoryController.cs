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
    public class ProductCategoryController : Controller
    {
        // GET: HomeSetting
        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;
        DataTable dt1;


        // GET: Product
        public ActionResult Index()
        {

            if (Session["oSysUser"] != null)
            {
                oSysUser = (EntitySysUser)Session["oSysUser"];
                oSysUser.USER_KEY = Convert.ToInt32(Session["USER_KEY"]);

                oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                errMsg = String.Empty;
                //  FillHomeSettingEdit();
                errMsg = FillMastProductCatgGrid();
                // ViewBag.QUICK_FACT_KEY = "0";
                //  dt = FillQuickFactTableGrid();
                ViewBag.hf_MAST_PRODUCT_CATG_KEY = "0";
                return View(dt.Rows);

            }
            else
            {
                return Redirect("~/Admin/Index");
            }

        }

        private String FillMastProductCatgGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BAProductCat oBMC = new BAProductCat())
                {
                    dt = oBMC.Get<Int32>("GET", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                }
              //  gvMastBenefits.DataSource = dt;
                //gvMastBenefits.DataBind();
                //MessageBox(1, "", "");
                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        [HttpPost]
        public ActionResult Add()
        {
            try
            {
                // errMsg = String.Empty;
                ViewBag.hf_MAST_PRODUCT_CATG_KEY = "0";

                ViewBag.JavaScriptFunction = string.Format("OpenTab2();");
                FillMastProductCatgGrid();
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
        public ActionResult delete(FormCollection form)
        {
            Int32 vKey = 0; Byte vRef = 0; String vDelMsg = String.Empty;
            EntityProductCatg oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string edit = form[0];
                    errMsg = String.Empty;
                    oBMAST = new EntityProductCatg();
                    //GridViewRow gvr = (GridViewRow)((HtmlAnchor)sender).NamingContainer;
                    oBMAST.MAST_CATEGORY_KEY = Convert.ToInt32(edit);
                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_DELETE = 0;

                    using (BAProductCat oBMC = new BAProductCat())
                    {
                        vRef = oBMC.SaveDelete<Object, Int32>("DELETE", oBMAST, null, ref vKey, ref errMsg, ref vDelMsg, "2019", 1);
                        if (vRef > 0)
                        {
                            if (vRef == 2)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('Data exists in We Do Master');");
                                //  MessageBox(1, "Data exists in We Do Master", errMsg);
                            }
                            else if (vRef == 1)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('"+ Message.msgSaveDelete +"');");
                                return Redirect("~/ProductCategory/Index");
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

            return Redirect("~/ProductCategory/Index");
        }

        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            try
            {
                string edit = form[0];
                errMsg = String.Empty;

                ViewBag.hf_MAST_PRODUCT_CATG_KEY = edit.ToString();
                errMsg = FillMastProductEdit(Convert.ToInt32(edit));
                FillMastProductCatgGrid();

                if (String.IsNullOrEmpty(errMsg))
                {
                    //  aPageName.InnerText = Message.modName21 + "(Edit)";
                    ViewBag.JavaScriptFunction = string.Format("OpenTab2();");
                }
                else
                {

                    ViewBag.JavaScriptFunction = string.Format("OpenTab1('" + Message.msgErrEditPop + "');");

                    //   MessageBox(1, Message.msgErrEditPop, errMsg);
                }

            }
            catch (Exception ex)
            {
                //  MessageBox(1, Message.msgErrCommon, ex.Message);
            }

            return View("Index", dt.Rows);
        }

        private String FillMastProductEdit(Int32 pDTLS_PRODUCT_CATG_KEY)
        {
            try
            {
                errMsg = String.Empty;
                dt = new DataTable();
                using (BAProductCat oBMG = new BAProductCat())
                {
                    dt = oBMG.Get<Int32>("GET_DTLS", pDTLS_PRODUCT_CATG_KEY, "", ref errMsg, "2019", 1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.txt_PRODUCT_CATG_NAME = Convert.ToString(dt.Rows[0]["CATEGORY_NAME"]);
                    //hf_MAST_PRODUCT_CATG_KEY.Value = Convert.ToString(dt.Rows[0]["MAST_CATEGORY_KEY"]);
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
        public ActionResult btn_Head_Save_Click(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            String LABELS = String.Empty;
            EntityProductCatg oBMAST = null;
            try
            {

                if (ModelState.IsValid)
                {

                    errMsg = String.Empty;
                    oBMAST = new EntityProductCatg();
                    oBMAST.MAST_CATEGORY_KEY = Convert.ToInt32(form["hf_MAST_PRODUCT_CATG_KEY"]);

                    oBMAST.CATEGORY_NAME = form["txt_PRODUCT_CATG_NAME"];



                    oBMAST.ENT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(Session["USER_KEY"]);
                    oBMAST.TAG_ACTIVE = 0;
                    oBMAST.TAG_DELETE = 0;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BAProductCat oBMC = new BAProductCat())
                    {
                        if (oBMAST.MAST_CATEGORY_KEY == 0)
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                              //  MessageBox(2, Message.msgSaveNew, "");
                                errMsg = FillMastProductCatgGrid();
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('"+ Message.msgSaveNew + "');");
                                // hf_MAST_PRODUCT_CATG_KEY.Value = Convert.ToString(vKey);
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgValidation + "');");
                                //   MessageBox(2, Message.msgValidation, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                //  MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                               
                        }
                        else
                        {
                            vRef = oBMC.SaveChanges<Object, Int32>("UPDATE", oBMAST, null, ref vKey, ref errMsg, "2019", 1);
                            if (vRef == 1)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveEdit + "');");
                                //MessageBox(2, Message.msgSaveEdit, "");
                                //errMsg = FillMastProductEdit(oBMAST.MAST_CATEGORY_KEY);
                                //hf_MAST_PRODUCT_CATG_KEY.Value = Convert.ToString(vKey);
                            }
                            else if (vRef == 2)
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                //  MessageBox(2, Message.msgSaveDuplicate, errMsg);
                            }

                            else
                            {
                                TempData["JavaScriptFunction"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                //  MessageBox(2, Message.msgSaveErr, errMsg);
                            }
                               
                        }
                    }
                    //oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
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

            return Redirect("~/ProductCategory/Index");

        }

    }
}