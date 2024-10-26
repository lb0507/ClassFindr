/*
*          --- 10/24 ---
*    Implemented Prepared statements
*    Added Sign Up method
*    Added Change Password method
*    Added Delete User method
*   
*    
*    WE NEED TO PUT THE CONNECTION STRING IN THE CONFIG FILE AND ENCRYPT IT -- THE PASSWORD IS VISIBLE
*/

using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{

    public class UserCalls
    {

        public static bool SignIn(string username, string password)
        {

            if (username.Contains('\'')) return false;  // If there a single quote in the username (SQL Injection), exit function

            // Connection string that links the function call to the database
            string connectionString = "Server=tcp:cf-server.database.windows.net,1433;Initial Catalog=ClassFinder-DB;Persist Security Info=False;User ID=CFAdmin24;Password=AdminPassword45;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection cnn = new SqlConnection(connectionString);    // The SQL connection primed with the connection string

            /*
             * There are MANY things that can go wrong (such as the IP not being allowed access, etc.)
             * 
             * Surround with a try catch to avoid headaches in the future
             */
            try
            {
                cnn.Open();     // Open the database connection

                // The query for this function call
                string query = $"SELECT * FROM [dbo].[User] WHERE [Username] = '{username}';";

                SqlCommand command = new SqlCommand(query, cnn);    // Create the command to be executed with the provided query
                SqlDataReader reader = command.ExecuteReader();   // Class for reading the query result.  Think StreamReader or Java's Scanner class but for the database
                reader.Read();  // Read the first (and should be only) entry

                // Get whether or not the inputted password is valid
                bool isValid = reader.GetString(2) == password;

                Console.WriteLine($"ID: {reader.GetInt64(0)} UN: {reader.GetString(1)}  PW: {reader.GetString(2)}  Email: {reader.GetString(3)}");

                // Close connection and reader
                // ===========================
                cnn.Close();
                reader.Close();
                // ===========================

                return isValid;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return false;
            }
        }

        public static bool SignUp(string email, string username, string password, string userType)
        {
            /*
             *   Sign up method -- Inputs: user's email, username, password, userType
             *   
             *   Simplify database by requiring usernames to be unique
             *   First check that the username does not already exist in the database
             *   If it is unique, insert it into database, return true
             *   If it is not, do not insert and return false
             *   --Will likely need to be modified later to allow schedules (addition of foreign key)--
             */

            // Connection string that links the function call to the database -- not sure if it is correct to have the code for the connection string in every method
            string connectionString = "Server=tcp:cf-server.database.windows.net,1433;Initial Catalog=ClassFinder-DB;Persist Security Info=False;User ID=CFAdmin24;Password=AdminPassword45;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection cnn = new SqlConnection(connectionString);    // right now cnn is being reused - might need to be renamed for each method

            try
            {
                cnn.Open();  //Open database connection

                string existing = (@"SELECT Username FROM [dbo].[User] WHERE EXISTS (SELECT Username FROM [dbo].[User] WHERE Username=@username;"); // this gets the username if it exists
                // Prepare statement
                SqlCommand com = new SqlCommand(existing, cnn);
                com.Parameters.AddWithValue("@username", username);
                com.Prepare();
                int exists = (int)com.ExecuteScalar();  // if the username is already in the database -> exists = 1, if it is not -> exists = 0

                if (exists == 1) // The username already exists
                {
                    Console.WriteLine("That username already exists.");
                    cnn.Close();
                    return false;
                }
                else
                {


                    string insert = "INSERT INTO [dbo].[User] (Username, Password, Email, UserType) VALUES (@username,@password, @email, @userType);";

                    SqlCommand cmd = new SqlCommand(insert, cnn);

                    //add the parameters to the SQL command
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = username;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@userType", userType);
                    cmd.Prepare();
                    int result = (int)cmd.ExecuteNonQuery();  // stores the number of rows affected into result

                    if (result > 0) //Display message to user according to whether or not they were successfully added
                    {
                        Console.WriteLine("Account Created! Welcome to ClassFindr.");
                    }
                    else
                    {
                        Console.WriteLine("There was an error creating your account.");
                    }

                    cnn.Close();
                    return (result > 0); // returns true if the user was successfully added, false if they were not

                }
            }
            catch (Exception e)  //there was an error 
            {
                Console.WriteLine($"Error: {e.Message}");
                return false;
            }
        }

        public static bool ChangePassword(int username, string oldpassword, string newpassword)
        {
            // Connection string that links the function call to the database
            string connectionString = "Server=tcp:cf-server.database.windows.net,1433;Initial Catalog=ClassFinder-DB;Persist Security Info=False;User ID=CFAdmin24;Password=AdminPassword45;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection cnn = new SqlConnection(connectionString);    // The SQL connection primed with the connection string

            try
            {
                /*
                 *  ChangePassword - Inputs: username, current (old) password, new password
                 * 
                 *  First get the current password for the corresponding username from the database 
                 *  Then make sure the password in database matches the old one input by the user
                 *  Last update the old password to be the new one input by user
                 */

                cnn.Open();
                // get the username and password from the database -- could be simplified to just get password
                const string query1 = @"SELECT * FROM [dbo].[User] WHERE Username=@username AND Password=@password;";
                SqlCommand search = new SqlCommand (query1, cnn);
                search.Parameters.AddWithValue("@username", username);
                search.Parameters.AddWithValue("@password", oldpassword);
                search.Prepare();
                SqlDataReader reader = search.ExecuteReader();   // Excute the query and gets the result set
                reader.Read();

                if (reader.GetString(2) == oldpassword) // if the input old password matches the one in the database, execute update, otherwise return false
                {

                    const string query2 = @"UPDATE [dbo].[User] SET Password=@password WHERE Username=@username;";

                    SqlCommand update = new SqlCommand(query2, cnn);    // Create the command to be executed with the provided query
                   
                    //update password
                    update.Parameters.AddWithValue("@password", newpassword);
                    update.Parameters.AddWithValue("@username", username);
                    update.Prepare();
                    update.ExecuteNonQuery();
                    Console.WriteLine("Your password has been updated.");
                    reader.Close();
                    cnn.Close();
                    return true;

                }
                else
                {
                    Console.WriteLine("Incorrect password.");
                    reader.Close();
                    cnn.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return false;
            }
        }

        public static bool DeleteUser(string username)
        {
            /*    Delete User -- Inputs: username, password
             * 
             *    This method find the user's account in the database by looking for a matching username and deletes the row
             */


            // Connection string that links the function call to the database
            string connectionString = "Server=tcp:cf-server.database.windows.net,1433;Initial Catalog=ClassFinder-DB;Persist Security Info=False;User ID=CFAdmin24;Password=AdminPassword45;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection cnn = new SqlConnection(connectionString);    // The SQL connection primed with the connection string

            try
            {
                cnn.Open();
                const string query = @"DELETE FROM [dbo].[User] WHERE Username=@username;";
                SqlCommand delete = new (query, cnn);
                delete.Parameters.AddWithValue("@username", username);
                delete.Prepare();
                int result = (int)delete.ExecuteNonQuery();  // stores the number of rows affected into result

                if (result > 0) //Display message to user according to whether or not they were successfully added
                {
                    Console.WriteLine("Account has been deleted.");
                }
                else
                {
                    Console.WriteLine("There was an error deleting your account.");
                }

                cnn.Close();
                return (result > 0); // returns true if the user was successfully deleted, false if they were not
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return false;
            }
        }
            

    }
}
