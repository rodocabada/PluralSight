using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class SY_PalletLinkFAQ
    {

        public DataSet GetFAQ(string strSQLServer, string strDataBase, int Available)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;

            strSQL = "SELECT F.[FAQ] " +
                     ",[User] = U.[FirstName] + ' ' + U.[LastName] " +
                     ",F.[LastUpdated] " +
                     ",F.[Available] " +
                     "FROM [dbo].SY_PalletLinkFAQ F (NOLOCK) " +
                     "INNER JOIN [ValeoApps].[dbo].SC_Users U (NOLOCK) ON F.FKUserUpdater = U.PKUser ";
                     
                  

            if (Available == 1)
            {
                strSQL += "WHERE F.[Available] = @Available ";

            }

            strSQL += "ORDER BY F.[LastUpdated] DESC ";

            objSQLCommand = new DataAccessNet.Command(strSQL);
           
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

        public void UpdateFAQ(string strSQLServer, string strSQLDataBase)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;


            strSQL = "UPDATE [dbo].SY_PalletLinkFAQ  " +
                     "SET Available = @Available ";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Available", DataAccessNet.Command.ParameterType.Bit, false);
 
            try
            {
                objCom.SelectResults(strSQLServer, strSQLDataBase, objSQLCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }


        }

        public void InsertFAQ(string strSQLServer, string strSQLDataBase,
                              string FAQ, int FKUserUpdater)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;

            strSQL = "INSERT INTO [dbo].SY_PalletLinkFAQ  " +
                     "( " +
                     "FAQ " +
                     ",FKUserUpdater " +
                      ") " +
                     "VALUES " +
                     "( " +
                     "@FAQ " +
                     ",@FKUserUpdater " +
                     " )";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FAQ", DataAccessNet.Command.ParameterType.NChar, FAQ);
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
