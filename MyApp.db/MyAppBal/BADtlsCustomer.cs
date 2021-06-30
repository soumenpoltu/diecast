using MyApp.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.db.MyAppBal
{
    public class BADtlsCustomer : IDisposable
    {
        Int16 vCount = 0;
        DataTable dt = null;
        SqlParameter[] para = null;

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
            

                using (sqlhelper oDBC = new sqlhelper("SP_GET_DTLS_CUSTOMER", CommandType.StoredProcedure))
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

        public DataTable Get<T>(String pMode, T pKey, String pSEARCH_TEXT, Int16 pCompanyKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
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
             
                using (sqlhelper oDBC = new sqlhelper("SP_GET_DTLS_CUSTOMER", CommandType.StoredProcedure))
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

        public Byte SaveChanges<T1, T2>(String pMode, EntityDtlsCustomer oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[20];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@CUSTOMER_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.CUSTOMER_KEY;

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

                para[vCount] = new SqlParameter("@CUSTOMER_PASSWORD", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_PASSWORD;
                para[vCount] = new SqlParameter("@CUSTOMER_CPASSWORD", SqlDbType.VarChar, 50);
                para[vCount++].Value = oEntity.CUSTOMER_CPASSWORD;

                para[vCount] = new SqlParameter("@USER_TYPE_KEY", SqlDbType.Int);
                para[vCount++].Value = 2;
                para[vCount] = new SqlParameter("@ACTIVATION_CODE", SqlDbType.VarChar, 100);
                para[vCount++].Value = oEntity.ACTIVATION_CODE;
                para[vCount] = new SqlParameter("@TAG_ACTIVATION", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVATION;


                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;
               

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_CUSTOMER", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@USER_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }

        public byte Delete<T>(String pMode, T pValue, ref T pKey, ref String pMsg, Int32 pUser, String pAccYr, Int16? pCompany_key)
        {
            byte retKey = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[4];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@USER_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = pValue;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.SmallInt);
                para[vCount++].Value = pUser;

                using (sqlhelper oDBC = new sqlhelper("SP_DELETE_DTLS_CUSTOMER", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    retKey = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return retKey;
        }

        #region DiecastZoneCC
        public Byte SaveRegister<T1, T2>(String pMode, EntityDtlsCustomer oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[14];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;
                para[vCount] = new SqlParameter("@CUSTOMER_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.CUSTOMER_KEY;

                para[vCount] = new SqlParameter("@CUSTOMER_NAME", SqlDbType.VarChar, 500);
                para[vCount++].Value = oEntity.CUSTOMER_NAME;
                para[vCount] = new SqlParameter("@CUSTOMER_EMAIL", SqlDbType.VarChar, 500);
                para[vCount++].Value = oEntity.CUSTOMER_EMAIL;

                para[vCount] = new SqlParameter("@CUSTOMER_PASSWORD", SqlDbType.VarChar, 500);
                para[vCount++].Value = oEntity.CUSTOMER_PASSWORD;
                para[vCount] = new SqlParameter("@CUSTOMER_CPASSWORD", SqlDbType.VarChar, 500);
                para[vCount++].Value = oEntity.CUSTOMER_CPASSWORD;

                para[vCount] = new SqlParameter("@USER_TYPE_KEY", SqlDbType.Int);
                para[vCount++].Value = 2;
                para[vCount] = new SqlParameter("@ACTIVATION_CODE", SqlDbType.VarChar, 100);
                para[vCount++].Value = oEntity.ACTIVATION_CODE;
                para[vCount] = new SqlParameter("@TAG_ACTIVATION", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVATION;


                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;


                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_CUSTOMER", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@CUSTOMER_KEY", ref pMsg), typeof(T2));
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
