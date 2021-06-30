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

   public class BAAddress : IDisposable
    {
        Int16 vCount = 0;
        DataTable dt = null;
        DataSet ds = null;
        SqlParameter[] para = null;

        public DataSet GetData(String pMode, String pSEARCH_TEXT, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            try
            {
                vCount = 0;
                para = new SqlParameter[3];
                para[vCount] = new SqlParameter("@GET_REC_TYPE", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@SEARCH_KEY", SqlDbType.Int);
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@SEARCH_TEXT", SqlDbType.VarChar, 50);
                para[vCount++].Value = pSEARCH_TEXT;

                using (sqlhelper oDBC = new sqlhelper("SP_GET_DTLS_CUSTOMER_ADDRESS", CommandType.StoredProcedure))
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


        public byte SaveChanges<T1, T2>(String pMode, EntityDtlsCustomer oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[17];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@BILLINGADDR", SqlDbType.VarChar, 9000);
                para[vCount++].Value = oEntity.BILLING_CUSTOMER_ADDRESS;
                para[vCount] = new SqlParameter("@BILLINGCITY", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.BILLING_CUSTOMER_CITY;
                para[vCount] = new SqlParameter("@BILLINGCOUNTRY", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.BILLING_CUSTOMER_COUNTRY;
                para[vCount] = new SqlParameter("@BILLINGPHONE", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.BILLING_CUSTOMER_PHONE;
                para[vCount] = new SqlParameter("@BILLINGPIN", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.BILLING_CUSTOMER_PINCODE;



                para[vCount] = new SqlParameter("@SHIPPINGADDR", SqlDbType.VarChar, 9000);
                para[vCount++].Value = oEntity.CUSTOMER_ADDRESS;
                para[vCount] = new SqlParameter("@SHIPPINGCITY", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_CITY;
                para[vCount] = new SqlParameter("@SHIPPINGCOUNTRY", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_COUNTRY;
                para[vCount] = new SqlParameter("@SHIPPINGPHONE", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_PHONE;
                para[vCount] = new SqlParameter("@SHIPPINGPIN", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_PINCODE;
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

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_CUSTOMER_ADDRESS", CommandType.StoredProcedure))
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

        #region Diecast Decals


        public byte SaveChanges(String pMode, EntityDtlsCustomer oEntity, ref String pMsg, String pAccYr, Int16? pCompany_key)
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
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@CUSTOMER_ADDRESS_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.CUSTOMER_ADRESS_KEY;


                para[vCount] = new SqlParameter("@CUSTOMER_FNAME", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_FNAME;
                para[vCount] = new SqlParameter("@CUSTOMER_LNAME", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_LNAME;
                para[vCount] = new SqlParameter("@CUSTOMER_ADDRESS", SqlDbType.VarChar, 9000);
                para[vCount++].Value = oEntity.CUSTOMER_ADDRESS;
                para[vCount] = new SqlParameter("@CUSTOMER_CITY", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_CITY;
                para[vCount] = new SqlParameter("@CUSTOMER_COUNTRY", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_COUNTRY;
                para[vCount] = new SqlParameter("@CUSTOMER_PHONE", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_PHONE;
                para[vCount] = new SqlParameter("@CUSTOMER_PINCODE", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_PINCODE;
                para[vCount] = new SqlParameter("@CUSTOMER_EMAIL", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_EMAIL;
                para[vCount] = new SqlParameter("@ADDRESS_CUSTOMER_EMAIL", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.ADDRESS_CUSTOMER_EMAIL;

                
                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_CUSTOMER_ADDRESS", CommandType.StoredProcedure))
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
