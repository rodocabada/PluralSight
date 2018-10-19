using PalletLinkWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PalletLinkWebAPI.Resources
{
    public class PalletQueryService
    {

        /// <summary>
        /// Method that receives the pallet serial, customer id and the sql connection as parameters.
        /// Responsible for querying the data that corresponds to the pallet serial and customer id.
        /// Date: 09-25-2018
        /// Author: Luis Rodolfo Cabada Gamillo
        /// </summary>
        /// <param name="palletSerial"></param>
        /// <param name="customerID"></param>
        /// <param name="sqlConnection"></param>
        /// <returns>Pallet pallet</returns>
        public static Pallet getPalletInformation(string palletSerial, int customerID, string sqlConnection)
        {
            var pallet = new Pallet();

            using (var connection = new SqlConnection(sqlConnection))
            using (var command = new SqlCommand("up_GetPalletStatusAndCounters", connection)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                connection.Open();
                command.Parameters.AddWithValue("@GRN", palletSerial);
                command.Parameters.AddWithValue("@CustomerID", customerID);

                SqlDataReader dataReader = command.ExecuteReader();

                if (!dataReader.HasRows)
                {
                    return null;
                }

                while (dataReader.Read())
                {
                    pallet.id = (int)dataReader["PKToolingID"];
                    pallet.status = (string)dataReader["Description"];
                    pallet.quantity = (int)dataReader["Quantity"];
                    pallet.washingCycles = (int)dataReader["CurrentWash"];
                    pallet.limitWashingCycles = (int)dataReader["LimitWash"];
                    pallet.maintenanceCycles = (int)dataReader["CurrentMain"];
                    pallet.limitMaintenance = (int)dataReader["LimitMain"];
                    pallet.washingDate = (DateTime)dataReader["LastDateWash"];
                    break;
                }
                return pallet;
            }
        }


        public static Dictionary<string, string> getAvailableValidations(string sqlConnection)
        {
            Dictionary<string, string> availableValidations = new Dictionary<string, string>();

            using (var connection = new SqlConnection(sqlConnection))
            using (var command = new SqlCommand("up_GetAvailableValidations", connection))
            {
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                if (!dataReader.HasRows)
                {
                    return null;
                }

                while (dataReader.Read())
                {
                    availableValidations.Add((string)dataReader["Description"], (string)dataReader["Description"]);

                }

                return availableValidations;
            }
        }

    }
}