using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CLPalletLink
{
    public class SY_BoardLoopsCounter
    {
        public DataSet GetBoardLoopsNumber(string strSQLServer, string strDataBase, String SerialNumber, string Assembly)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_GetSerialLoopsCounterOtro @SerialNumber, @Assembly";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@SerialNumber", DataAccessNet.Command.ParameterType.VarChar, SerialNumber);
            objSQLCommand.AddParameter("@Assembly", DataAccessNet.Command.ParameterType.VarChar, Assembly);


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
        public DataSet InsertBoardLoopsCounter(string strSQLServer, string strDataBase, string SerialNumber,
                                               int LoopsNumber, string Userupdated, string Assembly)
                         
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_AddSerialLoopsCounterOtro @SerialNumber, @LoopsNumber, @Userupdated, @Assembly";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@SerialNumber", DataAccessNet.Command.ParameterType.VarChar, SerialNumber);
            objSQLCommand.AddParameter("@LoopsNumber", DataAccessNet.Command.ParameterType.Int, LoopsNumber);
            objSQLCommand.AddParameter("@Userupdated", DataAccessNet.Command.ParameterType.VarChar, Userupdated);
            objSQLCommand.AddParameter("@Assembly", DataAccessNet.Command.ParameterType.VarChar, Assembly);
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

        public DataSet UpdateBoardLoopsCounter(string strSQLServer, string strDataBase, int PKBoardLoopCounter,
                                                int LoopsNumber, string Userupdated)
        {
            DataSet ds = new DataSet();
            DataAccessNet.SQLDataSet objCom = new DataAccessNet.SQLDataSet();
            DataAccessNet.Command objSQLCommand = default(DataAccessNet.Command);
            string strSQL = null;
            strSQL = "EXEC up_UpdSerialLoopsCounter @PKBoardLoopCounter, @LoopsNumber, @Userupdated";
            objSQLCommand = new DataAccessNet.Command(strSQL);
            objSQLCommand.AddParameter("@PKBoardLoopCounter", DataAccessNet.Command.ParameterType.Int, PKBoardLoopCounter);
            objSQLCommand.AddParameter("@LoopsNumber", DataAccessNet.Command.ParameterType.Int, LoopsNumber);
            objSQLCommand.AddParameter("@Userupdated", DataAccessNet.Command.ParameterType.VarChar, Userupdated);

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

        public DataSet fnGetBoardLoopsNumber(string strSQLServer, string strDataBase, String SerialNumber, string Assembly)
        {

            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = null;
            strSQL = "EXEC up_GetSerialLoopsCounterOtro " +
                     "@SerialNumber = '" + SerialNumber + "', " +
                     "@Assembly = '" + Assembly + "' ";


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
        public DataSet fnInsertBoardLoopsCounter(string strSQLServer, string strDataBase, string SerialNumber,
                                               int LoopsNumber, string Userupdated, string Assembly)
        {

           
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = null;
            strSQL = "EXEC up_AddSerialLoopsCounterOtro " +
                     "@SerialNumber = '" + SerialNumber + "', " +
                     "@LoopsNumber = '" + LoopsNumber + "', " +
                     "@Userupdated = '" + Userupdated + "', " +
                     "@Assembly = '" + Assembly + "' ";
                   


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

        public DataSet fnUpdateBoardLoopsCounter(string strSQLServer, string strDataBase, int PKBoardLoopCounter,
                                                int LoopsNumber, string Userupdated)
        {
            wsSQL.SQLServerDBv2 SQL = new wsSQL.SQLServerDBv2();
            DataSet ds = new DataSet();

            string strSQL = null;
            strSQL = "EXEC up_UpdSerialLoopsCounter " + 
                     "@PKBoardLoopCounter = '" + PKBoardLoopCounter + "', " +
                     "@LoopsNumber = '" + LoopsNumber + "', " +
                     "@Userupdated = '" + Userupdated + "' ";

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
