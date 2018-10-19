using PalletLinkWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PalletLinkWebAPI.Resources
{
    public abstract class PCBQueryService
    {

        public static Dictionary<string, string> getHistoryinfo(string pcbSerialNumber)
        {
            Dictionary<string, string> historyInfo = new Dictionary<string, string>();
            wsMes.Service wsMes = new wsMes.Service();
            DataSet dsSelectBySerialNumber = new DataSet();
            DataSet dsBoardHistory = new DataSet();
            DataSet dsUnitHistory = new DataSet();
            DataTable dtAssemblyID = new DataTable();
            DataTable dtBoardHistory = new DataTable();
            int lastRow;

            //Obtains information of the serial number in MES
            dsSelectBySerialNumber = wsMes.SelectBySerialNumber(pcbSerialNumber);
            if (dsSelectBySerialNumber.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            historyInfo.Add("WIP_ID", dsSelectBySerialNumber.Tables[0].Rows[0]["WIP_ID"].ToString());
            historyInfo.Add("Customer_ID", dsSelectBySerialNumber.Tables[0].Rows[0]["Customer_ID"].ToString());
            historyInfo.Add("CustomerText", dsSelectBySerialNumber.Tables[0].Rows[0]["CustomerText"].ToString());

            //Obtains information of the serial number history in MES
            dsBoardHistory = wsMes.BoardHistoryReport(pcbSerialNumber, Int32.Parse(historyInfo["Customer_ID"]));
            DataRow[] foundRows = dsBoardHistory.Tables[0].Select("TestType not like '%Desviation%' AND TestType not like '%Link%' AND TestType not like '%Unlink%'", "StartDatetime ASC");
            dtBoardHistory = foundRows.CopyToDataTable();
            lastRow = dtBoardHistory.Rows.Count - 1;
            historyInfo.Add("Assembly", dtBoardHistory.Rows[lastRow]["Assembly"].ToString());
            historyInfo.Add("Number", dtBoardHistory.Rows[lastRow]["Number"].ToString());
            historyInfo.Add("Revision", dtBoardHistory.Rows[lastRow]["Revision"].ToString());

            //Obtains the Assembly_ID of MES
            dtAssemblyID = wsMes.IDAssembly(historyInfo["Number"], historyInfo["Revision"], "");
            historyInfo.Add("Assembly_ID", dtAssemblyID.Rows[0]["Assembly_ID"].ToString());


            return historyInfo;

        }

        public static DataSet checkIfAssyLocallyExistsUpd(string assembly, int customerID, string palletID, string sqlConnection)
        {
            DataSet dataSet = new DataSet();

            using (var connection = new SqlConnection(sqlConnection))
            using (var command = new SqlCommand("up_GetLocallyAssemblyByCustomerPalletLink", connection)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@FKCustomer", customerID);
                command.Parameters.AddWithValue("@Assy", assembly);
                command.Parameters.AddWithValue("@PalletId", palletID);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = command;

                dataAdapter.Fill(dataSet);

                return dataSet;
            }

        }

        public static void insertLog(LinkingLog log, string sqlConnection)
        {
            using (var connection = new SqlConnection(sqlConnection))
            using (var command = new SqlCommand("up_AddLinkingLog", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@Customer_Id", log.pcb.customerID != null ? log.pcb.customerID.ToString() : "");
                command.Parameters.AddWithValue("@Customer", log.pcb.customerText != null ? log.pcb.customerText : "");
                command.Parameters.AddWithValue("@SerialNumber", log.palletSerial != null ? log.palletSerial : "");
                command.Parameters.AddWithValue("@PalletId", log.palletSerial != null ? log.palletSerial.ToString() : "");
                command.Parameters.AddWithValue("@Assembly_Id", log.pcb.assemblyID != null ? log.pcb.assemblyID : "");
                command.Parameters.AddWithValue("@Assembly", log.pcb.number != null ? log.pcb.number : "");
                command.Parameters.AddWithValue("@Wip_Id", log.pcb.wipID != null ? log.pcb.wipID.ToString() : "");
                command.Parameters.AddWithValue("@PanelNumberPL", log.palletSerial != null ? log.palletSerial : ""); //repetido
                command.Parameters.AddWithValue("@PanelSizePL", log.panelSizePL != null ? log.panelSizePL : "");
                command.Parameters.AddWithValue("@PanelSizeMES", log.panelSizeMES != null ? log.panelSizeMES : "");
                command.Parameters.AddWithValue("@LinkObject", log.linkObject != null ? log.linkObject : "");
                command.Parameters.AddWithValue("@LinkMaterialID", log.linkMaterialID != null ? log.linkMaterialID : "");
                command.Parameters.AddWithValue("@EquipmentValue", log.equipmentValue != null ? log.equipmentValue : "");
                command.Parameters.AddWithValue("@EquipmentName", log.equipmentName != null ? log.equipmentName : "");
                command.Parameters.AddWithValue("@RouteStepID", log.routeStepID != null ? log.routeStepID : "");
                command.Parameters.AddWithValue("@SerialLoops", log.serialLoops != null ? log.serialLoops : "");
                command.Parameters.AddWithValue("@LoopsAllowed", log.loopsAllowed != null ? log.loopsAllowed : "");
                command.Parameters.AddWithValue("@Message", log.message != null ? log.message : "");

                command.ExecuteNonQuery();
            }
        }

        public static int checkBoardCounterValidation(string serialNumber, int loopsNumber, PCB pcb, List<int> loopCustomers, string sqlConnection)
        {
            DataSet dsLoopsCounter = new DataSet();
            wsMes.Service wsMes = new wsMes.Service();
            int iProcessID = 0;
            int iHoldTypeID = 60;
            int iHolfByID = 1;
            string sHoldEMO = "Piece sent to Hold for exceeding the number of loops.";
            int iBoardStatus = 0;
            int iBoardLoops = 0;

            using (var connection = new SqlConnection(sqlConnection))
            using (var command = new SqlCommand("up_GetSerialLoopsCounterOtro", connection){
                    CommandType = CommandType.StoredProcedure})
            {   
                connection.Open();
                command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                command.Parameters.AddWithValue("@Assembly", pcb.assembly);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = command;

                dataAdapter.Fill(dsLoopsCounter);
            }

            if (dsLoopsCounter.Tables[0].Rows.Count > 0)
            {
                iBoardLoops = Convert.ToInt32(dsLoopsCounter.Tables[0].Rows[0]["LoopsNumber"]);
            }


            if (iBoardLoops >= loopsNumber){
                foreach (int element in loopCustomers)
                {
                    if (element.Equals(pcb.customerID))
                    {
                        iBoardStatus = 1;
                        return iBoardStatus;
                    }
                }
                iBoardStatus = wsMes.PlaceBoardOnHold(pcb.wipID, iProcessID, iHoldTypeID, iHolfByID, sHoldEMO);
                return iBoardStatus;
            }
            return iBoardStatus;

        }

        public static string getConfigurationByDescription(string description, string sqlConecction)
        {
            string result = null;

            using(SqlConnection connection = new SqlConnection(sqlConecction))
            using(SqlCommand command = new SqlCommand("up_GetConfigurationByDescription", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@description", description);

                SqlDataReader dataReader = command.ExecuteReader();

                if (!dataReader.HasRows)
                {
                    return null;
                }

                while (dataReader.Read())
                {
                    result = (string)dataReader["Value"];
                    break;
                }
                return result;
            }
        }

        public static DataSet getPalletLinkConfigByMachine(string pkMachine, string sqlConnection)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_GetPalletLinkConfig", connection){
                    CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@PKMachine", pkMachine);


                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = command;

                dataAdapter.Fill(dataSet);

                return dataSet;
            }
        }

        public static String registerLink(PCB pcb, Dictionary<string, string> initialConfig, String sqlConnection)
        {
            try
            {
                DataSet dsGetLoops = new DataSet();

                //Registers the link in the local database
                insertPalletLink(pcb, initialConfig, sqlConnection);

                //Update or insert the loop count
                dsGetLoops = getLoopsNumber(pcb, sqlConnection);

                if (!(dsGetLoops.Tables[0].Rows.Count > 0))
                {
                    initialConfig.Add("loopsNumber", "1");
                    insertBoardLoopsCounter(pcb, initialConfig, sqlConnection);
                    return "OK";
                }

                initialConfig.Add("PKBoardLoopCounter", dsGetLoops.Tables[0].Rows[0]["PKBoardLoopCounter"].ToString());
                initialConfig.Add("loopsNumber", dsGetLoops.Tables[0].Rows[0]["LoopsNumber"].ToString());
                updateBoardLoopsCounter(pcb, initialConfig, sqlConnection);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
            
        }

        private static void insertPalletLink(PCB pcb, Dictionary<string, string> initialConfig, String sqlConnection)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_InsertPalletLink", connection)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@SerialNumber", pcb.serialNumber);
                command.Parameters.AddWithValue("PalletId", initialConfig["palletID"]);
                command.Parameters.AddWithValue("@Operation", "PalletLink");
                command.Parameters.AddWithValue("@User", initialConfig["userName"]);
                command.Parameters.AddWithValue("@Available", 1);

                command.ExecuteNonQuery();

            }
        }

        private static DataSet getLoopsNumber(PCB pcb, string sqlConnection)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_GetSerialLoopsCounterOtro", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@SerialNumber", pcb.serialNumber);
                command.Parameters.AddWithValue("@Assembly", pcb.assembly);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(dataSet);

                return dataSet;
            }
        }

        private static void insertBoardLoopsCounter(PCB pcb, Dictionary<string, string> initialConfig, string sqlConnection)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_AddSerialLoopsCounterOtro", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@SerialNumber", pcb.serialNumber);
                command.Parameters.AddWithValue("@LoopsNumber", Convert.ToInt32(initialConfig["loopsNumber"]));
                command.Parameters.AddWithValue("@Userupdated", initialConfig["userName"]);
                command.Parameters.AddWithValue("@Assembly", pcb.assembly);

                command.ExecuteNonQuery();
            }
        }

        private static void updateBoardLoopsCounter(PCB pcb, Dictionary<string, string> initialConfig, string sqlConnection)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_UpdSerialLoopsCounter", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@PKBoardLoopCounter", Convert.ToInt32(initialConfig["PKBoardLoopCounter"]));
                command.Parameters.AddWithValue("@LoopsNumber", Convert.ToInt32(initialConfig["loopsNumber"]));
                command.Parameters.AddWithValue("@Userupdated ", initialConfig["userName"]);

                command.ExecuteNonQuery();
            }
        }

        public static void updateCounter(Dictionary<string, string> initialConfig, string sqlConnection)
        {
            int palletID = Int32.Parse(initialConfig["palletID"]);
            int newWash = Int32.Parse(initialConfig["washingCycles"]) + 1;
            int limitWash = Int32.Parse(initialConfig["limitWashingCycles"]);
            int newMaintenance = Int32.Parse(initialConfig["maintenanceCycles"] + 1);
            int limitMaintenance = Int32.Parse(initialConfig["limitMaintenance"]);
            

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_ChgToolingCountersPalletLink", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@ToolingID", palletID);
                command.Parameters.AddWithValue("@newWash", newWash);
                command.Parameters.AddWithValue("@newMain", newMaintenance);
                //10 = Status "In Use"
                //11 = Status "Burned"
                command.Parameters.AddWithValue("@FKStatusID", (newWash >= limitWash || newMaintenance >= limitMaintenance) ? 11 : 10);

                command.ExecuteNonQuery();
            }
        }

        public static DataSet getCustomerTimeOut(string customerID, string sqlConnection)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_GetPalletTimeOutByCustomer", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@FKCustomer", customerID);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(dataSet);

                return dataSet;
            }

        }

        public static DataSet getPalletTimeOut(Dictionary<string, string> initialConfig, bool isPalletValidation, string sqlConnection)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_GetPalletTimeOut", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("FKCustomer", initialConfig["customerID"]);
                command.Parameters.AddWithValue("@IsPalletValidation", isPalletValidation);
                command.Parameters.AddWithValue("@PalletName", initialConfig["palletSerial"]);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(dataSet);

                return dataSet;
            }
        }

        public static void updatePalletTimeOut(Dictionary<string, string> initialConfig, string sqlConnection)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("up_UpdatePalletTimeOut", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@PalletName", initialConfig["palletSerial"]);
                command.Parameters.AddWithValue("@FKCustome", initialConfig["customerID"]);
                command.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
                command.Parameters.AddWithValue("@AvailableDatetime", DateTime.Now.AddSeconds(Int32.Parse(initialConfig["palletTimeOut"])));

                command.ExecuteNonQuery();
            }
        }

        public static void insertPalletTimeOut(Dictionary<string, string> initialConfig, string sqlConnection)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            using (SqlCommand command = new SqlCommand("palletTimeOut", connection){
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@PalletName", initialConfig["palletSerial"]);
                command.Parameters.AddWithValue("@FKCustomer", initialConfig["customerID"]);
                command.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
                command.Parameters.AddWithValue("@AvailableDatetime", DateTime.Now.AddSeconds(Int32.Parse(initialConfig["palletTimeOut"])));

                command.ExecuteNonQuery();
            }
        }
    }
}