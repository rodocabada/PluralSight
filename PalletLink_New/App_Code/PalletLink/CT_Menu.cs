using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CT_Menu
    {
        public DataSet GetScreens(string strSQLServer, string strDataBase, int FKRoleID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetScreens @FKRoleID";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@FKRoleID", DataAccessNet.Command.ParameterType.Int, FKRoleID);
           

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
        public DataSet GetScreensDetails(string strSQLServer, string strDataBase, int PKScreenID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetScreenDetails @PKScreenID";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKScreenID", DataAccessNet.Command.ParameterType.Int, PKScreenID);
           

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
        
        public DataSet InsertAssemblyUpd(string strSQLServer, string strDataBase, int Updater, string Assy, int Rows,
                                        int Columns, int FKCustomer, string PalletID, Boolean Panel, Boolean LoopValidation,
                                        int LoopsAllowed, Boolean CheckPointValidation)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_AddAssemblyUpdDos @Updater, @Assy, @Rows, @Columns, @FKCustomer, @PalletID, @Panel,@LoopValidation,@LoopsAllowed,@CheckPointValidation";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Updater", DataAccessNet.Command.ParameterType.Int, Updater);
            objSQLCommand.AddParameter("@Assy", DataAccessNet.Command.ParameterType.NChar, Assy);
            objSQLCommand.AddParameter("@Rows", DataAccessNet.Command.ParameterType.Int, Rows);
            objSQLCommand.AddParameter("@Columns", DataAccessNet.Command.ParameterType.Int, Columns);
            objSQLCommand.AddParameter("@FKCustomer", DataAccessNet.Command.ParameterType.Int, FKCustomer);
            objSQLCommand.AddParameter("@PalletID", DataAccessNet.Command.ParameterType.NChar, PalletID);
            objSQLCommand.AddParameter("@Panel", DataAccessNet.Command.ParameterType.Bit, Panel);
            objSQLCommand.AddParameter("@LoopValidation", DataAccessNet.Command.ParameterType.Bit, LoopValidation);
            objSQLCommand.AddParameter("@LoopsAllowed", DataAccessNet.Command.ParameterType.Int, LoopsAllowed);
            objSQLCommand.AddParameter("@CheckPointValidation", DataAccessNet.Command.ParameterType.Bit, CheckPointValidation);

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

        public DataSet checkIfAssyExists(string strSQLServer, string strDataBase, string Assy, string Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [Assembly_ID]"+
                          ",[Assembly]"+
                          ",[Number]"+
                          ",[Revision]"+
                          ",[Version]"+
                          ",[Descr]"+
                          ",[Phantom]"+
                          ",[BOM_ID]"+
                          ",[EffectiveFrom]"+
                          ",[EffectiveTo]"+
                          ",[Active]"+
                          ",[Customer]"+
                          ",[Panel_ID]"+
                          ",[Family_ID]"+
                          ",[BarcodeMask_ID]"+
                          ",[UseMultiPartBarcode]"+
                          ",[UserID_ID]"+
                          ",[LastUpdated]"+
                          ",[BuildStatus_ID]"+
                          ",[CustomerNumber]"+
                          ",[CustomerRevision]"+
                          ",[AssemblyFilter1]"+
                          ",[AssemblyFilter2]"+
                      " FROM CHISQLV10A.[JEMS].[dbo].[CR_Assemblies]" +
                      " INNER JOIN CHISQLV10A.[JEMS].[dbo].CR_Customer_V ON CR_Customer_V.Customer_ID = CR_Assemblies.Customer_ID" +
                      " WHERE Number = @Assy AND Customer = @Customer AND Active = 1  AND Number NOT LIKE '% %'";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Assy", DataAccessNet.Command.ParameterType.NChar, Assy);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.NChar, Customer);

            try{
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }catch (Exception e){
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet ShowAssemblies(string strSQLServer, string strDataBase, int Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT TOP 1000 [PKAssembly]"+
                          ",[Assembly]"+
                          ",[Rows]"+
                          ",[Columns]"+
	                      ",[Rows] * [Columns] as Pieces"+
                          ",[FKCustomer]"+
                          ",U.FirstName + ' ' + U.LastName as UserUpdater"+
                          ",A.[LastUpdated]"+
                          ",PalletID"+
                      " FROM [ValeoApps].[dbo].[CT_Assemblies] A"+
                      " INNER JOIN SC_Users U ON U.PKUser = A.FKUserUpdater"+
                      " WHERE FKCustomer = @Customer";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.Int, Customer);

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

        public DataSet ShowAssembliesUpd(string strSQLServer, string strDataBase, int Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetAssembliesCustomer @PKCustomer";

            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKCustomer", DataAccessNet.Command.ParameterType.Int, Customer);

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

        public DataSet GetAssemblies(string strSQLServer, string strDataBase, string Customer)
        {


            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetAssembliesMES @Customer";
          
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.VarChar, Customer);

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

        public DataSet GetAssemblyBySN(string strSQLServer, string strDataBase, string Customer, string Serial)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetAssemblyBySN @Customer, @Serial";
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

        public DataSet GetAssemblyBySNTHLevel(string strSQLServer, string strDataBase, string Customer, string Serial)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetAssemblyBySNTHLevel @Customer, @Serial";
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

        public DataSet GetAssemblyBySNTHLevelJohnson(string strSQLServer, string strDataBase, string Customer, string Serial)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetAssemblyBySNTHLevelJE @Customer, @Serial";
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
        public DataSet checkIfAssyLocallyExists(string strSQLServer, string strDataBase, string Assy, int FKCustomer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [PKAssembly]"+
                          ",[Assembly]"+
                          ",[Rows]"+
                          ",[Columns]"+
                          ",[FKCustomer]"+
                          ",[FKUserUpdater]"+
                          ",[LastUpdated]"+
                          ",[Available]"+
                          ",PalletID"+
                      " FROM CT_Assemblies"+
                      " WHERE [Assembly] = @Assy AND FKCustomer = @FKCustomer";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Assy", DataAccessNet.Command.ParameterType.NChar, Assy);
            objSQLCommand.AddParameter("@FKCustomer", DataAccessNet.Command.ParameterType.Int, FKCustomer);

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
        public DataSet checkIfAssyLocallyExistsUpd(string strSQLServer, string strDataBase, string Assy, int FKCustomer, string PalledId)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetLocallyAssemblyByCustomerTEMPORAL @FKCustomer, @Assy, @PalletId";
          
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Assy", DataAccessNet.Command.ParameterType.VarChar, Assy);
            objSQLCommand.AddParameter("@FKCustomer", DataAccessNet.Command.ParameterType.Int, FKCustomer);
            objSQLCommand.AddParameter("@PalletId", DataAccessNet.Command.ParameterType.VarChar, PalledId);

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
        public DataSet GetWIPID(string strSQLServer, string strDataBase, string Serial)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "SELECT [Wip_ID]"+
                          " FROM CHISQLV10A.[JEMS].[dbo].[WP_Wip]" +
                          " WHERE SerialNumber = @Serial";
            objSQLCommand = new DataAccessNet.Command(strSQL);
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

        public DataSet GetLinkMaterialID(string strSQLServer, string strDataBase, int AssyID, int DeviationID, string LinkObject)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetLinkMaterialID @AssyID, @DeviationID, @LinkObject";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@AssyID", DataAccessNet.Command.ParameterType.Int, AssyID);
            objSQLCommand.AddParameter("@DeviationID", DataAccessNet.Command.ParameterType.Int, DeviationID);
            objSQLCommand.AddParameter("@LinkObject", DataAccessNet.Command.ParameterType.NChar, LinkObject);
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

        public DataSet LinkNonUniqueComponent(string strSQLServer, string strDataBase, int WipID, string LinkData, int LinkMaterialID, string User, int RouteStepID, int EquipmentID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_LinkNonUniqueComponent @WipID, @LinkData, @LinkMaterialID, @User, @RouteStepID, @EquipmentID";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@WipID", DataAccessNet.Command.ParameterType.Int, WipID);
            objSQLCommand.AddParameter("@LinkData", DataAccessNet.Command.ParameterType.NChar, LinkData);
            objSQLCommand.AddParameter("@LinkMaterialID", DataAccessNet.Command.ParameterType.Int, LinkMaterialID);
            objSQLCommand.AddParameter("@User", DataAccessNet.Command.ParameterType.NChar, User);
            objSQLCommand.AddParameter("@RouteStepID", DataAccessNet.Command.ParameterType.Int, RouteStepID);
            objSQLCommand.AddParameter("@EquipmentID", DataAccessNet.Command.ParameterType.Int, EquipmentID);

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

        public DataSet UpdateProcessStatusOnMES(string strSQLServer, string strDataBase, int WipID, int RouteStepID, int EquipmentID, string User)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_UpdateProcessStatusOnMES @WipID, @RouteStepID, @EquipmentID, @User";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@WipID", DataAccessNet.Command.ParameterType.Int, WipID);
            objSQLCommand.AddParameter("@User", DataAccessNet.Command.ParameterType.NChar, User);
            objSQLCommand.AddParameter("@RouteStepID", DataAccessNet.Command.ParameterType.Int, RouteStepID);
            objSQLCommand.AddParameter("@EquipmentID", DataAccessNet.Command.ParameterType.Int, EquipmentID);

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
        public DataSet DeleteAssemblyUpd(string strSQLServer, string strDataBase,  int PKAssy)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_DelAssemblyUpd @PKAssembly";
            objSQLCommand = new DataAccessNet.Command(strSQL);

            objSQLCommand.AddParameter("@PKAssembly", DataAccessNet.Command.ParameterType.Int, PKAssy);

           

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
        public DataSet UpdateAssemblyUpd(string strSQLServer, string strDataBase, int PKAssembly,int Updater, string Assy, int Rows,
                                        int Columns, int FKCustomer, string PalletID, Boolean Panel, Boolean LoopValidation,
                                        int LoopsAllowed, Boolean CheckPointValidation)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_ChgAssemblyUpd @PKAssembly, @Updater, @Assy, @Rows, @Columns,  @FKCustomer, @PalletID, @Panel,@LoopValidation,@LoopsAllowed,@CheckPointValidation";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKAssembly", DataAccessNet.Command.ParameterType.Int, PKAssembly);
            objSQLCommand.AddParameter("@Updater", DataAccessNet.Command.ParameterType.Int, Updater);
            objSQLCommand.AddParameter("@Assy", DataAccessNet.Command.ParameterType.NChar, Assy);
            objSQLCommand.AddParameter("@Rows", DataAccessNet.Command.ParameterType.Int, Rows);
            objSQLCommand.AddParameter("@Columns", DataAccessNet.Command.ParameterType.Int, Columns);
            objSQLCommand.AddParameter("@FKCustomer", DataAccessNet.Command.ParameterType.Int, FKCustomer);
            objSQLCommand.AddParameter("@PalletID", DataAccessNet.Command.ParameterType.NChar, PalletID);
            objSQLCommand.AddParameter("@Panel", DataAccessNet.Command.ParameterType.Bit, Panel);
            objSQLCommand.AddParameter("@LoopValidation", DataAccessNet.Command.ParameterType.Bit, LoopValidation);
            objSQLCommand.AddParameter("@LoopsAllowed", DataAccessNet.Command.ParameterType.Int, LoopsAllowed);
            objSQLCommand.AddParameter("@CheckPointValidation", DataAccessNet.Command.ParameterType.Bit, CheckPointValidation);

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
