using MyApp.db.Encryption;
using MyApp.Entity;
using MyApp.db;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyApp.db.MyAppBal
{
   public class BAInvoice : IDisposable
    {


        Int16 vCount = 0;
        DataTable dt = null;
        DataSet ds = null;
        SqlParameter[] para = null;


        public byte SaveChanges<T1, T2>(String pMode, EntityCart oEntity, ref T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[19];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@DTLS_INVOICE_ID", SqlDbType.Int);
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@DTLS_TRANSACTION", SqlDbType.VarChar , 25);
                para[vCount++].Value = oEntity.TRANSACTION_ID;

                para[vCount] = new SqlParameter("@PAYMENT_STATUS", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.PAYMENT_STATUS;


                para[vCount] = new SqlParameter("@SAME_BILLING_ADDRESS", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.SAME_BILLING_ADDRESS;


                para[vCount] = new SqlParameter("@INV", SqlDbType.Char, 12);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.INVOICE;


                para[vCount] = new SqlParameter("@DTLS_PRODUCT_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.DTLS_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;
                para[vCount] = new SqlParameter("@PRICE", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.PRICE;

                para[vCount] = new SqlParameter("@QUANTITY", SqlDbType.Int);
                para[vCount++].Value = oEntity.QUANTITY;
                para[vCount] = new SqlParameter("@TAX", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.TAX;

                para[vCount] = new SqlParameter("@GROSS_AMOUNT", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.GROSS_AMOUNT;
                para[vCount] = new SqlParameter("@TOTAL_AMOUNT", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.TOTAL_AMOUNT;
                para[vCount] = new SqlParameter("@CUSTOMER_EMAIL", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_EMAIL;




                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_INVOICE", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pValue = (T1)Convert.ChangeType(oDBC.GetParameterValue("@INV", ref pMsg), typeof(T1));

                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }

        public DataSet Get<T>(String pMode, T pKey, String pSEARCH_TEXT, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            try
            {
                vCount = 0;
                para = new SqlParameter[3];
                para[vCount] = new SqlParameter("@GET_REC_TYPE", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@SEARCH_KEY", SqlDbType.Int);
                para[vCount++].Value = pKey;
                para[vCount] = new SqlParameter("@SEARCH_TEXT", SqlDbType.VarChar, 50);
                para[vCount++].Value = pSEARCH_TEXT;

                using (sqlhelper oDBC = new sqlhelper("SP_GET_DTLS_INVOICE", CommandType.StoredProcedure))
                {
                    ds = oDBC.GetDataSet(pAccYr, pCompany_key, para, ref pMsg);
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return ds;
        }

        #region diecast decals

        public byte SaveChanges<T1>(String pMode, EntityCart oEntity, ref T1 pValue, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[21];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@DTLS_INVOICE_ID", SqlDbType.Int);
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@DTLS_TRANSACTION", SqlDbType.VarChar, 25);
                para[vCount++].Value = oEntity.TRANSACTION_ID;

                para[vCount] = new SqlParameter("@PAYMENT_STATUS", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.PAYMENT_STATUS;


                para[vCount] = new SqlParameter("@SHIPPING_ADDRESS_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.SHIPPING_ADDRESS_KEY;

                para[vCount] = new SqlParameter("@BILLING_ADDRESS_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.BILLING_ADDRESS_KEY;


                para[vCount] = new SqlParameter("@INV", SqlDbType.Char, 12);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.INVOICE;


                para[vCount] = new SqlParameter("@SHEET_CODE", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.SHEET_CODE;

                para[vCount] = new SqlParameter("@SHIPPING_AMOUNT", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.SHIPPING_AMOUNT;


                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;
                para[vCount] = new SqlParameter("@PRICE", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.PRICE;

                para[vCount] = new SqlParameter("@QUANTITY", SqlDbType.Int);
                para[vCount++].Value = oEntity.QUANTITY;
                para[vCount] = new SqlParameter("@TAX", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.TAX;

                para[vCount] = new SqlParameter("@GROSS_AMOUNT", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.GROSS_AMOUNT;
                para[vCount] = new SqlParameter("@TOTAL_AMOUNT", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.TOTAL_AMOUNT;
                para[vCount] = new SqlParameter("@CUSTOMER_EMAIL", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_EMAIL;




                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_INVOICE", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pValue = (T1)Convert.ChangeType(oDBC.GetParameterValue("@INV", ref pMsg), typeof(T1));

                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }

        #endregion

        public void Dispose()
        {
            if (dt != null)
            {
                dt.Dispose(); dt = null;
            }
            if (para != null)
                para = null;
        }


    }
}
