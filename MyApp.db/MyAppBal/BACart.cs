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
    public class BACart : IDisposable
    {

        Int16 vCount = 0;
        DataTable dt = null;
        DataSet ds = null;
        SqlParameter[] para = null;


        public byte SaveChanges<T1, T2>(String pMode, EntityCart oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[15];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@DTLS_CART_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.DTLS_CART_KEY;
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
                para[vCount] = new SqlParameter("@CUSTOMER_EMAIL", SqlDbType.VarChar,50);
                para[vCount++].Value = oEntity.CUSTOMER_EMAIL;




                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_CART", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));

                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }


        public byte SaveChangesCart<T1, T2>(String pMode, EntityCart oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[16];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = pValue;

                para[vCount] = new SqlParameter("@DTLS_CART_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.DTLS_CART_KEY;
                para[vCount] = new SqlParameter("@SHEET_CODE", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.SHEET_CODE;

                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@PRICE", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.PRICE;

                para[vCount] = new SqlParameter("@SHIPPING_AMOUNT", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.SHIPPING_AMOUNT;


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

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_CART", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));

                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }

        public DataTable Get<T>(String pMode, T pKey, String pSEARCH_TEXT, ref String pMsg, String pAccYr, Int16? pCompany_key)
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

                using (sqlhelper oDBC = new sqlhelper("SP_GET_DTLS_CART", CommandType.StoredProcedure))
                {
                    dt = oDBC.GetDataTable(pAccYr, pCompany_key, para, ref pMsg);
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return dt;
        }

        public Byte SaveDelete<T1, T2>(String pMode, EntityBlog oEntity, T1 pValue, ref T2 pKey, ref String pMsg, ref String pDelMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[4];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 30);
                para[vCount++].Value = "DELETE";
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@DTLS_CART_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.MAST_BLOG_KEY;

                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.SmallInt);
                para[vCount++].Value = oEntity.ENT_USER_KEY;

                using (sqlhelper oDBC = new sqlhelper("SP_DELETE_DTLS_CART", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@DTLS_CART_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }


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
