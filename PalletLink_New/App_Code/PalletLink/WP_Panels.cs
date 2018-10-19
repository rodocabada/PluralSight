using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class WP_Panels
    {
        public DataSet GetPanelSN(string strSQLServer, string strDataBase, string Customer, string Serial)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT P.[Panel_ID]"+
                          ",P.[Panel]"+
                          ",P.[Wip_ID]"+
                          ",P.[SerialNumber]"+
                          ",[Mapping]"+
                          ",[XOut]"+
                          ",[XOutStats]"+
	                      ",Customer"+
                      " FROM CHISQLV10A.[JEMS].[dbo].[WP_Panels_V] P"+
                      " INNER JOIN CHISQLV10A.[JEMS].[dbo].WP_Wip W WITH (NOLOCK) ON P.Wip_ID = W.Wip_ID"+
                      " INNER JOIN CHISQLV10A.[JEMS].[dbo].CR_Customer_V C WITH (NOLOCK) ON C.Customer_ID = W.Customer_ID"+
                      " WHERE Customer like'%'+@Customer+'%' AND P.Panel = (SELECT Panel FROM CHISQLV10A.[JEMS].[dbo].WP_Panels_V WHERE SerialNumber = @Serial)"+
                      " ORDER BY Mapping";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.NChar, Customer);
            objSQLCommand.AddParameter("@Serial", DataAccessNet.Command.ParameterType.NChar, Serial);
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

        public DataSet GetPanelSize(string strSQLServer, string strDataBase, string Customer, string Serial)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT P.[Panel_ID]" +
                          ",P.[Panel]" +
                          ",MAX([Mapping]) as Elements" +
                          ",Customer" +
                      " FROM CHISQLV10A.[JEMS].[dbo].[WP_Panels_V] P" +
                      " INNER JOIN CHISQLV10A.[JEMS].[dbo].WP_Wip W WITH (NOLOCK) ON P.Wip_ID = W.Wip_ID" +
                      " INNER JOIN CHISQLV10A.[JEMS].[dbo].CR_Customer_V C WITH (NOLOCK) ON C.Customer_ID = W.Customer_ID" +
                      " WHERE Customer like'%'+@Customer+'%' AND P.Panel = (SELECT Panel FROM CHISQLV10A.[JEMS].[dbo].WP_Panels_V WHERE SerialNumber = @Serial)" +
                      " GROUP BY P.Panel_ID, P.Panel, Customer";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.NChar, Customer);
            objSQLCommand.AddParameter("@Serial", DataAccessNet.Command.ParameterType.NChar, Serial);
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
