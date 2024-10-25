/*
 *  ------- 10/24 --------
 *  Added Get Location method
 *  Added Get Info method
 * 
 */

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassFinder_API
{

    // These structs might need to be moved inside of class BuildingCalls?
    public struct Location  
    {
         public float longitude;
         public float latitude;

        public Location(float longitude, float latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }
    }
    public struct Info
    {
        public string? Description;
        public DateTime DateErected;

        public Info(string? Description, DateTime DateErected) : this()
        {
            this.Description = Description;
            this.DateErected = DateErected;
        }
    }
	public class BuildingCalls
	{

        public static Location GetLocation(string name) // input name -> return struct Location containing latitude & longitude
        {

            // Connection string that links the function call to the database
            string connectionString = "Server=tcp:cf-server.database.windows.net,1433;Initial Catalog=ClassFinder-DB;Persist Security Info=False;User ID=CFAdmin24;Password=AdminPassword45;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection cnn = new SqlConnection(connectionString);    // The SQL connection primed with the connection string

            try
            {
                cnn.Open();
                const string query1 = "SELECT Longitude, Latitude FROM [dbo].[Building] WHERE name=@name;";  // get the building row by finding the name
                SqlCommand loc = new SqlCommand(query1, cnn);
                loc.Parameters.AddWithValue("@name", name);
                loc.Prepare();
                SqlDataReader reader = loc.ExecuteReader();
                reader.Read();
                float longitude = reader.GetFloat(4);                       // get the longitude
                float latitude = reader.GetFloat(5);                      // get the latitude
                Location position = new Location(longitude, latitude);      // store it in a Location struct and return it
                reader.Close();
                cnn.Close();
                return position;
            }
            catch (Exception e) 
            {
                Console.WriteLine($"Error: {e.Message}");
                // since GetLocation returns a struct (Location), not sure if we can return false in case of an error
                // set a Location with values set to zero to return instead
                //return false;
                Location empty = new Location(0.0f ,0.0f);
                return empty;
            }
        }

        

        public static Info GetInfo(string name) // input name -> return struct Info containing description & date erected
        {
            // Connection string that links the function call to the database
            string connectionString = "Server=tcp:cf-server.database.windows.net,1433;Initial Catalog=ClassFinder-DB;Persist Security Info=False;User ID=CFAdmin24;Password=AdminPassword45;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection cnn = new SqlConnection(connectionString);    // The SQL connection primed with the connection string

            try
            {
                cnn.Open();
                const string query2 = "SELECT Description, DateErected FROM [dbo].[Building] WHERE name=@name;";  // get the building row by finding the name
                SqlCommand info = new SqlCommand(query2, cnn);
                info.Parameters.AddWithValue("@name", name);
                info.Prepare();
                SqlDataReader reader = info.ExecuteReader();
                reader.Read();
                string desc = reader.GetString(3);          // get the description
                DateTime date = reader.GetDateTime(6);      // get the date
                Info information = new Info(desc, date);    // store the above in a struct and return in
                reader.Close();
                cnn.Close();
                return information;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");     // in case of an exception, return a null string and January 1, 0001, 00:00:00
                DateTime noTime = DateTime.MinValue;
                Info none = new Info(null, noTime);
                return none;
            }

        }
    }
}
