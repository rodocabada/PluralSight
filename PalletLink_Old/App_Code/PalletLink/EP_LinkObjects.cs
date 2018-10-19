using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class EP_LinkObjects
    {
        public DataSet GetLinkObject(string strSQLServer, string strDataBase, string Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [LinkObject_ID]"+
	                      ",CR_Customer_V.Customer"+
                          ",L.[Customer_ID]"+
                          ",[LinkObject]"+
                          ",[ChildBoardAssembly]"+
                          ",[Number]"+
                          ",[LinkObjectGroup_ID]"+
                          ",[Descr]"+
                          ",[UniqueFlag]"+
                          ",[UseInTranslation]"+
                          ",L.[UserID_ID]"+
                          ",L.[LastUpdated]"+
                      " FROM CHISQLV10A.[JEMS].[dbo].[EP_LinkObjects] L" +
                      " INNER JOIN CHISQLV10A.[JEMS].[dbo].[CR_Customers] C WITH (NOLOCK) ON C.Customer_ID = L.Customer_ID" +
                      " INNER JOIN CHISQLV10A.[JEMS].[dbo].CR_Customer_V ON CR_Customer_V.Customer_ID = L.Customer_ID" +
                      " WHERE LinkObject like '%PALLETID%' AND CR_Customer_V.Customer = @Customer";
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
    }
}
