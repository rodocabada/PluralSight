using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CT_Applications
    {
        public DataSet GetAdminEmail(string strSQLServer, string strDataBase, int PKApplication, int PKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [PKApplication]" +
                          ",[Application]" +
                          ",[Admin]" +
                          ",[FirstName] + ' ' + [LastName] as AdminUser" +
                          ",[Email]" +
                          ",[Description]" +
                          ",[MainPage]" +
                          ",[FKUserUpdater]" +
                      " FROM [ValeoApps].[dbo].[CT_Applications] A" +
                      " INNER JOIN SC_AdminCustomerApplication ACA ON A.PKApplication = ACA.FKApplication" +
                      " INNER JOIN SC_Users U ON ACA.Admin = U.PKUser" +
                      " WHERE ACA.FKCustomer = @PKCustomer AND PKApplication = @PKApplication";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKApplication", DataAccessNet.Command.ParameterType.Int, PKApplication);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, PKCustomer);
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

        public DataSet ApproveAccess(string strSQLServer, string strDataBase, int PKUser, int PKApplication, int Approved, int Updater)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_ApproveAccess @PKUser, @PKApplication, @Approved, @Updater";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKUser", DataAccessNet.Command.ParameterType.Int, PKUser);
            objSQLCommand.AddParameter("@PKApplication", DataAccessNet.Command.ParameterType.Int, PKApplication);
            objSQLCommand.AddParameter("@Approved", DataAccessNet.Command.ParameterType.Int, Approved);
            objSQLCommand.AddParameter("@Updater", DataAccessNet.Command.ParameterType.Int, Updater);
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
    }
}
