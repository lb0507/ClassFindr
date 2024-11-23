using ClassFindrDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary
{
    public class ClassData : IClassData
    {
        private readonly ISqlDataAccess _db;    // Instance of the database connection

        public ClassData(ISqlDataAccess db)
        {
            _db = db;
        }

        /// <summary>
        ///     Returns the buildings assoicated with a user's schedule
        /// </summary>
        /// <param name="classes"> The list of classes that the user is in </param>
        /// <returns> The buildings that the classes are in </returns>
        public async Task<List<BuildingModel>> GetClassBuildings(List<ClassModel> classes)
        {
            List<BuildingModel> buildings = new();

            try
            {
                // Form the SQL query
                string query = "SELECT * FROM [dbo].[Building] WHERE ";

                // Form the 
                foreach (var c in classes.SkipLast(1))
                {
                    query += $"BID = {c.BRef} OR ";
                }

                query += $" BID = {classes.Last().BRef};";

                Console.WriteLine(query);

                // Gets a list of all buildings
                buildings = await _db.LoadData<BuildingModel>(query);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return buildings;
        }

    }
}
