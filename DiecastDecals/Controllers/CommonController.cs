using MyApp.db.MyAppBal;
using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MyApp.db.Encryption;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        EntitySysUser oSysUser = null;
        String errMsg = String.Empty;
        DataTable dt;  //pagesetting
        DataTable dt1; //sitesetting
        DataSet ds;
        public void layoutpopulate()
        {
            checklogin();
            FillSiteSettingEdit();
            getcategory();
        }


        public String FillSiteSettingEdit()
        {
            try
            {
                String vCOMPANY_ACCESS = String.Empty;
                errMsg = String.Empty;
                dt1 = new DataTable();
                using (BAHeadSiteSetting oBME = new BAHeadSiteSetting())
                {
                    dt1 = oBME.GetData("ALL", "", ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ViewBag.txt_CONTACT_NO = Convert.ToString(dt1.Rows[0]["CONTACT_NO"]);
                    ViewBag.txt_MAIL = Convert.ToString(dt1.Rows[0]["MAIL"]);
                    ViewBag.txt_ADDRESS = Convert.ToString(dt1.Rows[0]["ADDRESS"]);

                    ViewBag.txt_FACEBOOK_LINK = Convert.ToString(dt1.Rows[0]["FACEBOOK_LINK"]);
                    ViewBag.txt_TWITTER_LINK = Convert.ToString(dt1.Rows[0]["TWITTER_LINK"]);
                    ViewBag.txt_PRINTEREST_LINK = Convert.ToString(dt1.Rows[0]["PRINTEREST_LINK"]);
                    ViewBag.txt_INSTAGRAM_LINK = Convert.ToString(dt1.Rows[0]["INSTAGRAM_LINK"]);

                    ViewBag.img_HEADER_LOGO = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["HEADER_LOGO"]);
                    ViewBag.HEADER_LOGO = Convert.ToString(dt1.Rows[0]["HEADER_LOGO"]);
                    ViewBag.img_FOOTER_LOGO = ConfigurationManager.AppSettings["HOME"].ToString() + Convert.ToString(dt1.Rows[0]["FOOTER_LOGO"]);
                    ViewBag.FOOTER_LOGO = Convert.ToString(dt1.Rows[0]["FOOTER_LOGO"]);
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

        public void checklogin()
        {
           
            HttpCookie objCookie = Request.Cookies["__asd"];
            if(objCookie != null)
            {
                ViewBag.customername = Encrypted.DecryptASCII(objCookie["fname"].ToString()).ToString();
                ViewBag.customerfullname = Encrypted.DecryptASCII(objCookie["fname"].ToString()).ToString() + " " + Encrypted.DecryptASCII(objCookie["lname"].ToString()).ToString();
                getcart(Encrypted.Decryptdata(objCookie["a"].ToString()).ToString());
                ViewBag.logindisplay = "style='display:inline-block;'";
                ViewBag.logoutdisplay = "style='display:none;'";
                ViewBag.moblogoutdisplay = "style='display:block;'";
                ViewBag.customerdisplay = "style='display:block;'";
                ViewBag.myaccountdisplay = "style='display:table;'";

            }
            else
            {
                ViewBag.cartitemscount = "0";
                ViewBag.cartitemprice = "0";
                ViewBag.customername = "Guest";
                ViewBag.logindisplay = "style='display:none;'";
                ViewBag.moblogoutdisplay = "style='display:none;'";
                ViewBag.logoutdisplay = "style='display:block;'";
                ViewBag.customerdisplay = "style='display:none;'";
                ViewBag.myaccountdisplay = "style='display:none;'";
            }
        }

        private void getcart(string useremail)
        {
            dt = new DataTable();

            using (BACart oBMC = new BACart())
            {
                dt = oBMC.Get<Int32>("GET_COUNT", 0, useremail, ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }
            if(dt!=null && dt.Rows.Count > 0)
            {
                ViewBag.cartitemscount = dt.Rows[0]["QUANTITY"];
                ViewBag.cartitemprice = dt.Rows[0]["TOTAL_AMOUNT"];
            }
        }

        private void getcategory()
        {
            String vString = String.Empty;

            ds = new DataSet();
            using (BAProductCat oBMC = new BAProductCat())
            {
                ds = oBMC.DsGet<Int32>("GET_CATG", 0, "", ref errMsg, "2019", 1);
         
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
                
                    PRO_CAT = ds.Tables[3].Select("MAST_CATEGORY_KEY = " + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "");
                    SUB_CAT = ds.Tables[1].Select("MAST_CATEGORY_KEY = " + ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"] + "");
                        if(SUB_CAT.Length > 0)
                        {
                        vString += "<li class='hasdropdown'>";
                        vString += "<a href='" + Domain + "/ProductlistUI/Index?CATG=" + Encrypted.Encryptdata(ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"].ToString()) + "'>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "</a> <i class='fa fa-angle-right' aria-hidden='true'></i>";
                        vString += "<ul>";
                        
                           for (int SC = 0; SC < SUB_CAT.Length; SC++)
                           {
                            
                            SUB_SUB_CAT = ds.Tables[2].Select("SUB_CATEGORY_KEY = " + SUB_CAT[SC]["SUB_CATEGORY_KEY"] + "");
                            if (SUB_SUB_CAT.Length > 0)
                            {
                                vString += "<li class='hasdropdown'>";

                                vString += " <a href='" + Domain + "/ProductlistUI/Index?SUBCATG=" + Encrypted.Encryptdata(SUB_CAT[SC]["SUB_CATEGORY_KEY"].ToString()) + "'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "</a> <i class='fa fa-angle-right' aria-hidden='true'></i>";

                                vString += "<ul>";
                                for (int SSC = 0; SSC < SUB_SUB_CAT.Length; SSC++)
                                {

                                    PRO_CAT = ds.Tables[3].Select("SUB_SUB_CATEGORY_KEY = " + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"] + "");
                                    if (PRO_CAT.Length > 0)
                                    {
                                        vString += "<li class='hasdropdown'>";
                                        vString += "<a href='" + Domain + "/ProductlistUI/Index?SUBSUBCATG=" + Encrypted.Encryptdata(SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"].ToString()) + "'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "</a> <i class='fa fa-angle-right' aria-hidden='true'></i>";

                                        vString += "<ul>";

                                        for (int PC = 0; PC < PRO_CAT.Length; PC++)
                                        {
                                            vString += "<li><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(PRO_CAT[PC]["HEAD_PRODUCT_KEY"])) + "'>" + PRO_CAT[PC]["PRODUCT_NAME"] + "</a></li>";

                                        }


                                        vString += "</ul>";

                                    }
                                    else
                                    {

                                        vString += "<li>";
                                        vString += "<a href='" + Domain + "/ProductlistUI/Index?SUBSUBCATG=" + Encrypted.Encryptdata(SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_KEY"].ToString()) + "'>" + SUB_SUB_CAT[SSC]["SUB_SUB_CATEGORY_NAME"] + "</a>";

                                    }
                                    vString += "</li>";


                                }



                                vString += "</ul>";
                            }
                            else
                            {
                                vString += "<li>";

                                vString += " <a href='" + Domain + "/ProductlistUI/Index?SUBCATG=" + Encrypted.Encryptdata(SUB_CAT[SC]["SUB_CATEGORY_KEY"].ToString()) + "'>" + SUB_CAT[SC]["SUB_CATEGORY_NAME"] + "</a>";

                            }
                            vString += "</li>";
                        }

                          vString += "</ul>";

                          
                        }
                  
                        else if(PRO_CAT.Length>0)
                        {
                        vString += "<li class='hasdropdown'>";
                        vString += "<a href='" + Domain + "/ProductlistUI/Index?CATG=" + Encrypted.Encryptdata(ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"].ToString()) + "'>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "</a> <i class='fa fa-angle-right' aria-hidden='true'></i>";
                        //vString = String.Empty;
                        //vString += "<li class='hasdropdown'>";
                        //vString += "<a href=''>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "</a> <i class='fa fa-angle-right' aria-hidden='true'></i>";
                        vString += "<ul>";
                        for (int PC = 0; PC < PRO_CAT.Length; PC++)
                            {
                            vString += "<li><a href='" + Domain + "/ProductNewUI/Index/" + Encrypted.Encryptdata(Convert.ToString(PRO_CAT[PC]["HEAD_PRODUCT_KEY"])) + "'>" + PRO_CAT[PC]["PRODUCT_NAME"] + "</a></li>";
                        }

                        vString += "</ul>";
                        //vString += "</li>";
                        }
                        else
                        {
                        vString += "<li>";
                        vString += "<a href='" + Domain + "/ProductlistUI/Index?CATG=" + Encrypted.Encryptdata(ds.Tables[0].Rows[i]["MAST_CATEGORY_KEY"].ToString()) + "'>" + ds.Tables[0].Rows[i]["CATEGORY_NAME"] + "</a>";
                        //vString = String.Empty;
                    }
                    //vString += "<li><a href='" + Domain + "/ProductlistUI/Index?CATG=" +Encrypted.Encryptdata( dt.Rows[i]["MAST_CATEGORY_KEY"].ToString()) + "'>" + dt.Rows[i]["CATEGORY_NAME"].ToString() + "</a></li>";
                    //vString += "<span><input type='checkbox' name='CATG' onchange='checkboxcategory();' value='" + dt.Rows[i]["MAST_CATEGORY_KEY"].ToString() + "' class='checkbox'></span>";
                    //vString += "<label><a href='#'>" + dt.Rows[i]["CATEGORY_NAME"].ToString() + "<span> (" + dt.Rows[i]["COUNTPRODUCT"].ToString() + ")</span></a> </label>";
                    //vString += "</li>";
                    vString += "</li>";
                }
             //   vString += "</ul>";

                ViewBag.Homme_CATEGORY_LIST = vString;

            }

        }

    }
}