using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using MyApp.db.PayPalPayment;
using MyApp.db.MyAppBal;
using System.Data;
using MyApp.db.Encryption;
using MyApp.Entity;

namespace DiecastDecals.Controllers
{
    public class PaymentUIController : Controller
    {
        // GET: Payment
        String errMsg = String.Empty;
        DataTable dt;

        public ActionResult Index(string Cancel = null)
        {

            if (Cancel != null)
            {
                return Redirect("~/CartUI/Index");
            }
            else
            {
                APIContext apicontext = PayPalConfigaration.GetAPIContext();
                try
                {
                    string PayerId = Request.Params["PayerID"];

                    if (string.IsNullOrEmpty(PayerId))
                    {
                        TempData["InvoiceID"] = null;
                        if (SaveInvoice("", 0, "") == "1")
                        {
                            string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority +
                                             "/PaymentUI/Index?";
                            var guid = Convert.ToString((new Random()).Next(100000000));
                            var createPayment = this.CreatePayment(apicontext, baseURi + "guid=" + guid);

                            var links = createPayment.links.GetEnumerator();
                            string paypalRedirectURL = null;

                            while (links.MoveNext())
                            {
                                Links lnk = links.Current;

                                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                                {
                                    paypalRedirectURL = lnk.href;
                                }


                            }
                            Session.Add(guid, createPayment.id);
                            return Redirect(paypalRedirectURL);

                            //HttpContext.Current.ApplicationInstance.CompleteRequest();

                        }
                    }

                    else
                    {
                        string payerId = Request.Params["PayerID"];
                        var guid = Request.Params["guid"];
                        var executedPayment = ExecutePayment(apicontext, PayerId, Session[guid] as string);


                        if (executedPayment.state.ToLower() != "approved")
                        {

                        }
                        else
                        {
                            if (SaveInvoice(Session[guid] as string, 1, TempData.Peek("InvoiceID").ToString()) == "1")
                            {
                                return Redirect("~/Invoice/Index");
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    return View("FailureView");
                    //throw;
                }



            }

            return View("SucessView");
        }


        private Payment ExecutePayment(APIContext apicontext, string payerId, string PaymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = PaymentId };
            return this.payment.Execute(apicontext, paymentExecution);
        }

        private PayPal.Api.Payment payment;
        private Payment CreatePayment(APIContext apicontext, string redirectURl)
        {
            decimal total = 0;
            decimal subtotal = 0;
            decimal totaltax = 0;
            var ItemLIst = new ItemList() { items = new List<Item>() };

            HttpCookie objCookie = Request.Cookies["__asd"];

            dt = new DataTable();
            using (BACart oBMC = new BACart())
            {
                dt = oBMC.Get<Int32>("GET_PAY", 0, Encrypted.Decryptdata(objCookie["a"].ToString()).ToString(), ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }


            if (dt != null && dt.Rows.Count > 0)
            {

                totaltax = Convert.ToDecimal(dt.Rows[0]["TOTAL_TAX"]);
                Int32 totalqty = 0;
              
                String price = String.Empty;
               


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String qty = String.Empty;

                    total = total + Convert.ToDecimal(dt.Rows[i]["TOTAL_AMOUNT"]);
                    subtotal = subtotal + Convert.ToDecimal(dt.Rows[i]["SUB_TOTAL"]);
                    totalqty = totalqty + Convert.ToInt32(dt.Rows[i]["QUANTITY"]);


                    price = Convert.ToString(dt.Rows[i]["PRICE"]);
                        if (i + 1 == dt.Rows.Count)
                        {
                            if (totalqty > 7)
                            {
                            if (Convert.ToInt32(dt.Rows[i]["QUANTITY"])==1)
                            {
                                price = "0";
                                qty = dt.Rows[i]["QUANTITY"].ToString();
                            }
                            else
                            {
                                qty = (Convert.ToInt32(dt.Rows[i]["QUANTITY"]) - 1).ToString();
                            }

                            }
                            else
                            {
                                qty = dt.Rows[i]["QUANTITY"].ToString();
                            }

                        }
                        else
                        {
                        qty = dt.Rows[i]["QUANTITY"].ToString();
                        }

                    

                    ItemLIst.items.Add(new Item()
                    {

                        name = dt.Rows[i]["PRODUCT"].ToString(),
                        currency = "AUD",
                        price = price,

                        quantity = qty
                    });

                }



                var payer = new Payer() { payment_method = "paypal" };

                var redirUrl = new RedirectUrls()
                {
                    cancel_url = redirectURl + "&Cancel=true",
                    return_url = redirectURl
                };

                var details = new Details()
                {

                    tax = totaltax.ToString(),
                    shipping = total > 120 ? "0" : "10.82",
                    subtotal = subtotal.ToString()
                };

                var amount = new Amount()
                {
                    currency = "AUD",

                    total = total > 120 ? total.ToString() : Convert.ToString(Convert.ToDecimal(total) + Convert.ToDecimal(10.82)),
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Transaction Description",
                    //invoice_number = "#100000",
                    amount = amount,
                    item_list = ItemLIst

                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };
            }



            return this.payment.Create(apicontext);

        }

        private string SaveInvoice(string transactionid, Byte paymentstatus, String pValue)
        {

            HttpCookie objCookie = Request.Cookies["__asd"];

            Byte vRef = 0; Int32 vKey = 0;

            EntityCart oBMAST = null;
            EntityDtlsCustomer oBMAST1 = null;
            dt = new DataTable();
            using (BACart oBMC = new BACart())
            {
                dt = oBMC.Get<Int32>("GET_INVOICE_CART", 0, Encrypted.Decryptdata(objCookie["a"].ToString()).ToString(), ref errMsg, "2019", 1);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            }

            if (dt != null)
            {


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ModelState.IsValid)
                    {

                        errMsg = String.Empty;
                        oBMAST = new EntityCart();
                        oBMAST1 = new EntityDtlsCustomer();

                        if (Session["oBMAST"] != null)
                        {
                            oBMAST1 = (EntityDtlsCustomer)Session["oBMAST"];
                            oBMAST.SHIPPING_ADDRESS_KEY = oBMAST1.CUSTOMER_ADRESS_KEY;
                            oBMAST.BILLING_ADDRESS_KEY = oBMAST1.BILLING_CUSTOMER_ADRESS_KEY;
                        }
                        oBMAST.TRANSACTION_ID = transactionid;
                        oBMAST.PAYMENT_STATUS = paymentstatus;
                        if (TempData["InvoiceID"] != null)
                        {
                            oBMAST.INVOICE = TempData["InvoiceID"].ToString();

                        }
                        else
                        {
                            oBMAST.INVOICE = pValue;

                        }


                        oBMAST.SHEET_CODE = Convert.ToString(dt.Rows[i]["SHEET_CODE"]);
                        oBMAST.HEAD_PRODUCT_KEY = Convert.ToInt32(dt.Rows[i]["HEAD_PRODUCT_KEY"]);
                        oBMAST.PRICE = Convert.ToDecimal(dt.Rows[i]["PRICE"]);
                        oBMAST.QUANTITY = Convert.ToInt32(dt.Rows[i]["QUANTITY"]);
                        oBMAST.TAX = Convert.ToDecimal(dt.Rows[i]["TAX"]);
                        oBMAST.GROSS_AMOUNT = Convert.ToDecimal(dt.Rows[i]["SUB_TOTAL"]);
                        oBMAST.TOTAL_AMOUNT = Convert.ToDecimal(dt.Rows[i]["TOTAL_AMOUNT"]);
                        oBMAST.CUSTOMER_EMAIL = Encrypted.Decryptdata(objCookie["a"]);
                        oBMAST.SHIPPING_AMOUNT = Convert.ToDecimal(dt.Rows[i]["SHIPPING_AMOUNT"]);

                        oBMAST.ENT_USER_KEY = Convert.ToInt32(0);
                        oBMAST.EDIT_USER_KEY = Convert.ToInt32(0);
                        oBMAST.TAG_ACTIVE = 1;
                        oBMAST.TAG_DELETE = 0;
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                        using (BAInvoice oBMC = new BAInvoice())
                        {
                            if (pValue == "")
                            {
                                vRef = oBMC.SaveChanges<string>("INVOICE", oBMAST, ref pValue, ref errMsg, "2019", 1);
                                if (vRef == 1)
                                {
                                    TempData["InvoiceID"] = pValue;
                                    TempData.Peek("InvoiceID");
                                    pValue = "";
                                    continue;


                                }
                                else if (vRef == 2)
                                {
                                    return "0";

                                }
                                else
                                {
                                    return "0";

                                }
                            }
                            else
                            {
                                vRef = oBMC.SaveChanges<string>("UPDATE", oBMAST, ref pValue, ref errMsg, "2019", 1);
                                if (vRef == 1)
                                {
                                    Session["InvoiceID"] = pValue;
                                    TempData["InvoiceID"] = null;
                                    break;

                                }
                                else if (vRef == 2)
                                {
                                    return "0";

                                }
                                else
                                {
                                    return "0";

                                }
                            }





                        }
                    }
                }
            }


            return "1";

        }

    }
}