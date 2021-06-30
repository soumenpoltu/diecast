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
    public class OrderDetailsController : CommonController
    {
        // GET: OrderDetails
        // GET: Blog

        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;
        DataTable dt1;
        DataSet ds;
        
        public ActionResult Index()
        {
            try
            {
                if (Session["oSysUser"] != null)
                {
                    layoutpopulate();
                    oSysUser = (EntitySysUser)Session["oSysUser"];
                    oSysUser.USER_KEY = Convert.ToInt32(Session["USER_KEY"]);

                    oSysUser.TAG_PAGE_REFRESH = Server.UrlEncode(System.DateTime.Now.ToString());
                    errMsg = String.Empty;

                    dt = FillBlogGrid();
                    getorder("0");

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


        private DataTable FillBlogGrid()
        {
            try
            {
                errMsg = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                ds = new DataSet();


                using(BAInvoice oBMC = new BAInvoice())
                {
                    ds = oBMC.Get<Int32>("GET_ORDER", 0,"", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                }
                dt = ds.Tables[0];
             
            }
            catch (Exception ex)
            {
                // return ex.Message;
            }
            return dt;

        }


        [HttpPost]
        public ActionResult view(FormCollection form)
        {
            try
            {
                string edit = form[0];
                errMsg = String.Empty;

                ViewBag.hf_MAST_BLOG_KEY = edit.ToString();
                getorder(Convert.ToString(edit));
                FillBlogGrid();
          
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


        private void getorder(string invoice)
        {
            ds = new DataSet();
            String productname = string.Empty;
            using (BAInvoice oBMC = new BAInvoice())
            {
                ds = oBMC.Get<Int32>("GET_INVOICE", 0, invoice, ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if (ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count>0)
                {
                    ViewBag.billaddr = ds.Tables[0].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[0].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                    ViewBag.inv = ds.Tables[0].Rows[0]["DTLS_INVOICE"].ToString();
                    ViewBag.invdate = ds.Tables[0].Rows[0]["INV_DATE"].ToString();
                }
             
            }
            if (ds.Tables[1] != null)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewBag.shipaddr = ds.Tables[1].Rows[0]["CUSTOMER_ADDRESS"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_CITY"].ToString() + ", " + ds.Tables[1].Rows[0]["CUSTOMER_COUNTRY"].ToString();
                }
            }
            dt1 = ds.Tables[2];
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                ViewBag.deliveredstatus  = dt1.Rows[0]["DELIVERED_STATUS"];
                ViewBag.tax = dt1.Rows[0]["TAX"];
                ViewBag.totalamnt = dt1.Rows[0]["TOTAL_AMOUNT"];
                ViewBag.subtotal = dt1.Rows[0]["SUB_TOTAL"];
                ViewBag.shipping = dt1.Rows[0]["SHIPPING_AMOUNT"];

            }

            ViewData["dt1"] = dt1;


        }


        [HttpPost]
        public ActionResult delivered(FormCollection form)
        {
            Int32 vKey = 0; Byte vRef = 0; String vDelMsg = String.Empty;

            EntityCart oBMAST = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string edit = form[0];
                    errMsg = String.Empty;
                    oBMAST = new EntityCart();

                    //GridViewRow gvr = (GridViewRow)((HtmlAnchor)sender).NamingContainer;
                    oBMAST.TRANSACTION_ID = "";
                    oBMAST.PAYMENT_STATUS = 1;
                    oBMAST.INVOICE = edit;


                    oBMAST.DTLS_PRODUCT_KEY = Convert.ToInt32(0);
                    oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(0);
                    oBMAST.PRICE = Convert.ToDecimal(0);
                    oBMAST.QUANTITY = Convert.ToInt32(0);
                    oBMAST.TAX = Convert.ToDecimal(0);
                    oBMAST.GROSS_AMOUNT = Convert.ToDecimal(0);
                    oBMAST.TOTAL_AMOUNT = Convert.ToDecimal(0);
                    oBMAST.CUSTOMER_EMAIL = "";


                    oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
                    oBMAST.TAG_ACTIVE = 1;
                    oBMAST.TAG_DELETE = 0;
                    using (BAInvoice oBMC = new BAInvoice())
                    {
                        vRef = oBMC.SaveChanges<string, Int32>("UPDATE_DELIVERED", oBMAST, ref vDelMsg, ref vKey, ref errMsg, "2019", 1);
                        if (vRef > 0)
                        {
                            if (vRef == 2)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                //  MessageBox(1, "Data exists in We Do Master", errMsg);
                            }
                            else if (vRef == 1)
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('Order Delivered');");
                                return Redirect("~/OrderDetails/Index");

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

            return Redirect("~/OrderDetails/Index");
        }




    }



}