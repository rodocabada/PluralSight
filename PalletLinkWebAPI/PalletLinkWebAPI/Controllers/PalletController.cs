using Encryption4_5;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PalletLinkWebAPI.Models;
using PalletLinkWebAPI.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PalletLinkWebAPI.Controllers
{
    public class PalletController : ApiController
    {
        private string sqlServerPL = System.Configuration.ConfigurationManager.AppSettings["sqlServerPL"].ToString();
        private string sqlDatabasePL = System.Configuration.ConfigurationManager.AppSettings["sqlDatabasePL"].ToString();
        private string sqlServerMICT = System.Configuration.ConfigurationManager.AppSettings["sqlServerMICT"].ToString();
        private string sqlDatabaseMICT = System.Configuration.ConfigurationManager.AppSettings["sqlDatabaseMICT"].ToString();
        private string sqlConnectionPL;
        private string sqlConnectionMICT;
        private const string statusValidation = "STATUS VALIDATION";
        private const string washingCyclesValidation = "WASHING CYCLES VALIDATION";
        private const string palletMaintenanceValidation = "PALLET MAINTENANCE VALIDATION";

        /// <summary>
        /// Method that receives the request by POST with a JSON object as parameter
        /// Responsible for finding the pallet serial number contained in the JSON object parameter.
        /// Date: 09-24-2018
        /// Author: Luis Rodolfo Cabada Gamillo
        /// </summary>
        /// <param name="dataJSON"></param>
        /// <returns>HttpResponseMessage response</returns>
        /// POST api/pallet
        public HttpResponseMessage Post([FromBody]JObject dataJSON)
        {
            try
            {
                Encryption encryption = new Encryption();
                sqlConnectionPL = "Data Source=" + encryption.DecryptString(sqlServerPL) + ";Integrated Security=SSPI; Initial Catalog=" +
                    encryption.DecryptString(sqlDatabasePL) + ";";
                sqlConnectionMICT = "Data Source=" + encryption.DecryptString(sqlServerMICT) + ";Integrated Security=SSPI; Initial Catalog=" +
                    encryption.DecryptString(sqlDatabaseMICT) + ";";

                JToken token = dataJSON;
                string palletSerial = (string)token.SelectToken("palletSerial");
                int customerID = (int)token.SelectToken("customerID");
                var pallet = PalletQueryService.getPalletInformation(palletSerial, customerID, sqlConnectionPL);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                Dictionary<string, string> availableValidations = new Dictionary<string, string>();
                

                //If the pallet is null it means that it does not exist in the database.
                if (pallet == null)
                {
                    response.Content = new StringContent(getJSONResultPallet("Does not exist", pallet), Encoding.UTF8, "application/json");
                    return response;
                }

                //Querying the available validations to apply to the pallet. 
                availableValidations = PalletQueryService.getAvailableValidations(sqlConnectionMICT);

                if (availableValidations != null)
                {
                    if (availableValidations.ContainsKey(statusValidation) && !validatePalletStatus(pallet))
                    {
                        response.Content = new StringContent(getJSONResultPallet("Status invalid", pallet), Encoding.UTF8, "application/json");
                        return response;
                    }

                    if (availableValidations.ContainsKey(washingCyclesValidation) && !validatePalletWashingCycles(pallet))
                    {
                        response.Content = new StringContent(getJSONResultPallet("Limit Washing", pallet), Encoding.UTF8, "application/json");
                        return response;
                    }

                    if (availableValidations.ContainsKey(palletMaintenanceValidation) && !validatePalletMaintenanceCycles(pallet)){
                        response.Content = new StringContent(getJSONResultPallet("Limit Maintenance", pallet), Encoding.UTF8, "application/json");
                        return response;
                    }
                }

                response.Content = new StringContent(getJSONResultPallet("OK", pallet), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(getJSONResultPallet(ex.Message, null), Encoding.UTF8, "application/json");
                return response;
            }
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
        private string getJSONResultPallet(string result, Pallet pallet)
        {
            JObject json = new JObject();
            switch (result)
            {
                case "OK":
                    json = JObject.FromObject(pallet);
                    json.Add("Result", "OK");
                    return json.ToString();

                case "Does not exist":
                    json.Add("Result", "The pallet serial number doesn't exist");
                    return json.ToString();

                case "Status invalid":
                    json.Add("Result", "The status of the pallet " + pallet.status + " is not valid." + Environment.NewLine + "Please take it to the pallet room.");
                    return json.ToString();

                case "Limit Washing":
                    json.Add("Result", "The pallet exceeded the " + pallet.limitWashingCycles + " washing cycles." + Environment.NewLine + "Please take it to the pallet room.");
                    return json.ToString();

                case "Limit Maintenance":
                    json.Add("Result", "The pallet exceeded the " + pallet.limitMaintenance + " maintenance cycles." + Environment.NewLine + "Please take it to the pallet room.");
                    return json.ToString();

                default:
                    json.Add("Exception", result);
                    return json.ToString();
            }
        }

        private bool validatePalletStatus(Pallet pallet)
        {
            if (!"In Use".Equals(pallet.status))
            {
                return false;
            }
            return true;
        }

        private bool validatePalletWashingCycles(Pallet pallet)
        {
            if (pallet.washingCycles >= pallet.limitWashingCycles)
            {
                return false;
            }
            return true;
        }

        private bool validatePalletMaintenanceCycles(Pallet pallet)
        {
            if (pallet.maintenanceCycles >= pallet.limitMaintenance)
            {
                return false;
            }
            return true;
        }

    }
}
