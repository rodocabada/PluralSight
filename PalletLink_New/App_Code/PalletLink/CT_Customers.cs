using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CT_Customers
    {
        public DataSet GetCustomers(string strSQLServer, string strDataBase)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [PKCustomer]" +
                          ",[Customer]" +
                          ",[LastUpdated]" +
                          ",[Available]" +
                      " FROM [ValeoApps].[dbo].[CT_Customers]";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet GetMESRoutesByClient(string strSQLServer, string strDataBase, string Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetMARoutesByCustomerAndRouteStep @Customer";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.NChar, Customer);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }
        public DataSet GetMESRoutesByClientUpd(string strSQLServer, string strDataBase, string Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetMARoutesByCustomerAndRouteStepUpd @Customer";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.NChar, Customer);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet GetMESRouteSteps(string strSQLServer, string strDataBase, string Customer, int MARouteID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetRouteStepsUpd @Customer, @MARouteID";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.NChar, Customer);
            objSQLCommand.AddParameter("@MARouteID", DataAccessNet.Command.ParameterType.Int, MARouteID);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }
       
        public DataSet GetRouteStepByEquipment(string strSQLServer, string strDataBase, int EquipmentID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetRouteStepByEquipment @EquipmentID";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@EquipmentID", DataAccessNet.Command.ParameterType.Int, EquipmentID);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet GetEquipmentsByRouteStep(string strSQLServer, string strDataBase, int RouteStepID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetEquipmentsByRouteStep @RouteStepID";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@RouteStepID", DataAccessNet.Command.ParameterType.Int, RouteStepID);
            try
            {
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }
     
        // Server and DB Test Group

       public DataSet getCustomersInfo(string strSQLServer, string strDataBase, string customer)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[up_GetCustomersByName] " +
                            "@Combo = 1 " +
                            ",@CustomerName = " + customer;

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

        public DataSet getCustomersByUserInfo(string strSQLServer, string strDataBase, int user, int module)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = "DECLARE	@return_value int " +
                            "EXEC	@return_value = [dbo].[up_GetCustomersByUser] " +
                            "@UserID = " + user + ", " +
                            "@Module = " + module;

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
