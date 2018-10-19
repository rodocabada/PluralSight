using Encryption4_5;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PalletLinkWebAPI.Models;
using PalletLinkWebAPI.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PalletLinkWebAPI.Controllers
{
    public class PCBController : ApiController
    {
        private string sqlServerPL = System.Configuration.ConfigurationManager.AppSettings["sqlServerPL"].ToString();
        private string sqlDatabasePL = System.Configuration.ConfigurationManager.AppSettings["sqlDatabasePL"].ToString();
        private string sqlServerMICT = System.Configuration.ConfigurationManager.AppSettings["sqlServerMICT"].ToString();
        private string sqlDatabaseMICT = System.Configuration.ConfigurationManager.AppSettings["sqlDatabaseMICT"].ToString();
        private string sqlConnectionPL;
        private string sqlConnectionMICT;
        private string palletID;
        private string palletSerial;
        private bool autoBreakPanel;
        private string machineName;
        private DataSet palletLinkConfig;
        private string plCustomerID;
        private string resultProcessTestData;
        private string userName;
        private Dictionary<string, string> initialConfig;

        /// <summary>
        /// Method that receives the request by POST with a JSON object as parameter
        /// Responsible for linking the PCB serial number with the pallet.
        /// Date: 09-25-2018
        /// Author: Luis Rodolfo Cabada Gamillo
        /// </summary>
        /// <param name="dataJSON"></param>
        /// <returns>HttpResponseMessage response</returns>
        /// POST api/pallet
        public HttpResponseMessage Post([FromBody]JObject dataJSON)
        {
            try
            {
                //Variables
                wsMes.Service wsMes = new wsMes.Service();
                string pcbSerialNumber = null;
                JToken token = null;
                Encryption encryption = new Encryption();
                Dictionary<string, string> historyInfo = new Dictionary<string, string>();
                Dictionary<int, string> unitsInPanel = new Dictionary<int, string>();
                Dictionary<int, PCB> unitsInPanelOK = new Dictionary<int, PCB>();
                initialConfig = new Dictionary<string, string>();
                var response = Request.CreateResponse(HttpStatusCode.OK);
                PCB pcb = new PCB();
                DataSet dsLocalAssembly = new DataSet();
                DataSet dsListByBoard = new DataSet();
                sqlConnectionPL = "Data Source=" + encryption.DecryptString(sqlServerPL) + ";Integrated Security=SSPI; Initial Catalog=" +
                    encryption.DecryptString(sqlDatabasePL) + ";";
                sqlConnectionMICT = "Data Source=" + encryption.DecryptString(sqlServerMICT) + ";Integrated Security=SSPI; Initial Catalog=" +
                    encryption.DecryptString(sqlDatabaseMICT) + ";";
                LinkingLog log = new LinkingLog();
                string validateSerialNumberResult;


                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                token = dataJSON;
                pcbSerialNumber = formatPCBSerial((string)token.SelectToken("pcbSerial"));
                plCustomerID = (string)token.SelectToken("customerID");
                palletID = (string)token.SelectToken("palletID");
                palletSerial = (string)token.SelectToken("palletSerial");
                autoBreakPanel = (bool)token.SelectToken("autoBreakPanel");
                machineName = (string)token.SelectToken("machineName");
                userName = (string)token.SelectToken("userName");

                palletLinkConfig = PCBQueryService.getPalletLinkConfigByMachine(machineName, sqlConnectionPL);

                if (palletLinkConfig.Tables[0].Rows.Count < 1)
                {
                    response.Content = new StringContent(getJSONResultPCB("Error PalletLink Config", pcb), Encoding.UTF8, "application/json");
                    return response;
                }

                //Agregar todos estos datos a un dictionario para no traerlos todos por separado Ejemplo InitialConfig.
                initialConfig.Add("Customer", palletLinkConfig.Tables[0].Rows[0]["Customer"].ToString());
                initialConfig.Add("MesEquipment", palletLinkConfig.Tables[0].Rows[0]["MesEquipment"].ToString());
                initialConfig.Add("RouteStep", palletLinkConfig.Tables[0].Rows[0]["RouteStep"].ToString());
                initialConfig.Add("LinkObject", palletLinkConfig.Tables[0].Rows[0]["LinkObject"].ToString());
                initialConfig.Add("Process", palletLinkConfig.Tables[0].Rows[0]["Process"].ToString());
                initialConfig.Add("machineName", machineName);
                initialConfig.Add("palletID", palletID);
                initialConfig.Add("palletSerial", palletSerial);
                initialConfig.Add("customerID", plCustomerID);
                initialConfig.Add("userName", userName);
                initialConfig.Add("washingCycles", (string)token.SelectToken("washingCycles"));
                initialConfig.Add("limitWashingCycles", (string)token.SelectToken("limitWashingCycles"));
                initialConfig.Add("maintenanceCycles", (string)token.SelectToken("maintenanceCycles"));
                initialConfig.Add("limitMaintenance", (string)token.SelectToken("limitMaintenance"));

                //Obtains information of the serial number in MES
                historyInfo = PCBQueryService.getHistoryinfo(pcbSerialNumber);
                if (historyInfo == null)
                {
                    response.Content = new StringContent(getJSONResultPCB("Does not exist in MES", pcb), Encoding.UTF8, "application/json");
                    return response;
                }

                //Obtains the values from dictionary historyInfo to reuse across the code
                pcb.wipID = Int32.Parse(historyInfo["WIP_ID"]);
                pcb.customerID = Int32.Parse(historyInfo["Customer_ID"]);
                pcb.customerText = historyInfo["CustomerText"];
                pcb.assembly = historyInfo["Assembly"];
                pcb.number = historyInfo["Number"];
                pcb.revision = historyInfo["Revision"];
                pcb.assemblyID = historyInfo["Assembly_ID"];

                //Obtain the pallet timeout by customer
                getPalletTimeOut(pcb);

                //Obtains the information from Pallet Link database
                dsLocalAssembly = PCBQueryService.checkIfAssyLocallyExistsUpd(pcb.number, Int32.Parse(plCustomerID), palletSerial, sqlConnectionPL);

                if (dsLocalAssembly.Tables[0].Rows.Count == 0)
                {
                    log = prepareLog(pcb, null);
                    PCBQueryService.insertLog(log, sqlConnectionPL);
                    log.message = getJSONResultPCB("Error assambly configuration", pcb);
                    response.Content = new StringContent(log.message, Encoding.UTF8, "application/json");
                    return response;
                }

                //-------A partir de este punto es posible saber si la tablilla viene en panel y dependiendo de esto se debe hacer un proceso u otro para
                //-------poder convivir con el MICT
                //Obtains information of the Panel from MES
                bool validatePanel = Boolean.Parse(dsLocalAssembly.Tables[0].Rows[0]["Panel"].ToString());
                dsListByBoard = wsMes.ListByBoard(pcb.wipID);


                if (!autoBreakPanel && validatePanel){
                    foreach (DataRow dataRow in dsListByBoard.Tables[0].Rows)
                    {
                        unitsInPanel.Add(Int32.Parse(dataRow["Mapping"].ToString()), dataRow["SerialNumber"].ToString());
                    }
                } 
                else
                {
                    unitsInPanel.Add(1, pcbSerialNumber);
                }

                //Validates the serial numbers
                foreach (int key in unitsInPanel.Keys){
                    //Obtains history information from MES
                    string snToValidate = unitsInPanel[key].ToString();
                    historyInfo = PCBQueryService.getHistoryinfo(snToValidate);
                    pcb = new PCB();
                    pcb.wipID = Int32.Parse(historyInfo["WIP_ID"]);
                    pcb.customerID = Int32.Parse(historyInfo["Customer_ID"]);
                    pcb.customerText = historyInfo["CustomerText"];
                    pcb.assembly = historyInfo["Assembly"];
                    pcb.number = historyInfo["Number"];
                    pcb.revision = historyInfo["Revision"];
                    pcb.assemblyID = historyInfo["Assembly_ID"];
                    pcb.serialNumber = snToValidate;

                    validateSerialNumberResult = validateSerialNumber(snToValidate, pcb, dsLocalAssembly);

                    if (!"OK".Equals(validateSerialNumberResult))
                    {
                        response.Content = new StringContent(validateSerialNumberResult, Encoding.UTF8, "application/json");
                        return response;
                    }

                    unitsInPanelOK.Add(key, pcb);
                }

                string resultLinkingProcess = linkingProcess(unitsInPanelOK);

                if (!"OK".Equals(resultLinkingProcess))
                {
                    response.Content = new StringContent(resultLinkingProcess, Encoding.UTF8, "application/json");
                    return response;
                }

                response.Content = new StringContent(getFinalResult(pcb, unitsInPanelOK), Encoding.UTF8, "application/json");
                return response;

            }
             catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(getJSONResultPCB(ex.Message, null), Encoding.UTF8, "application/json");
                return response;
            }
        }

        private string formatPCBSerial(string pcbSerial)
        {
            pcbSerial = pcbSerial.ToUpper(new System.Globalization.CultureInfo("es-ES", false));
            pcbSerial = pcbSerial.Replace("'", "-");
            pcbSerial = pcbSerial.Replace("Ñ", ":");
            return pcbSerial.Trim();
        }

        /// <summary>
        /// Method that receives the string result and the object pallet as parameters.
        /// Responsible for building a string with JSON format evaluating the result parameter.
        /// Date: 09-25-2018
        /// Author: Luis Rodolfo Cabada Gamillo
        /// </summary>
        /// <param name="result"></param>
        /// <param name="pallet"></param>
        /// <returns>string json</returns>
        private string getJSONResultPCB(string result, PCB pcb)
        {
            JObject json = new JObject();
            switch (result)
            {
                case "OK":
                    json = JObject.FromObject(pcb);
                    json.Add("Result", "OK");
                    return json.ToString();

                case "Does not exist in MES":
                    json.Add("Result", "The PCB serial number does not exist");
                    return json.ToString();

                case "Error assambly configuration":
                    json.Add("Result", "Error, assambly configuration not found in Pallet Link, please register the assambly.");
                    return json.ToString();

                case "Piece on hold":
                    json.Add("Result", "Board in HOLD, please contact to Quality department to release it.");
                    return json.ToString();

                case "Loops without Hold":
                    json.Add("Result", "Linking error. The board has exceeded the allowed linking number." + Environment.NewLine + 
                                        "Remove the unit of the pallet and press OK button to continue.");
                    return json.ToString();

                case "Loops with Hold":
                    json.Add("Result", "Status of the board ONHOLD because exceeded the allowed linking number," + Environment.NewLine +
                                        "please contact to Quality supervisor to release it." + Environment.NewLine +
                                        " Remove the unit from the pallet and press OK button to continue.");
                    return json.ToString();

                case "Error PalletLink Config":
                    json.Add("Result", "This equipment: " + machineName + " is not valid for Pallet Link." + Environment.NewLine + 
                                        "Please contact to Manufacture Engineering staff.");
                    return json.ToString();

                case "No linking material":
                    json.Add("Result", "No linking material was found for the next configuration: " + Environment.NewLine +
                                        "Process: " + initialConfig["Process"] + "." + Environment.NewLine +
                                        "Linking object: " + initialConfig["LinkObject"] + "." + Environment.NewLine +
                                        "Assembly ID: " + pcb.assemblyID + "." + Environment.NewLine +
                                        "Review: " + pcb.revision + "." + Environment.NewLine +
                                        "Please contact Quality staff.");
                    return json.ToString();

                case "No linking possible":
                    json.Add("Result", "It was not possible link the serial number: " + pcb.serialNumber + Environment.NewLine +
                                        "With the pallet: " + palletSerial);
                    return json.ToString();

                case "Error MES step":
                    json.Add("Result", "It cannot be send the \"Step\" to MES. " + Environment.NewLine +
                                        "The following error was found: " + Environment.NewLine +
                                        resultProcessTestData);
                    return json.ToString();

                default:
                    json.Add("Exception", result);
                    return json.ToString();
            }
        }

        private LinkingLog prepareLog(PCB pcb, DataSet logData)
        {
            LinkingLog log = new LinkingLog();
            log.pcb = pcb;
            log.palletID = palletID;
            log.palletSerial = palletSerial;
            if (logData != null && logData.Tables[0].Rows.Count > 0)
            {
                log.loopsAllowed = logData.Tables[0].Rows[0]["LoopsAllowed"].ToString();
            }
            return log;
        }

        private string validateSerialNumber(string serialNumberToValidate, PCB pcb, DataSet dsLocalAssembly)
        {
            wsMes.Service wsMes = new wsMes.Service();
            int rows = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["Rows"].ToString());
            int columns = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["Columns"].ToString());
            int panelTotal = rows * columns;
            bool validPanel = Boolean.Parse(dsLocalAssembly.Tables[0].Rows[0]["Panel"].ToString());
            bool checkPointVerification = Boolean.Parse(dsLocalAssembly.Tables[0].Rows[0]["CheckPointValidation"].ToString());
            bool checkLoops = Boolean.Parse(dsLocalAssembly.Tables[0].Rows[0]["LoopValidation"].ToString());
            int loopsNumber = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["LoopsAllowed"].ToString());
            int palletPieces = Int32.Parse(dsLocalAssembly.Tables[0].Rows[0]["Pieces"].ToString());

            //Checks if the piece is on Hold
            if (wsMes.BoardInHold(pcb.wipID))
            {
                LinkingLog log = new LinkingLog();
                log = prepareLog(pcb, dsLocalAssembly);
                log.message = getJSONResultPCB("Piece on hold", pcb);
                PCBQueryService.insertLog(log, sqlConnectionPL);
                return log.message;
            }

            //Checks MES checkpoint
            if (checkPointVerification)
            {
                DataSet dsCheckPoint = new DataSet();
                dsCheckPoint = wsMes.GetCheckPoint(serialNumberToValidate, "0", pcb.customerID, Int32.Parse(pcb.assemblyID));
                if (dsCheckPoint.Tables[0].Rows.Count > 0)
                {
                    LinkingLog log = new LinkingLog();
                    string message;
                    string errorCheckpoint = "Error checkpoint, ";
                    for (int i = 0; i < dsCheckPoint.Tables[0].Rows.Count; i++)
                    {
                        errorCheckpoint = errorCheckpoint + dsCheckPoint.Tables[0].Rows[i]["DescrText"].ToString() + ", ";
                    }
                    JObject json = new JObject();
                    message = "The following MES required processes are missing for the piece: "
                        + errorCheckpoint + " Please contact to Quality department to continue. Remove the unit from the pallet and press OK button to continue.";
                    json.Add("Result", message);
                    log = prepareLog(pcb, dsLocalAssembly);
                    log.message = json.ToString();  
                    PCBQueryService.insertLog(log, sqlConnectionPL);

                    return log.message;
                }
            }

            if (checkLoops)
            {
                List<int> loopCustomers = PCBQueryService.getConfigurationByDescription("Loops Validation by Customer", sqlConnectionMICT).Split(',')
                                                         .Select(Int32.Parse).ToList();

                int boardLoops = PCBQueryService.checkBoardCounterValidation(serialNumberToValidate, loopsNumber, pcb, loopCustomers, sqlConnectionPL);

                if (boardLoops > 0)
                {
                    LinkingLog log = new LinkingLog();
                    log = prepareLog(pcb, dsLocalAssembly);

                    if (loopCustomers != null)
                    {
                        foreach(int element in loopCustomers)
                        {
                            if (element.Equals(pcb.customerID))
                            {
                                    log.message = getJSONResultPCB("Loops without Hold", pcb);
                                    PCBQueryService.insertLog(log, sqlConnectionPL);
                                    return log.message;
                            }
                        }

                    }    
                    log.message = getJSONResultPCB("Loops with Hold", pcb);
                    PCBQueryService.insertLog(log, sqlConnectionPL);
                    return log.message;
                }
            }

            return "OK";

        }

        private string linkingProcess(Dictionary<int, PCB> unitsInPanelOK)
        {
            wsMesTis.MES_TIS wsMesTis = new wsMesTis.MES_TIS();
            wsMes.Service wsMes = new wsMes.Service();
            DataTable dtGetMaterialID = new DataTable();
            LinkingLog log = new LinkingLog();
            int linkMaterialID;
            bool linkOK = false;
            string result = null;

            foreach (KeyValuePair<int, PCB> element in unitsInPanelOK)
            {
                dtGetMaterialID = wsMes.GetMaterialID(Int32.Parse(element.Value.assemblyID), "");

                if (dtGetMaterialID.Rows.Count <= 0)
                {
                    log = prepareLog(element.Value, null);
                    log.message = getJSONResultPCB("No linking material", element.Value);
                    PCBQueryService.insertLog(log, sqlConnectionPL);
                    return log.message;
                }

                DataTable dtMaterialID = new DataTable();
                DataRow[] drMaterialID;

                drMaterialID = dtGetMaterialID.Select("LinkObject = '" + initialConfig["LinkObject"] + "'");
                dtMaterialID = drMaterialID.CopyToDataTable();

                linkMaterialID = Int32.Parse(dtMaterialID.Rows[0]["LinkMaterial_ID"].ToString());

                //linkOK = wsMes.EPS_LinkNonUniqueComponent(element.Value.customerID, element.Value.wipID, linkMaterialID, initialConfig["palletSerial"], 1, 1);

                //if (!linkOK)
                //{
                //    log = prepareLog(element.Value, null);
                //    log.message = getJSONResultPCB("No linking possible", element.Value);
                //    PCBQueryService.insertLog(log, sqlConnectionPL);
                //    return log.message;
                //}

                string sMES = "S" + element.Value.serialNumber + Environment.NewLine +
                                "C" + initialConfig["Customer"] + Environment.NewLine +
                                "I" + element.Value.customerText + Environment.NewLine +
                                "N" + initialConfig["MesEquipment"] + Environment.NewLine +
                                "P" + initialConfig["RouteStep"] + Environment.NewLine +
                                "O" + Environment.NewLine +
                                "p12" + Environment.NewLine +
                                "L" + Environment.NewLine +
                                "n" + element.Value.number + Environment.NewLine +
                                "r" + element.Value.revision + Environment.NewLine +
                                "TP" + Environment.NewLine +
                                "[" + DateTime.Now.AddSeconds(3).ToString("MM/dd/yyyy HH:mm:ss.fff") + Environment.NewLine +
                                "]" + DateTime.Now.AddSeconds(4).ToString("MM/dd/yyyy HH:mm:ss.fff") + Environment.NewLine;

                //resultProcessTestData = wsMesTis.ProcessTestData(sMES, "Generic");

                //if (!"PASS".Equals(resultProcessTestData.ToUpper()))
                //{
                //    log = prepareLog(element.Value, null);
                //    log.message = getJSONResultPCB("Error MES step", element.Value);
                //    PCBQueryService.insertLog(log, sqlConnectionPL);
                //    return log.message;
                //}

                //Registers the link in the local database.
                result = PCBQueryService.registerLink(element.Value, initialConfig, sqlConnectionPL);

                if (!"OK".Equals(result))
                {
                    log = prepareLog(element.Value, null);
                    log.message = result;
                    PCBQueryService.insertLog(log, sqlConnectionPL);
                    return log.message;
                }

            }

            //Finish the link
            PCBQueryService.updateCounter(initialConfig, sqlConnectionPL);
            updatePalletTimeOut();
            
            return "OK";

        }

        private string getFinalResult(PCB pcb, Dictionary<int, PCB> unitsInPanelOK)
        {
            //Falta devolver 2 datos, checar si en verdad es necesario devolverlos.
            //Ambos datos indican si la pieza viene panelizada.

            JObject json = new JObject();
            json.Add("serialNumbers", JObject.FromObject(unitsInPanelOK));
            return json.ToString();

        }

        private void getPalletTimeOut(PCB pcb)
        {
            DataSet dataSet = new DataSet();
            dataSet = PCBQueryService.getCustomerTimeOut(initialConfig["customerID"], sqlConnectionPL);

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                initialConfig.Add("palletTimeOut", dataSet.Tables[0].Rows[0]["TimeOut"].ToString());
            }
            initialConfig.Add("palletTimeOut", "120");


        }

        private void updatePalletTimeOut()
        {
            DataSet dataSet = new DataSet();

            dataSet = PCBQueryService.getPalletTimeOut(initialConfig, true, sqlConnectionPL);

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                PCBQueryService.updatePalletTimeOut(initialConfig, sqlConnectionPL);
                return;
            }

            PCBQueryService.insertPalletTimeOut(initialConfig, sqlConnectionPL);

        }
    }
}
