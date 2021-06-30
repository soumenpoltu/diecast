using MyApp.db.Encryption;
using MyApp.db.MyAppBal;
using MyApp.Entity;
using MyApp.Entity.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DiecastDecals.Controllers
{
    public class LoginUIController : CommonController
    {
        // GET: LoginUI

        EntityDtlsCustomer oSysUser = null;
        String errMsg = String.Empty;
        string customer_key;
        Int32 vRef = 0;
        DataTable dt;
        DataTable dt1;
        DataTable dt2;
        DataTable dt10;

        public ActionResult Index()
        {
            layoutpopulate();
            return View();
        }


        #region Register

        [HttpPost]
        public ActionResult btn_Register_click(FormCollection form)
        {
            Byte vRef = 0; Int32 vKey = 0;
            EntityDtlsCustomer oBMAST = null;
            EntityDtlsCustomer oSysUser = new EntityDtlsCustomer();
            dt10 = new DataTable();
            try
            {
                if (ModelState.IsValid)
                {
                    Random r = new Random();
                    string OTP = r.Next(100000, 999999).ToString();
                    Session["OTP"] = OTP;

                    errMsg = String.Empty;
                    oBMAST = new EntityDtlsCustomer();
                    oBMAST.CUSTOMER_KEY = Convert.ToInt32(ViewBag.hf_CUSTOMER_KEY);
                    oBMAST.CUSTOMER_FNAME = form["txt_First_Name"];
                    oBMAST.CUSTOMER_LNAME = form["txt_Last_Name"];
                    oBMAST.CUSTOMER_ADDRESS = form["txt_Address"];
                    oBMAST.CUSTOMER_CITY = form["txt_City"];
                    oBMAST.CUSTOMER_COUNTRY = form["txt_Country"];
                    oBMAST.CUSTOMER_PHONE = form["txt_Phone"];
                    oBMAST.CUSTOMER_PINCODE = form["txt_Pin"];

                    oBMAST.CUSTOMER_EMAIL = form["txt_Email"];
                    oBMAST.CUSTOMER_PASSWORD = Encrypted.Encryptdata(form["txt_Password"]);
                    oBMAST.CUSTOMER_CPASSWORD = Encrypted.Encryptdata(form["txt_CPassword"]);

                    oBMAST.ENT_USER_KEY = Convert.ToInt32(ViewBag.hf_CUSTOMER_KEY);
                    oBMAST.EDIT_USER_KEY = Convert.ToInt32(ViewBag.hf_CUSTOMER_KEY);

                    oBMAST.ACTIVATION_CODE = Convert.ToString(OTP);
                    oBMAST.TAG_DELETE = 0;
                    oBMAST.TAG_ACTIVATION = 0;

                    Session["mailid"] = oBMAST.CUSTOMER_EMAIL;

                    Session["CUSTOMER_FNAME"] = oBMAST.CUSTOMER_FNAME;
                    Session["CUSTOMER_PASSWORD"] = oBMAST.CUSTOMER_PASSWORD;

                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    using (BADtlsCustomer oBMC = new BADtlsCustomer())
                    {
                        if (oBMAST.CUSTOMER_KEY == 0)
                        {
                            if (form["txt_Password"] == form["txt_CPassword"])
                            {
                                if (SendEmailVerification(form, form["txt_Email"]))
                                {
                                    vRef = oBMC.SaveChanges<Object, Int32>("INSERT", oBMAST, null, ref vKey, ref errMsg, "", 1);
                                    dt10 = oBMC.Get<Int16>("TYPE_4", 0, form["txt_Email"], 1, ref errMsg, null, null);
                                    Session["custkey"] = Convert.ToString(dt10.Rows[0]["CUSTOMER_KEY"]);
                                    //TempData["CUSTOMER_KEY"] = dt1.Rows[0]["CUSTOMER_KEY"];

                                    if (vRef == 1)
                                    {
                                        TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveNew + "');");
                                        sendmail(form["txt_Email"], oBMAST.ACTIVATION_CODE);
                                        ClearControl(form);

                                        //MessageBox(2, Message.msgSaveNew, "");
                                        //  Response.Redirect("/index.aspx", false);
                                        //Context.ApplicationInstance.CompleteRequest();

                                    }
                                    else if (vRef == 2)
                                        TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                    //MessageBox(2, Message.msgSaveDuplicate, errMsg);
                                    else
                                        // MessageBox(2, Message.msgSaveErr, errMsg);
                                        TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveErr + "');");
                                }
                                else
                                {
                                    TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                                }

                            }
                            else
                            {
                                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
                            }
                        }
                    }
                }
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
            return Redirect("~/LoginUI/Index");

        }

        private bool SendEmailVerification(FormCollection form, String user_email)
        {

            DataTable dt = new DataTable();
            EntityDtlsCustomer oBMAST = new EntityDtlsCustomer();

            bool rt = true;

            using (BADtlsCustomer oBMC = new BADtlsCustomer())
            {
                if (form["txt_Email"] != oBMAST.CUSTOMER_EMAIL)
                {
                    dt = oBMC.Get<Int16>("TYPE_3", 0, form["txt_Email"], 1, ref errMsg, null, null);
                    if (dt.Rows.Count > 0)
                    {
                        rt = false;
                        TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgSaveDuplicate + "');");
                        //MessageBox(2, "Data Already Exist", errMsg);
                    }
                }
            }
            return rt;
        }

        private void ClearControl(FormCollection form)
        {
            form["txt_First_Name"] = "";
            form["txt_Last_Name"] = "";
            form["txt_Address"] = "";
            form["txt_City"] = "";
            form["txt_Country"] = "";
            form["txt_Phone"] = "";
            form["txt_Pin"] = "";
            form["txt_Email"] = "";
            form["txt_Password"] = "";
            form["txt_CPassword"] = "";

        }

        private String sendmail(String to_username, string code)
        {
            EntityDtlsCustomer oBMST = new EntityDtlsCustomer();
            //to_username = Convert.ToString(Session["mailid"]);
            //String emailId = txt_User_Email.Text;
            String EmailAddress = Encrypted.Encryptdata(to_username);
            try
            {


                // string to_username = ConfigurationManager.AppSettings["to_username"].ToString();
                string form_username = ConfigurationManager.AppSettings["form_username"].ToString();
                string form_password = ConfigurationManager.AppSettings["form_password"].ToString();
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true; //false


                using (MailMessage mail = new MailMessage())
                {


                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    ViewBag.DomainURL = Domain;

                    string strBody = string.Empty;
                    strBody += "<html><head></head><body>";
                    strBody += Environment.NewLine;
                    strBody += "Your Account Verification OTP is " + code;
                    strBody += "<br/>";
                    //strBody += "<a href=" + Domain + "/UserActivationUI/Index?ActivationCode=" + code.ToString()+ "'> Click the following link to activate your account.</a>";
                    //strBody += "<a href='" + Domain.Replace("/LoginUI/Index", "/UserActivationUI/Index?ActivationCode=" + code.ToString()) + "'>Click the following link to activate your account.</a>";
                    strBody += "<br/>Thanks from Diecast Decals team.</body></html>";

                    mail.From = new MailAddress(form_username,"Diecast Decals");
                    mail.To.Add(to_username);
                    mail.Subject = "User Activation Mail";
                    mail.Body = strBody.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(form_username, Encrypted.Decryptdata(form_password));
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                    TempData["JavaScriptMsg"] = string.Format("redirect();");
                    //ClientScript.RegisterStartupScript(GetType(), "popup", "popup();", true);
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("popupservererror();");
                //ClientScript.RegisterStartupScript(GetType(), "popupservererror", "popupservererror(); console.log('" + ex.Message + "');", true);
                return "fail";
            }

        }
        #endregion

        #region Login

        [HttpPost]
        public ActionResult btn_Login_click(FormCollection form1)
        {
            try
            {
                dt2 = new DataTable();
                //if (Session["oSysUser"] != null)
                //{
                //    oSysUser = (EntityDtlsCustomer)Session["oSysUser"];
                //    oSysUser.CUSTOMER_KEY = Convert.ToInt32(Session["custkey"]);

                //    string username = Convert.ToString( Session["CUSTOMER_FNAME"]);
                //    string password = Convert.ToString(Session["CUSTOMER_PASSWORD"]);

                //    errMsg = String.Empty;

                //    DataTable dt = null;
                //    Int32 vRef = 0;
                //    using (BADtlsCustomer oBSU = new BADtlsCustomer())
                //    {
                //        dt2 = oBSU.Get<Int16>("TYPE_1", 0, username, 1, ref errMsg, null, null);
                //        if (dt2 != null && dt2.Rows.Count > 0)
                //        {
                //            foreach (DataRow dr in dt2.Rows)
                //            {
                //                if (password == Convert.ToString(dr["CUSTOMER_PASSWORD"]))
                //                {
                //                    setUserSession(dr);
                //                    vRef = 1;
                //                }
                //            }

                //            if (vRef == 1)
                //                Response.Redirect("~/IndexUI/Index");
                //        }
                //    }


                //}

                using (BADtlsCustomer oBSU = new BADtlsCustomer())
                {
                    dt1 = oBSU.Get<Int16>("TYPE_3", 0, form1["txtemail"], 1, ref errMsg, null, 1);
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {

                    DataColumnCollection columns = dt1.Columns;
                    if (columns.Contains("RETURNCODE"))
                    {
                        using (BADtlsCustomer oBSU = new BADtlsCustomer())
                        {
                            dt10 = oBSU.Get<Int16>("TYPE_4", 0, form1["txtemail"], 1, ref errMsg, null, null);
                            Session["custkey"] = Convert.ToString(dt10.Rows[0]["CUSTOMER_KEY"]);
                            Random r = new Random();
                            string OTP = r.Next(100000, 999999).ToString();
                            Session["OTP"] = OTP;

                            errMsg = String.Empty;
                            EntityDtlsCustomer oBMAST;
                            oBMAST = new EntityDtlsCustomer();
                            oBMAST.ACTIVATION_CODE = Session["OTP"].ToString();
                            sendmail(form1["txtemail"], oBMAST.ACTIVATION_CODE);
                            Session["mailid"] = form1["txtemail"];
                        }

                        TempData["JavaScriptMsg"] = string.Format("notactive();");
                        return Redirect("~/LoginUI/Index");

                    }

                    else
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            if (Encrypted.Encryptdata(form1["txtpass"]) == Convert.ToString(dr["CUSTOMER_PASSWORD"]))
                            {
                                setUserSession(dr);

                                customer_key = Convert.ToString(dr["CUSTOMER_KEY"]);

                                string hash = "f0xle@rn";

                                byte[] data = UTF8Encoding.UTF8.GetBytes(customer_key);
                                using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                                {
                                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                                    using (System.Security.Cryptography.TripleDESCryptoServiceProvider tripDes = new System.Security.Cryptography.TripleDESCryptoServiceProvider() { Key = keys, Mode = System.Security.Cryptography.CipherMode.ECB, Padding = System.Security.Cryptography.PaddingMode.PKCS7 })
                                    {
                                        System.Security.Cryptography.ICryptoTransform transform = tripDes.CreateEncryptor();
                                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                                        customer_key = Convert.ToBase64String(results, 0, results.Length);
                                        Session["user"] = customer_key;
                                    }

                                }

                                HttpCookie objCookie = new HttpCookie("__asd");
                                objCookie["a"] = Encrypted.Encryptdata(form1["txtemail"]).ToString();
                                objCookie["p"] = Encrypted.EncryptASCII(form1["txtpass"]).ToString();
                                objCookie["fname"] = Encrypted.EncryptASCII(dr["CUSTOMER_FNAME"].ToString()).ToString();
                                objCookie["lname"] = Encrypted.EncryptASCII(dr["CUSTOMER_LNAME"].ToString()).ToString();
                                objCookie.Expires = DateTime.Now.AddDays(60);
                                Response.Cookies.Add(objCookie);
                                vRef = 1;
                            }

                        }
                        if (vRef == 1)
                        {
                            Response.Redirect("~/IndexUI/Index", false);
                            //Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            //MessageBox(Message.msgErr401, errMsg);
                            //  ClientScript.RegisterStartupScript(GetType(), "popupinvaid", "popupinvaid();", true);
                            TempData["JavaScriptMsg"] = string.Format("popupinvaid();");
                        }
                    }
                 

                }
                else
                {
                    // MessageBox(Message.msgErrPage, errMsg);
                    TempData["JavaScriptMsg"] = string.Format("popuperror();");

                    

                    return Redirect("~/LoginUI/Index");
                }


            }
            catch (Exception ex)
            {
                TempData["JavaScriptMsg"] = string.Format("OpenTab1('" + Message.msgErrCommon + "');");
            }

            return Redirect("~/IndexUI/Index");

        }

        private void setUserSession(DataRow dr)
        {
            oSysUser = new EntityDtlsCustomer();
            oSysUser.CUSTOMER_KEY = Convert.ToInt16(dr["CUSTOMER_KEY"]);
            oSysUser.CUSTOMER_FNAME = Convert.ToString(dr["CUSTOMER_FNAME"]);
            oSysUser.USER_TYPE_KEY = Convert.ToInt32(dr["USER_TYPE_KEY"]);
            oSysUser.CUSTOMER_EMAIL = Convert.ToString(dr["CUSTOMER_EMAIL"]);

            Session["oSysUser"] = oSysUser;
            Session["name"] = oSysUser.CUSTOMER_FNAME + " " + oSysUser.CUSTOMER_LNAME;
            Session["customerkey"] = oSysUser.CUSTOMER_KEY;
            Session["customeremail"] = oSysUser.CUSTOMER_EMAIL;
        }

        #endregion
    }
}