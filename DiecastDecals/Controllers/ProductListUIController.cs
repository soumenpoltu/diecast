using MyApp.db.MyAppBal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MyApp.db.Encryption;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.Entity;

namespace DiecastDecals.Controllers
{
    public class ProductListUIController : CommonController
    {
        // GET: ProductList
        String errMsg = String.Empty;
        String vString = String.Empty;
        DataTable dt;
        DataTable dt2;
        DataSet ds;
        public ActionResult Index(string CATG = null, string SUBCATG = null, string SUBSUBCATG = null)
        {
           
            FillMastProductCatgGrid();
            FillMastAllProduct();
            layoutpopulate();
            if (CATG != null)
            {
                ViewBag.categoryhf = Encrypted.Decryptdata( CATG);
                TempData["JavaScriptMsg"] = string.Format("checkboxcategory();");
            }
            if (SUBCATG != null)
            {
                ViewBag.categoryhf = Encrypted.Decryptdata(SUBCATG);
                TempData["JavaScriptMsg"] = string.Format("checkboxsubcategory();");
            }
            if (SUBSUBCATG != null)
            {
                ViewBag.categoryhf = Encrypted.Decryptdata(SUBSUBCATG);
                TempData["JavaScriptMsg"] = string.Format("checkboxsubsubcategory();");
            }
            return View();
        }


