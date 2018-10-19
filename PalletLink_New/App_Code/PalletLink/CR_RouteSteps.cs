using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CR_RouteSteps
    {
        public DataSet GetMESRouteStepPalletPos(string strSQLServer, string strDataBase, string Customer, int MARouteID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetRouteStepPalletPos @Customer, @MARouteID";
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
    }
}
