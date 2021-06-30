
using MyApp.Entity;
using MyApp.db;
using System;
using System.Data;
using System.Data.SqlClient;


namespace MyApp.db.MyAppBal
{
    public class BAProduct : IDisposable
    {
        Int16 vCount = 0;
        DataTable dt = null;
        DataSet ds = null;
        SqlParameter[] para = null;

       
        public DataTable BindDdl(Int32 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            try
            {
                vCount = 0;
                para = new SqlParameter[1];
                para[vCount] = new SqlParameter("@GET_REC_TYPE", SqlDbType.VarChar, 20);
                para[vCount++].Value = "DDL";


                using (sqlhelper oDBC = new sqlhelper("SP_GET_HEAD_PRODUCT_DETAILS", CommandType.StoredProcedure))
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

                using (sqlhelper oDBC = new sqlhelper("SP_GET_HEAD_PRODUCT_DETAILS", CommandType.StoredProcedure))
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

        public DataTable GetData(String pMode,  String pSEARCH_TEXT, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            try
            {
                vCount = 0;
                para = new SqlParameter[3];
                para[vCount] = new SqlParameter("@GET_REC_TYPE", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@SEARCH_KEY", SqlDbType.Int);
                para[vCount++].Value = pSEARCH_TEXT;
                para[vCount] = new SqlParameter("@SEARCH_TEXT", SqlDbType.VarChar, 50);
                para[vCount++].Value = pSEARCH_TEXT;

                using (sqlhelper oDBC = new sqlhelper("SP_GET_DTLS_PRODUCT_DETAILS", CommandType.StoredProcedure))
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

        public byte SaveChanges<T1, T2>(String pMode, EntityProduct oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[11];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;
               

                para[vCount] = new SqlParameter("@MAST_CATEGORY_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.MAST_CATEGORY_KEY;

                para[vCount] = new SqlParameter("@PRODUCT_NAME", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_NAME;

                para[vCount] = new SqlParameter("@PRICE", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.PRICE;


                para[vCount] = new SqlParameter("@PRODUCT_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_IMAGE;


                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_HEAD_PRODUCT_DETAILS", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@HEAD_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }


        public byte SaveProductChanges<T1, T2>(String pMode, EntityProduct oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[11];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;


                para[vCount] = new SqlParameter("@DTLS_PRODUCT_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.DTLS_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@SHEET_CODE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE;

                para[vCount] = new SqlParameter("@FILE_UPLOAD_1", SqlDbType.VarChar, 500);
                para[vCount++].Value = oEntity.PRODUCT_IMAGE;
                para[vCount] = new SqlParameter("@FILE_UPLOAD_2", SqlDbType.VarChar, 500);
                para[vCount++].Value = oEntity.PRODUCT_IMAGE_2;


                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_PRODUCT_DETAILS", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@HEAD_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }


        public DataSet GetData<T>(String pMode, T pKey, String pSEARCH_TEXT, ref String pMsg, String pAccYr, Int16? pCompany_key)
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

                using (sqlhelper oDBC = new sqlhelper("SP_GET_ABOUT_US", CommandType.StoredProcedure))
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

        public Byte SaveDelete<T1, T2>(String pMode, EntityProduct oEntity, T1 pValue, ref T2 pKey, ref String pMsg, ref String pDelMsg, String pAccYr, Int16? pCompany_key)
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
                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.SmallInt);
                para[vCount++].Value = oEntity.ENT_USER_KEY;

                using (sqlhelper oDBC = new sqlhelper("SP_DELETE_HEAD_PRODUCT_DETAILS", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@HEAD_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }



        public Byte SaveDelete<T2>(String pMode, EntityProduct oEntity, ref T2 pKey, ref String pMsg, ref String pDelMsg, String pAccYr, Int16? pCompany_key)
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
                para[vCount] = new SqlParameter("@DTLS_PRODUCT_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.DTLS_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.SmallInt);
                para[vCount++].Value = oEntity.ENT_USER_KEY;

                using (sqlhelper oDBC = new sqlhelper("SP_DELETE_DTLS_PRODUCT_DETAILS", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@DTLS_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }



        public byte ProductSaveChanges<T1, T2>(String pMode, EntityProduct oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[27];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;


                para[vCount] = new SqlParameter("@MAST_CATEGORY_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.MAST_CATEGORY_KEY;
                para[vCount] = new SqlParameter("@SUB_CATEGORY_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.SUB_CATEGORY_KEY;
                para[vCount] = new SqlParameter("@SUB_SUB_CATEGORY_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.SUB_SUB_CATEGORY_KEY;

                para[vCount] = new SqlParameter("@PRODUCT_NAME", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_NAME;

                para[vCount] = new SqlParameter("@PRICE", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.PRICE;


                para[vCount] = new SqlParameter("@PRODUCT_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.MAIN_IMAGE;
                para[vCount] = new SqlParameter("@PRODUCT_WATER_MARK_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_IMAGE;
                para[vCount] = new SqlParameter("@PRODUCT_FILE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_IMAGE_2;

                //////
                para[vCount] = new SqlParameter("@SHEET_CODE_1", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_1;
                para[vCount] = new SqlParameter("@SHEET_CODE_1_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_1_IMG;
                para[vCount] = new SqlParameter("@SHEET_CODE_1_FILE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_1_PDF;


                para[vCount] = new SqlParameter("@SHEET_CODE_2", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_2;
                para[vCount] = new SqlParameter("@SHEET_CODE_2_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_2_IMG;
                para[vCount] = new SqlParameter("@SHEET_CODE_2_FILE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_2_PDF;

                para[vCount] = new SqlParameter("@SHEET_CODE_3", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_3;
                para[vCount] = new SqlParameter("@SHEET_CODE_3_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_3_IMG;
                para[vCount] = new SqlParameter("@SHEET_CODE_3_FILE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_3_PDF;

                para[vCount] = new SqlParameter("@SHEET_CODE_4", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_4;
                para[vCount] = new SqlParameter("@SHEET_CODE_4_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_4_IMG;
                para[vCount] = new SqlParameter("@SHEET_CODE_4_FILE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.SHEET_CODE_4_PDF;
                ///////

                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_HEAD_PRODUCT_DETAILS", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@HEAD_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }




        /// <Start_diecaastzonecc>
        public DataTable ProductBindDdl(Int32 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            try
            {
                vCount = 0;
                para = new SqlParameter[1];
                para[vCount] = new SqlParameter("@GET_REC_TYPE", SqlDbType.VarChar, 20);
                para[vCount++].Value = "DDL";


                using (sqlhelper oDBC = new sqlhelper("SP_GET_HEAD_PRODUCT", CommandType.StoredProcedure))
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


        public DataTable Get(String pMode, String pSEARCH_TEXT, ref String pMsg, String pAccYr, Int16? pCompany_key)
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

                using (sqlhelper oDBC = new sqlhelper("SP_GET_HEAD_PRODUCT", CommandType.StoredProcedure))
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

        public DataTable GetDatadtls(String pMode, String pSEARCH_TEXT, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            try
            {
                vCount = 0;
                para = new SqlParameter[3];
                para[vCount] = new SqlParameter("@GET_REC_TYPE", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@SEARCH_KEY", SqlDbType.Int);
                para[vCount++].Value = pSEARCH_TEXT;
                para[vCount] = new SqlParameter("@SEARCH_TEXT", SqlDbType.VarChar, 50);
                para[vCount++].Value = pSEARCH_TEXT;

                using (sqlhelper oDBC = new sqlhelper("SP_GET_DTLS_PRODUCT", CommandType.StoredProcedure))
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

        public byte SaveHeadProduct<T1, T2>(String pMode, EntityProduct oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[12];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@PRODUCT_NAME", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_NAME;

                para[vCount] = new SqlParameter("@PRODUCT_DESC", SqlDbType.VarChar, 500);
                para[vCount++].Value = oEntity.PRODUCT_DESC;


                para[vCount] = new SqlParameter("@PRODUCT_IMAGE", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.MAIN_IMAGE;

                para[vCount] = new SqlParameter("@PRODUCT_IMAGE_1", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_IMAGE;

                para[vCount] = new SqlParameter("@PRODUCT_IMAGE_2", SqlDbType.VarChar, 200);
                para[vCount++].Value = oEntity.PRODUCT_IMAGE_2;


                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_HEAD_PRODUCT", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@HEAD_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }

        public byte SaveDTlsProduct<T1, T2>(String pMode, EntityProduct oEntity, T1 pValue, ref T2 pKey, ref String pMsg, String pAccYr, Int16? pCompany_key)
        {
            Byte vRef = 0;
            try
            {
                vCount = 0;
                para = new SqlParameter[10];
                para[vCount] = new SqlParameter("@SELECT_ACTION", SqlDbType.VarChar, 20);
                para[vCount++].Value = pMode;
                para[vCount] = new SqlParameter("@RETURN_KEY", SqlDbType.TinyInt);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = 0;

                para[vCount] = new SqlParameter("@HEAD_PRODUCT_KEY", SqlDbType.Int);
               
                para[vCount++].Value = oEntity.HEAD_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@DTLS_PRODUCT_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.DTLS_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@QTY", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.QTY;


                para[vCount] = new SqlParameter("@PRICE", SqlDbType.Decimal);
                para[vCount++].Value = oEntity.PRICE;

                para[vCount] = new SqlParameter("@ENT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.ENT_USER_KEY;
                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.Int);
                para[vCount++].Value = oEntity.EDIT_USER_KEY;
                para[vCount] = new SqlParameter("@TAG_ACTIVE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_ACTIVE;
                para[vCount] = new SqlParameter("@TAG_DELETE", SqlDbType.TinyInt);
                para[vCount++].Value = oEntity.TAG_DELETE;

                using (sqlhelper oDBC = new sqlhelper("SP_SAVE_DTLS_PRODUCT", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@HEAD_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }
        public Byte SaveDeleteDTLS<T2>(String pMode, EntityProduct oEntity, ref T2 pKey, ref String pMsg, ref String pDelMsg, String pAccYr, Int16? pCompany_key)
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
                para[vCount] = new SqlParameter("@DTLS_PRODUCT_KEY", SqlDbType.Int);
                para[vCount].Direction = ParameterDirection.InputOutput;
                para[vCount++].Value = oEntity.DTLS_PRODUCT_KEY;

                para[vCount] = new SqlParameter("@EDIT_USER_KEY", SqlDbType.SmallInt);
                para[vCount++].Value = oEntity.ENT_USER_KEY;

                using (sqlhelper oDBC = new sqlhelper("SP_DELETE_DTLS_PRODUCT", CommandType.StoredProcedure))
                {
                    oDBC.ExecuteNonQuery(pAccYr, pCompany_key, para, ref pMsg);
                    vRef = Convert.ToByte(oDBC.GetParameterValue("@RETURN_KEY", ref pMsg));
                    pKey = (T2)Convert.ChangeType(oDBC.GetParameterValue("@DTLS_PRODUCT_KEY", ref pMsg), typeof(T2));
                }
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return vRef;
        }
        /// </end>

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
