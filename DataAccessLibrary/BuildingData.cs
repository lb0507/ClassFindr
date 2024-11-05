using ClassFindrDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary
{
    public class BuildingData : IBuildingData
    {

        private readonly ISqlDataAccess _db;    // Instance of the database connection

        public BuildingData(ISqlDataAccess db)
        {
            _db = db;
        }

        /// <summary>
        ///     Fetches a list of every building in the database
        /// </summary>
        /// <returns> A list of all buildings </returns>
        public async Task<List<BuildingModel>> FetchBuildings()
        {
            List<BuildingModel> selectedUser = new();

            try
            {
                // Form the SQL query
                string query = $"SELECT * FROM [dbo].[Building];";

                // Gets a list of all buildings
                selectedUser = await _db.LoadData<BuildingModel>(query);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return selectedUser;
        }

    }
}
