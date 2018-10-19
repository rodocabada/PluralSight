using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CT_Shifts
    {
      
        public DataSet GetShifts(string strSQLServer, string strDataBase)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT S.PKShift, S.Shift " +
                     "FROM [ValeoApps].[dbo].CT_Shifts S (NOLOCK) ";
                      
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
