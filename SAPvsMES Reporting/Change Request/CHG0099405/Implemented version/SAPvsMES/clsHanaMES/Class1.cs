using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace clsHanaMES
{
    public class CT_Hana
    {
        string strConnectionString;

        public CT_Hana(string ConnectionString)
        {
            //Encryption4_5.Encryption obj = new Encryption4_5.Encryption();
            try
            {
                strConnectionString = ConnectionString;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            //finally
            //{
            //    obj = null;
            //}
        }
        
        public DataTable GetMESInfo(int customerID)
        {
           
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection cnn = new SqlConnection(strConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            string sSQL = "EXEC [up_GetHANAvsMES] " + customerID;
            try
            {
                cmd.CommandText = sSQL;
                da.SelectCommand = cmd;
                cnn.Open();
                da.Fill(dt);
                cnn.Close();
            }
            catch (Exception ex)
            {
                cnn.Close();
                Debug.WriteLine(ex.Message.ToString());
                dt = null;
            }
            finally
            {
                cnn = null;
                cmd = null;
                sSQL = null;
                da = null;
            }
            return dt;
        }

        public DataTable GetMESStock(string strDateFrom, string strDateTo)
        {
            DataTable dt = new DataTable();
            
            wsReportMovementsHana.wsReportMovements objHana = new wsReportMovementsHana.wsReportMovements();

            //string queryString = "SELECT  'MATNR', 'WERKS', 'LGORT', 'LABST', 'KLABS', 'TOTALUNRESTRICTEDSTOCK', 'MATKL' FROM '_SYS_BIC'.'production.imwm/CA_SQL_STOCK_OVERVIEW_BY_PLANTSLOC' ('PLACEHOLDER' = ('$$MATERIAL$$',''),'PLACEHOLDER' = ('$$PLANT$$','US03'),'PLACEHOLDER' = ('$$SLOC$$',''),'PLACEHOLDER' = ('$$MATGRP$$',''))";

                 try
            {
                dt = objHana.Hana_GetMovements("TEST_QUERY");
                //cmd.CommandText = queryString;
                //da.SelectCommand = cmd;
                //cnn.Open();
                //da.Fill(ds);
                //cnn.Close();
            }
            catch (Exception ex)
            {
                //cnn.Close();
                Debug.WriteLine(ex.Message.ToString());
                dt = null;
            }
            finally
            {
                //cnn = null;
                //cmd = null;
                //queryString = null;
                //da = null;
            }
            return dt;
        }
    }
}
