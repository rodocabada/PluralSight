using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CT_LinkPath
    {
      
        public DataSet GetLinkPath(string strSQLServer, string strDataBase, string FKCustomer, int Available)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
           
            strSQL = "SELECT L.[PKLinkPath] " +
                    ",L.[FKCustomer] " +
                    ",L.[FKLevel] " +
                    ",S.[Shift] " +
                    ",L.[Name] " +
                    ",L.[Phone] " +
                    ",[User] = U.[FirstName] + ' ' + U.[LastName] " +
                    ",L.[LastUpdated] " +
                    ",L.[Available] " +
                    "FROM [ValeoApps].[dbo].CT_LinkPath L (NOLOCK) " +
                    "INNER JOIN [ValeoApps].[dbo].CT_Shifts S (NOLOCK) ON L.FKShift = S.PKShift " +
                    "INNER JOIN [ValeoApps].[dbo].SC_Users U (NOLOCK) ON L.FKUserUpdater = U.PKUser " +
                    "INNER JOIN [ValeoApps].[dbo].CT_Customers C (NOLOCK) ON L.FKCustomer = c.PKCustomer " +
                    "INNER JOIN [ValeoApps].[dbo].CT_Level N (NOLOCK) ON L.FKLevel = N.PKLevel " +
                   "WHERE L.FKCustomer = @FKCustomer ";

            if (Available == 1)
            {
                strSQL += "AND L.[Available] = @Available ";

            }

            strSQL += "ORDER BY FKLevel ASC";
                      
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FKCustomer", DataAccessNet.Command.ParameterType.Int, FKCustomer);
            if (Available == 1)
            {
                objSQLCommand.AddParameter("@Available", DataAccessNet.Command.ParameterType.Bit, true);

            }
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

        public void UpdateLinkPath(string strSQLServer, string strSQLDataBase, int PKLinkPath,
                                   int FKLevel, int FKShift, string Name, string Phone, int FKUserUpdater, bool Available)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;


            strSQL = "UPDATE [dbo].CT_LinkPath  " +
                     "SET FKLevel = @FKLevel " +
                     ",FKShift = @FKShift " +
                     ",Name = @Name " +
                     ",Phone = @Phone " +
                     ",FKUserUpdater = @FKUserUpdater " +
                     ",Available = @Available " +
                     "WHERE PKLinkPath = @PKLinkPath ";


            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FKLevel", DataAccessNet.Command.ParameterType.Int, FKLevel);
            objSQLCommand.AddParameter("@FKShift", DataAccessNet.Command.ParameterType.Int, FKShift);
            objSQLCommand.AddParameter("@Name", DataAccessNet.Command.ParameterType.NChar, Name);
            objSQLCommand.AddParameter("@Phone", DataAccessNet.Command.ParameterType.NChar, Phone);
            objSQLCommand.AddParameter("@FKUserUpdater", DataAccessNet.Command.ParameterType.Int, FKUserUpdater);
            objSQLCommand.AddParameter("@Available", DataAccessNet.Command.ParameterType.Bit, Available);
            objSQLCommand.AddParameter("@PKLinkPath", DataAccessNet.Command.ParameterType.Int, PKLinkPath);
            try
            {
                objCom.SelectResults(strSQLServer, strSQLDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
         


        }
       
        public void InsertLinkPath(string strSQLServer, string strSQLDataBase, int FKCustomer,
                                int FKLevel, int FKShift, string Name, string Phone, int FKUserUpdater, bool Available)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;

            strSQL = "INSERT INTO [dbo].CT_LinkPath  " +
                     "( " +
                     "FKCustomer " +
                     ",FKLevel " +
                     ",FKShift " +
                     ",Name " +
                     ",Phone " +
                     ",FKUserUpdater ) " +
                     "VALUES " + 
                     "( " +
                     "@FKCustomer " +
                     ",@FKLevel " +
                     ",@FKShift " +
                     ",@Name " +
                     ",@Phone " +
                     ",@FKUserUpdater )" ;

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FKCustomer", DataAccessNet.Command.ParameterType.Int, FKCustomer);
            objSQLCommand.AddParameter("@FKLevel", DataAccessNet.Command.ParameterType.Int, FKLevel);
            objSQLCommand.AddParameter("@FKShift", DataAccessNet.Command.ParameterType.Int, FKShift);
            objSQLCommand.AddParameter("@Name", DataAccessNet.Command.ParameterType.NChar, Name);
            objSQLCommand.AddParameter("@Phone", DataAccessNet.Command.ParameterType.NChar, Phone);
            objSQLCommand.AddParameter("@FKUserUpdater", DataAccessNet.Command.ParameterType.Int, FKUserUpdater);
            try
            {
                objCom.SelectResults(strSQLServer, strSQLDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }



        }
    }
}
