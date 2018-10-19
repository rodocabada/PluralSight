using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;




namespace CLPalletLink
{
    public class CT_PalletTimeOutByCustomer
    {

        public DataSet GetCustomerTimeOut(string strSQLServer, string strDataBase, string FKCustomer)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[up_GetPalletTimeOutByCustomer] " +
                            "@FKCustomer = '" + FKCustomer + "'";

            try
            {
                ds = SQL.dsSQLQuery(strSQLServer, strDataBase, strSQL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet UpdateCustomerTimeOut(string strSQLServer, string strDataBase, int gvPKTime, int gvTimeout, int gvPKUser)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[up_UpdatePalletTimeOutByCustomer] " +
                            "@PKTimeOut = " + gvPKTime + ", " +
                            "@TimeOut = '" + gvTimeout + "', " +
                            "@pFKLastUpdatedUserID = " + gvPKUser;

            try
            {
                ds = SQL.dsSQLQuery(strSQLServer, strDataBase, strSQL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet GetPalletTimeOut(string strSQLServer, string strDataBase, string FKCustomer, bool IsPalletValidation, string PalletName)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[up_GetPalletTimeOut] " +
                            "@FKCustomer = '" + FKCustomer + "'" +
                            ",@IsPalletValidation  = '" + IsPalletValidation + "'" +
                            ",@PalletName = '" + PalletName + "'";


            try
            {
                ds = SQL.dsSQLQuery(strSQLServer, strDataBase, strSQL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet InsertPalletTimeOut(string strSQLServer, string strDataBase, string PalletName, string Customer, string AvailableDatetime)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();
            int minutes = int.Parse(AvailableDatetime);
            int FKCustomer = int.Parse(Customer);

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[palletTimeOut] " +
                            "@PalletName = '" + PalletName + "', " +
                            "@FKCustomer = " + FKCustomer + ", " +
                            "@LastUpdated = '" + DateTime.Now + "', " +
                            "@AvailableDatetime = '" + DateTime.Now.AddSeconds(minutes) + "'";


            try
            {
                ds = SQL.dsSQLQuery(strSQLServer, strDataBase, strSQL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet UpdatePalletTimeOut(string strSQLServer, string strDataBase, string PalletName, string Customer, string AvailableDatetime)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();
            int minutes = int.Parse(AvailableDatetime);
            int FKCustomer = int.Parse(Customer);

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[up_UpdatePalletTimeOut] " +
                            "@PalletName = '" + PalletName + "', " +
                            "@FKCustomer = " + FKCustomer + ", " +
                            "@LastUpdated = '" + DateTime.Now + "', " +
                            "@AvailableDatetime = '" + DateTime.Now.AddSeconds(minutes) + "'";


            try
            {
                ds = SQL.dsSQLQuery(strSQLServer, strDataBase, strSQL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }





    }
}
