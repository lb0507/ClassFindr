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

        private List<BuildingModel> buildings = new ();

        public BuildingData(ISqlDataAccess db)
        {
            _db = db;
            FetchBuildings();
        }

        /// <summary>
        ///     Fetches a list of every building in the database
        /// </summary>
        /// <returns> A list of all buildings </returns>
        private async void FetchBuildings()
        {
            List<BuildingModel> selectedUser = new();

            try
            {
                // Form the SQL query
                string query = $"SELECT * FROM [dbo].[Building];";

                // Gets a list of all buildings
                buildings = await _db.LoadData<BuildingModel>(query);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public List<BuildingModel> GetBuildingList()
        {
            return buildings;
        }

    }
}
