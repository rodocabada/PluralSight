using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class SY_PalletLink
    {
        public DataSet InsertPalletLink(string strSQLServer, string strDataBase, String SerialNumber, string PalletId, String Operation, string User, int Available)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_InsertPalletLink @SerialNumber, @PalletId, @Operation, @User, @Available";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@SerialNumber", DataAccessNet.Command.ParameterType.NChar, SerialNumber);
            objSQLCommand.AddParameter("@PalletId", DataAccessNet.Command.ParameterType.NChar, PalletId);
            objSQLCommand.AddParameter("@Operation", DataAccessNet.Command.ParameterType.NChar, Operation);
            objSQLCommand.AddParameter("@User", DataAccessNet.Command.ParameterType.NChar, User);
            objSQLCommand.AddParameter("@Available", DataAccessNet.Command.ParameterType.Int, Available);

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

        public DataSet sendLog(string strSQLServer, string strDataBase,string Customer_Id, string Customer, string SerialNumber,string PalletId,
                                 string Assembly_Id,string Assembly,string Wip_Id,string PanelNumberPL,string PanelSizePL,
                                 string PanelSizeMES, string LinkObject, string LinkMaterialID, string EquipmentValue, 
                                 string EquipmentName, string RouteStepID, string SerialLoops,string LoopsAllowed,string Message)
      {
          DataSet ds = new DataSet();
          DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
          DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
          string strSQL = null;
          strSQL = "EXEC up_AddLinkingLog @Customer_Id, @Customer, @SerialNumber, @PalletId, @Assembly_Id,"+
                                          "@Assembly, @Wip_Id,@PanelNumberPL,@PanelSizePL,@PanelSizeMES,@LinkObject,"+
                                          "@LinkMaterialID,@EquipmentValue,@EquipmentName,@RouteStepID,"+
                                          "@SerialLoops,@LoopsAllowed,@Message";
          objSQLCommand = new DataAccessNet.Command(strSQL);
          objSQLCommand.AddParameter("@Customer_Id", DataAccessNet.Command.ParameterType.VarChar, Customer_Id);
          objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.VarChar, Customer);
          objSQLCommand.AddParameter("@SerialNumber", DataAccessNet.Command.ParameterType.VarChar, SerialNumber);
          objSQLCommand.AddParameter("@PalletId", DataAccessNet.Command.ParameterType.VarChar, PalletId);
          objSQLCommand.AddParameter("@Assembly_Id", DataAccessNet.Command.ParameterType.VarChar, Assembly_Id);
          objSQLCommand.AddParameter("@Assembly", DataAccessNet.Command.ParameterType.VarChar, Assembly);
          objSQLCommand.AddParameter("@Wip_Id", DataAccessNet.Command.ParameterType.VarChar, Wip_Id);
          objSQLCommand.AddParameter("@PanelNumberPL", DataAccessNet.Command.ParameterType.VarChar, PanelNumberPL);
          objSQLCommand.AddParameter("@PanelSizePL", DataAccessNet.Command.ParameterType.VarChar, PanelSizePL);
          objSQLCommand.AddParameter("@PanelSizeMES", DataAccessNet.Command.ParameterType.VarChar, PanelSizeMES);
          objSQLCommand.AddParameter("@LinkObject", DataAccessNet.Command.ParameterType.VarChar, LinkObject);
          objSQLCommand.AddParameter("@LinkMaterialID", DataAccessNet.Command.ParameterType.VarChar, LinkMaterialID);
          objSQLCommand.AddParameter("@EquipmentValue", DataAccessNet.Command.ParameterType.VarChar, EquipmentValue);
          objSQLCommand.AddParameter("@EquipmentName", DataAccessNet.Command.ParameterType.VarChar, EquipmentName);
          objSQLCommand.AddParameter("@RouteStepID", DataAccessNet.Command.ParameterType.VarChar, RouteStepID);
          objSQLCommand.AddParameter("@SerialLoops", DataAccessNet.Command.ParameterType.VarChar, SerialLoops);
          objSQLCommand.AddParameter("@LoopsAllowed", DataAccessNet.Command.ParameterType.VarChar, LoopsAllowed);
          objSQLCommand.AddParameter("@Message", DataAccessNet.Command.ParameterType.VarChar, Message);
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


        public DataSet fnInsertPalletLink(string strSQLServer, string strDataBase, String SerialNumber, string PalletId, String Operation, string User, int Available)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = "EXEC up_InsertPalletLink " +
                            "@SerialNumber = '" + SerialNumber + "', " +
                            "@PalletId = '" + PalletId + "', " +
                            "@Operation = '" + Operation + "', " +
                            "@User = '" + User + "', " +
                            "@Available = '" + Available + "' ";


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
