using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CT_Level
    {
      
        public DataSet GetLevels(string strSQLServer, string strDataBase)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT N.PKLevel, N.Level " +
                     "FROM [ValeoApps].[dbo].CT_Level N (NOLOCK) ";
                      
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

     

    }
}
