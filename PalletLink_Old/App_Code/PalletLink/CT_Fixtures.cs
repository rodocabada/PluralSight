using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class CT_Fixtures
    {
        public DataSet GetPallets(string strSQLServer, string strDataBase, string Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetPalletsByCustomer @Customer";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Customer", DataAccessNet.Command.ParameterType.VarChar, Customer);

            try{
                ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ds = null;
            }
            return ds;
        }

        public DataSet checkIfPalletExists(string strSQLServer, string strDataBase, string PalletID)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "up_GetPalletStatusAndCounters @GRN";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@GRN", DataAccessNet.Command.ParameterType.VarChar, PalletID);

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

        public DataSet GetFixtureStatus(string strSQLServer, string strDataBase, string Fixture, string Customer)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC [dbo].[up_CheckFixtureMaintenanceStatusOnFCS] @Fixture, @Customer";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Fixture", DataAccessNet.Command.ParameterType.NChar, Fixture);
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
        
        public DataSet StatusPallet(string strSQLServer, string strDataBase, string FKFixture)
        {
         
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC [dbo].[up_GetPalletStatusGral] @Fixture";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Fixture", DataAccessNet.Command.ParameterType.VarChar, FKFixture);
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
     
        public  DataSet PalletCycles(string strSQLServer, string strDataBase, string FKFixture ) 
     {
         DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC [dbo].[up_GetPalletCountGral] @Fixture";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@Fixture", DataAccessNet.Command.ParameterType.VarChar, FKFixture);
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

        public  void UpdateCounter(string strSQLServer, string strDataBase, string FKFixture, int NumberOfCycles, int AccumulatedCycles, 
                                    int CycleLimit, int MaintenanceCycles, int MaintenanceCyclesLimit,  DateTime LastUpdated, int FKUserUpdater)
         {
             DataSet ds = new DataSet();
             DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
             DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
             string strSQL = null;
             strSQL = "EXEC up_ChgPaceLineCyclesGral @FKFixture, @NumberOfCycles, @AccumulatedCycles, @CycleLimit, @MaintenanceCycles, @MaintenanceCyclesLimit, @LastUpdated, @FKUserUpdater";
             objSQLCommand = new DataAccessNet.Command(strSQL);
             objSQLCommand.AddParameter("@FKFixture", DataAccessNet.Command.ParameterType.VarChar, FKFixture);
             objSQLCommand.AddParameter("@NumberOfCycles", DataAccessNet.Command.ParameterType.Int, NumberOfCycles);
             objSQLCommand.AddParameter("@AccumulatedCycles", DataAccessNet.Command.ParameterType.Int, AccumulatedCycles);
             objSQLCommand.AddParameter("@CycleLimit", DataAccessNet.Command.ParameterType.Int, CycleLimit);
             objSQLCommand.AddParameter("@MaintenanceCycles", DataAccessNet.Command.ParameterType.Int, MaintenanceCycles);
             objSQLCommand.AddParameter("@MaintenanceCyclesLimit", DataAccessNet.Command.ParameterType.Int, MaintenanceCyclesLimit);
             objSQLCommand.AddParameter("@LastUpdated", DataAccessNet.Command.ParameterType.DateTime, LastUpdated);
             objSQLCommand.AddParameter("@FKUserUpdater", DataAccessNet.Command.ParameterType.Int, FKUserUpdater);

             try
             {
                 ds = objCom.SelectResults(strSQLServer, strDataBase, objSQLCommand);
                 
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
                 ds = null;
             }
             //return ds;
         }
    }
}