        [HttpGet]
        public ActionResult search(string id)
        {
            try
            {

                errMsg = String.Empty;
                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt2 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt2 = oBMC.Get<Int32>("GET_HEADER_PRO", 0, id, ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    Session["PRODUCT"] = dt2;

                }
                if (dt2 != null && dt2.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";
                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt2.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";


                    }

                    ViewBag.Products = vString;


                }
                else
                {
                    using (BAProduct oBMC = new BAProduct())
                    {
                        dt2 = oBMC.Get<Int32>("GET_PRO", 0, "", ref errMsg, "2019", 1);
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                        Session["PRODUCT"] = dt2;

                    }
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);


                    vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";
                    ViewBag.Products = vString;
                }

                
            }
            catch (Exception ex)
            {
               // return ex.Message;
            }
            layoutpopulate();
            FillMastProductCatgGrid();
            return View("Index");

        }

        private String FillMastProductCatgGrid()
        {
            try
            {

              
                errMsg = String.Empty;
                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                ds = new DataSet();
                using (BAProductCat oBMC = new BAProductCat())
                {
                    ds = oBMC.DsGet<Int32>("GET_CATG", 0, "", ref errMsg, "2019", 1);
                    Session["CATG_LIST"] = ds;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                    
                }


                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                DataRow[] SUB_CAT;

                DataRow[] SUB_SUB_CAT;

                DataRow[] PRO_CAT;


                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    // vString += "<ul>";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        vString += "<li>";
                        
                        PRO_CAT = ds.Tables[3].Select("MAST_CATEGORY_KEY = " + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "");
                        SUB_CAT = ds.Tables[1].Select("MAST_CATEGORY_KEY = " + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "");
                        if (SUB_CAT.Length > 0)
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "<span> (" + ds.Tables[0].Rows[i]["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                            vString += "<ul>";

                            for (int SC = 0; SC < SUB_CAT.Length; SC++)
                            {
                                vString += "<li>";

                                vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";
                                SUB_SUB_CAT = ds.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                if (SUB_SUB_CAT.Length > 0)
                                {
                                    vString += "<ul>";
                                    for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                    {

                                        vString += "<li>";
                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";
                                      
                                        vString += "</li>";


                                    }



                                    vString += "</ul>";
                                }

                                vString += "</li>";
                            }

                            vString += "</ul>";


                        }
                        else
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "<span> (" + ds.Tables[0].Rows[i]["COUNTPRODUCT"] + ")</span></a></label>";

                        }

                        vString += "</li>";
                    }

                    ViewBag.category = vString;
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


        private String FillMastAllProduct()
        {
            try
            {

                errMsg = String.Empty;
                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt2 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt2 = oBMC.Get<Int32>("GET_PRO", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    Session["PRODUCT"] = dt2;

                }
                if (dt2 != null && dt2.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";

                        }
                        else
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                        }

                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt2.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";


                    }

                    ViewBag.Products = vString;


                }
                else
                {
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);


                    vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";
                    ViewBag.Products = vString;
                }

                return errMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public JsonResult filter(string categorykey)
        {

            EntityJavaScriptPopulate jspopulate = new EntityJavaScriptPopulate();

            if (categorykey != null && categorykey != "0")
            {

                string[] categoryname = categorykey.Split(',');
                List<string> selected_category = new List<string>(categoryname);
                DataSet categorylist = new DataSet();
                categorylist = (DataSet)Session["CATG_LIST"];
          
                int i = 0;
                DataRow[] SUB_CAT;

                DataRow[] SUB_SUB_CAT;

                DataRow[] PRO_CAT;


                foreach (DataRow o in categorylist.Tables[0].Rows)
                {
                    i = i + 1;
                    int count = 0;
                    vString += "<li>";

                    SUB_CAT = categorylist.Tables[1].Select("MAST_CATEGORY_KEY = " + o["MAST_CATEGORY_KEY"] + "");

                    foreach (var item in selected_category)
                    {

                        if (o["MAST_CATEGORY_KEY"].ToString() == item)
                        {
                            if (SUB_CAT.Length > 0)
                            {
                                vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' checked onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                                vString += "<ul>";

                                for (int SC = 0; SC < SUB_CAT.Length; SC++)
                                {
                                    vString += "<li>";

                                    vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";
                                    SUB_SUB_CAT = categorylist.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                    if (SUB_SUB_CAT.Length > 0)
                                    {
                                        vString += "<ul>";
                                        for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                        {

                                            vString += "<li>";
                                            vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                            vString += "</li>";


                                        }



                                        vString += "</ul>";
                                    }

                                    vString += "</li>";
                                }

                                vString += "</ul>";


                            }
                            else
                            {
                                vString += "<span class='filtercheckbox'><input type='checkbox' checked name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label>";

                            }

                            //vString += "<li>";
                            //vString += "<span><input type='checkbox' onchange='checkboxcategory();' checked name='MainCATG' value='" + o["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                            //vString += "<label><a href='#'>" + o["CATEGORY_NAME"].ToString() + "<span> (" + o["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                            //vString += "</li>";
                            //categorylist.Rows.Remove(o);
                            selected_category.RemoveAt(0);
                            i++;
                            //categoryname = selected_category.ToArray();
                            count++;

                           

                                break;

                        }
                        else
                        {

                        }


                    }
                    if (count == 0)
                    {
                        if (SUB_CAT.Length > 0)
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                            vString += "<ul>";

                            for (int SC = 0; SC < SUB_CAT.Length; SC++)
                            {
                                vString += "<li>";

                                vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";
                                SUB_SUB_CAT = categorylist.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                if (SUB_SUB_CAT.Length > 0)
                                {
                                    vString += "<ul>";
                                    for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                    {

                                        vString += "<li>";
                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                        vString += "</li>";


                                    }



                                    vString += "</ul>";
                                }

                                vString += "</li>";
                            }

                            vString += "</ul>";

                        }

                        else
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label>";

                        }



                        //vString += "<li>";
                        //vString += "<span><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                        //vString += "<label><a href='#'>" + o["CATEGORY_NAME"].ToString() + "<span> (" + o["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                        //vString += "</li>";

                    }


                    vString += "</li>";
                }
               


                jspopulate.CATEGORY_LIST = vString;


                DataTable product = new DataTable();
                product = (DataTable)Session["PRODUCT"];
                vString = string.Empty;

                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);


                foreach (var item in categoryname)
                {
                    foreach (DataRow o in product.Select("MAST_CATEGORY_KEY = " + item + ""))
                    {
                        if (Convert.ToString(o["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(o["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";
                        }
                        else
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                           

                        }

                   
                       
                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(o["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";

                    }

                }

                if (vString == "")
                {
                    
                        vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";
                    
                }

                jspopulate.PRODUCT_LIST = vString;

            }
            else
            {

                /////for all category/////


                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                //dt = new DataTable();
                //using (BAProductCat oBMC = new BAProductCat())
                //{
                //    dt = oBMC.Get<Int32>("GET_CATG", 0, "", ref errMsg, "2019", 1);
                //    Session["CATG_LIST"] = dt;
                //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                //}

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    vString += " <span class='cat-title'>Categories</span><ul>";
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        vString += "<li>";
                //        vString += "<span><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + dt.Rows[i]["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                //        vString += "<label><a href='#'>" + dt.Rows[i]["CATEGORY_NAME"].ToString() + "<span> (" + dt.Rows[i]["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                //        vString += "</li>";
                //    }
                //    vString += "</ul>";

                //   jspopulate.CATEGORY_LIST = vString;

                //}



                ds = new DataSet();
                using (BAProductCat oBMC = new BAProductCat())
                {
                    ds = oBMC.DsGet<Int32>("GET_CATG", 0, "", ref errMsg, "2019", 1);
                    Session["CATG_LIST"] = ds;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                }


                //string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                DataRow[] SUB_CAT;

                DataRow[] SUB_SUB_CAT;

                DataRow[] PRO_CAT;

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    // vString += "<ul>";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        vString += "<li>";

                        PRO_CAT = ds.Tables[3].Select("MAST_CATEGORY_KEY = " + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "");
                        SUB_CAT = ds.Tables[1].Select("MAST_CATEGORY_KEY = " + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "");
                        if (SUB_CAT.Length > 0)
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "<span> (" + ds.Tables[0].Rows[i]["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                            vString += "<ul>";

                            for (int SC = 0; SC < SUB_CAT.Length; SC++)
                            {
                                vString += "<li>";

                                vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";
                                SUB_SUB_CAT = ds.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                if (SUB_SUB_CAT.Length > 0)
                                {
                                    vString += "<ul>";
                                    for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                    {

                                        vString += "<li>";
                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                        vString += "</li>";


                                    }



                                    vString += "</ul>";
                                }

                                vString += "</li>";
                            }

                            vString += "</ul>";


                        }
                        else
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "<span> (" + ds.Tables[0].Rows[i]["COUNTPRODUCT"] + ")</span></a></label>";

                        }

                        vString += "</li>";
                    }
               
                    jspopulate.CATEGORY_LIST = vString;
                }


                ///









                ///for all product///

                errMsg = String.Empty;
                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt2 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt2 = oBMC.Get<Int32>("GET_PRO", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    Session["PRODUCT"] = dt2;

                }
                if (dt2 != null && dt2.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";

                        }
                        else
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                        }
                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt2.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";


                    }




                    jspopulate.PRODUCT_LIST = vString;


                }
                else
                {
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";
                    jspopulate.PRODUCT_LIST = vString;

                }

                ///

            }

            return Json(jspopulate);




        }



        [HttpPost]
        public JsonResult Subfilter(string categorykey)
        {

            EntityJavaScriptPopulate jspopulate = new EntityJavaScriptPopulate();

            if (categorykey != null && categorykey != "0")
            {

                string[] categoryname = categorykey.Split(',');
                List<string> selected_category = new List<string>(categoryname);
                DataSet categorylist = new DataSet();
                categorylist = (DataSet)Session["CATG_LIST"];
                
                int i = 0;
                DataRow[] SUB_CAT;

                DataRow[] SUB_SUB_CAT;

                DataRow[] PRO_CAT;


                foreach (DataRow o in categorylist.Tables[0].Rows)
                {
                    i = i + 1;
                    int count = 0;
                    vString += "<li>";

                    SUB_CAT = categorylist.Tables[1].Select("MAST_CATEGORY_KEY = " + o["MAST_CATEGORY_KEY"] + "");

                    foreach (var item in selected_category)
                    {
                        DataRow[] maincatg = categorylist.Tables[1].Select("SUB_CATEGORY_KEY = " + item + "");
                        string maincatgval = maincatg[0]["MAST_CATEGORY_KEY"].ToString();

                        if (o["MAST_CATEGORY_KEY"].ToString() == maincatgval)
                        {
                            if (SUB_CAT.Length > 0)
                            {
                                vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' checked onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                                vString += "<ul>";

                                for (int SC = 0; SC < SUB_CAT.Length; SC++)
                                {

                                    vString += "<li>";


                                    if (SUB_CAT[SC]["SUB_CATEGORY_KEY"].ToString() == item)
                                    {
                                        vString += "<span class='filtercheckbox'><input type='checkbox' checked name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                                    }
                                    else
                                    {

                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                                    }
                                    SUB_SUB_CAT = categorylist.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                    if (SUB_SUB_CAT.Length > 0)
                                    {
                                        vString += "<ul>";
                                        for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                        {

                                            vString += "<li>";
                                            vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                            vString += "</li>";


                                        }



                                        vString += "</ul>";
                                    }

                                    vString += "</li>";
                                }

                                vString += "</ul>";


                            }
                            else
                            {
                                vString += "<span class='filtercheckbox'><input type='checkbox' checked name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label>";

                            }

                            //vString += "<li>";
                            //vString += "<span><input type='checkbox' onchange='checkboxcategory();' checked name='MainCATG' value='" + o["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                            //vString += "<label><a href='#'>" + o["CATEGORY_NAME"].ToString() + "<span> (" + o["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                            //vString += "</li>";
                            //categorylist.Rows.Remove(o);
                            selected_category.RemoveAt(0);
                            i++;
                            //categoryname = selected_category.ToArray();
                            count++;



                            break;

                        }
                        else
                        {

                        }


                    }
                    if (count == 0)
                    {
                        if (SUB_CAT.Length > 0)
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                            vString += "<ul>";

                            for (int SC = 0; SC < SUB_CAT.Length; SC++)
                            {
                                vString += "<li>";

                                vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";
                                SUB_SUB_CAT = categorylist.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                if (SUB_SUB_CAT.Length > 0)
                                {
                                    vString += "<ul>";
                                    for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                    {

                                        vString += "<li>";
                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                        vString += "</li>";


                                    }



                                    vString += "</ul>";
                                }

                                vString += "</li>";
                            }

                            vString += "</ul>";

                        }

                        else
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label>";

                        }



                        //vString += "<li>";
                        //vString += "<span><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                        //vString += "<label><a href='#'>" + o["CATEGORY_NAME"].ToString() + "<span> (" + o["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                        //vString += "</li>";

                    }


                    vString += "</li>";
                }


                //for (int i = 0; i < categorylist.Rows.Count; i++)
                //{

                //    vString += "<li>";
                //    vString += "<span><input type='checkbox' name='CATG' onchange='checkboxcategory();' value='" + categorylist.Rows[i]["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                //    vString += "<label><a href='#'>" + categorylist.Rows[i]["CATEGORY_NAME"].ToString() + "<span> (" + categorylist.Rows[i]["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                //    vString += "</li>";
                //}

                

                jspopulate.CATEGORY_LIST = vString;


                DataTable product = new DataTable();
                product = (DataTable)Session["PRODUCT"];
                vString = string.Empty;

                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);


                foreach (var item in categoryname)
                {
                    foreach (DataRow o in product.Select("SUB_CATEGORY_KEY = " + item + ""))
                    {
                        if (Convert.ToString(o["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(o["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";
                        }
                        else
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";



                        }

                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(o["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";

                    }

                }

                if (vString == "")
                {

                    vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";

                }

                jspopulate.PRODUCT_LIST = vString;

            }
            else
            {

                /////for category/////


                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BAProductCat oBMC = new BAProductCat())
                {
                    dt = oBMC.Get<Int32>("GET_CATG", 0, "", ref errMsg, "2019", 1);
                    Session["CATG_LIST"] = dt;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    vString += " <span class='cat-title'>Categories</span><ul>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vString += "<li>";
                        vString += "<span><input type='checkbox' name='CATG' onchange='checkboxcategory();' value='" + dt.Rows[i]["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                        vString += "<label><a href='#'>" + dt.Rows[i]["CATEGORY_NAME"].ToString() + "<span> (" + dt.Rows[i]["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                        vString += "</li>";
                    }
                    vString += "</ul>";

                    jspopulate.CATEGORY_LIST = vString;

                }






                ///









                ///for all product///

                errMsg = String.Empty;
                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt2 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt2 = oBMC.Get<Int32>("GET_PRO", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    Session["PRODUCT"] = dt2;

                }
                if (dt2 != null && dt2.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";

                        }
                        else
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                        }
                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt2.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";


                    }




                    jspopulate.PRODUCT_LIST = vString;


                }
                else
                {
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";
                    jspopulate.PRODUCT_LIST = vString;

                }

                ///

            }

            return Json(jspopulate);




        }

        [HttpPost]
        public JsonResult SubSUbfilter(string categorykey)
        {

            EntityJavaScriptPopulate jspopulate = new EntityJavaScriptPopulate();

            if (categorykey != null && categorykey != "0")
            {
                string[] categoryname = categorykey.Split(',');
                List<string> selected_category = new List<string>(categoryname);
                DataSet categorylist = new DataSet();
                categorylist = (DataSet)Session["CATG_LIST"];
               
                int i = 0;
                DataRow[] SUB_CAT;

                DataRow[] SUB_SUB_CAT;

                DataRow[] PRO_CAT;


                foreach (DataRow o in categorylist.Tables[0].Rows)
                {
                    i = i + 1;
                    int count = 0;
                    vString += "<li>";

                    SUB_CAT = categorylist.Tables[1].Select("MAST_CATEGORY_KEY = " + o["MAST_CATEGORY_KEY"] + "");

                    foreach (var item in selected_category)
                    {
                        DataRow[] maincatg = categorylist.Tables[2].Select("SUB_SUB_CATEGORY_KEY = " + item + "");
                        string maincatgval = maincatg[0]["MAST_CATEGORY_KEY"].ToString();
                        string subcatgval = maincatg[0]["SUB_CATEGORY_KEY"].ToString();
                        if (o["MAST_CATEGORY_KEY"].ToString() == maincatgval)
                        {


                            if (SUB_CAT.Length > 0)
                            {


                                vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' checked onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                                vString += "<ul>";

                                for (int SC = 0; SC < SUB_CAT.Length; SC++)
                                {
                                    vString += "<li>";


                                    if (SUB_CAT[SC]["SUB_CATEGORY_KEY"].ToString() == subcatgval)
                                    {

                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' checked onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                                    }
                                    else
                                    {

                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                                    }

                                    SUB_SUB_CAT = categorylist.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                    if (SUB_SUB_CAT.Length > 0)
                                    {
                                        vString += "<ul>";
                                        for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                        {

                                            vString += "<li>";
                                            if (SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"].ToString() == item)
                                            {

                                                vString += "<span class='filtercheckbox'><input type='checkbox' checked name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                            }
                                            else
                                            {

                                                vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                            }

                                            vString += "</li>";


                                        }



                                        vString += "</ul>";
                                    }

                                    vString += "</li>";
                                }

                                vString += "</ul>";


                            }
                            else
                            {
                                vString += "<span class='filtercheckbox'><input type='checkbox' checked name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label>";

                            }

                            //vString += "<li>";
                            //vString += "<span><input type='checkbox' onchange='checkboxcategory();' checked name='MainCATG' value='" + o["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                            //vString += "<label><a href='#'>" + o["CATEGORY_NAME"].ToString() + "<span> (" + o["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                            //vString += "</li>";
                            //categorylist.Rows.Remove(o);
                            selected_category.RemoveAt(0);
                            i++;
                            //categoryname = selected_category.ToArray();
                            count++;



                            break;

                        }
                        else
                        {

                        }


                    }
                    if (count == 0)
                    {
                        if (SUB_CAT.Length > 0)
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label><i id='CAT" + i + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";

                            vString += "<ul>";

                            for (int SC = 0; SC < SUB_CAT.Length; SC++)
                            {
                                vString += "<li>";

                                vString += "<span class='filtercheckbox'><input type='checkbox' name='SubCATG' onchange='checkboxsubcategory();' value='" + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "<span> (" + SUB_CAT[SC]["COUNTPRODUCT"] + ")</span></a> </label><i id='SUBCAT" + SC + "' onclick='showulli(this.id);' class='fa fa-plus-square-o' aria-hidden='true'></i>";
                                SUB_SUB_CAT = categorylist.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                                if (SUB_SUB_CAT.Length > 0)
                                {
                                    vString += "<ul>";
                                    for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                    {

                                        vString += "<li>";



                                        vString += "<span class='filtercheckbox'><input type='checkbox' name='SubSubCATG' onchange='checkboxsubsubcategory();' value='" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "<span> (" + SUB_SUB_CAT[SSC]["COUNTPRODUCT"] + ")</span></a> </label>";

                                        vString += "</li>";


                                    }



                                    vString += "</ul>";
                                }

                                vString += "</li>";
                            }

                            vString += "</ul>";

                        }

                        else
                        {
                            vString += "<span class='filtercheckbox'><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"] + "' class='checkbox'></span><label><a href='#'>" + o["CATEGORY_NAME"] + "<span> (" + o["COUNTPRODUCT"] + ")</span></a></label>";

                        }



                        //vString += "<li>";
                        //vString += "<span><input type='checkbox' name='MainCATG' onchange='checkboxcategory();' value='" + o["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                        //vString += "<label><a href='#'>" + o["CATEGORY_NAME"].ToString() + "<span> (" + o["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                        //vString += "</li>";

                    }


                    vString += "</li>";
                }
                //for (int i = 0; i < categorylist.Rows.Count; i++)
                //{

                //    vString += "<li>";
                //    vString += "<span><input type='checkbox' name='CATG' onchange='checkboxcategory();' value='" + categorylist.Rows[i]["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                //    vString += "<label><a href='#'>" + categorylist.Rows[i]["CATEGORY_NAME"].ToString() + "<span> (" + categorylist.Rows[i]["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                //    vString += "</li>";
                //}

                jspopulate.CATEGORY_LIST = vString;


                DataTable product = new DataTable();
                product = (DataTable)Session["PRODUCT"];
                vString = string.Empty;

                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);


                foreach (var item in categoryname)
                {
                    foreach (DataRow o in product.Select("SUB_SUB_CATEGORY_KEY = " + item + ""))
                    {
                        if (Convert.ToString(o["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(o["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";
                        }
                        else
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";



                        }

                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(o["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(o["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";

                    }

                }

                if (vString == "")
                {

                    vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";

                }

                jspopulate.PRODUCT_LIST = vString;

            }
            else
            {

                /////for category/////


                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt = new DataTable();
                using (BAProductCat oBMC = new BAProductCat())
                {
                    dt = oBMC.Get<Int32>("GET_CATG", 0, "", ref errMsg, "2019", 1);
                    Session["CATG_LIST"] = dt;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    vString += " <span class='cat-title'>Categories</span><ul>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vString += "<li>";
                        vString += "<span><input type='checkbox' name='CATG' onchange='checkboxcategory();' value='" + dt.Rows[i]["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                        vString += "<label><a href='#'>" + dt.Rows[i]["CATEGORY_NAME"].ToString() + "<span> (" + dt.Rows[i]["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                        vString += "</li>";
                    }
                    vString += "</ul>";

                    jspopulate.CATEGORY_LIST = vString;

                }






                ///









                ///for all product///

                errMsg = String.Empty;
                vString = String.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                dt2 = new DataTable();
                using (BAProduct oBMC = new BAProduct())
                {
                    dt2 = oBMC.Get<Int32>("GET_PRO", 0, "", ref errMsg, "2019", 1);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    Session["PRODUCT"] = dt2;

                }
                if (dt2 != null && dt2.Rows.Count > 0)
                {

                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) != "")
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + Convert.ToString(dt2.Rows[i]["PRODUCT_IMAGE"]) + "' alt='Product'></a></div>";

                        }
                        else
                        {
                            vString += "<div class='col-lg-3 col-md-6 col-sm-6 margin-bottom-30'><div class='featured-inner'><div class='featured-image'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'><img src='" + ConfigurationManager.AppSettings["PRODUCT"].ToString() + "no-product.jpg' alt='Product'></a></div>";

                        }
                        vString += "<div class='featured-info'><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(dt2.Rows[i]["HEAD_PRODUCT_KEY"])) + "'>" + Convert.ToString(dt2.Rows[i]["PRODUCT_NAME"]) + "</a>";
                        vString += "</div></div></div>";


                    }




                    jspopulate.PRODUCT_LIST = vString;


                }
                else
                {
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    vString += "<div class='col-md-12 noproductfound'><img src='/Content/main-assets/css/img/no-pdt.svg' /><h1>No Products Found</h1><a href='" + Domain + "/ProductListUI/Index'>Continue Shopping</a></div>";
                    jspopulate.PRODUCT_LIST = vString;

                }

                ///

            }

            return Json(jspopulate);




        }



    }
}