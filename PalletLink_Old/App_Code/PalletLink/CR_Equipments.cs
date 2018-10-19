using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CR_Equipments
    {
        public DataSet GetEquipmentID(string strSQLServer, string strDataBase, string Equipment)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT TOP 1000 [Equipment_ID]"+
                          ",[EquipmentMaster_ID]"+
                          ",[CommonName]"+
                          ",[SeqNumber]"+
                          ",[AssetTag]"+
                          ",[ManSerialNumber]"+
                          ",[Available]"+
                          ",[EquipmentSetup_ID]"+
                          ",[TableA]"+
                          ",[TableB]"+
                          ",[PPM]"+
                          ",[DateOfManufacture]"+
                          ",[RunTimeHours]"+
                          ",[SoftwareVersion]"+
                          ",[Condition_ID]"+
                          ",[Status_ID]"+
                          ",[FeederTracking]"+
                          ",[FeederTypeValidation]"+
                          ",[UserID_ID]"+
                          ",[LastUpdated]"+
                          ",[Build_ID]"+
                          ",[IsQtyRequiredOnAllocation]"+
                      " FROM CHISQLV10A.[JEMS].[dbo].[CR_Equipment]" +
                      " WHERE CommonName = @Equipment";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Equipment", DataAccessNet.Command.ParameterType.NChar, Equipment);
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
