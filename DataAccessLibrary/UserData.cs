
using ClassFindrDataAccessLibrary.Models;
using System.Data.SqlClient;

namespace ClassFindrDataAccessLibrary
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;    // Instance of the database connection

        private UserModel? _model;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        /// <summary>
        ///     Method for logging the user into the application
        /// </summary>
        /// <param name="username"> The inputted username </param>
        /// <param name="password"> The normal, unhashed password that the user has inputted  </param>
        /// <returns> Tuple containing if the user has been succefully signed in or not, and the appropriate message associated with the result </returns>
        public async Task<Tuple<bool, string>> SignIn(string username, string password)
        {
            // Username sanitization.  Prevents any [redacted] in the class from doing SQL injection.
            if (username.Contains('\'')) return new(false, "Invalid username");

            // Return false if the user has not entered a value
            else if (username.Length < 1) return new(false, "Please enter a username");
            else if (password.Length < 1) return new(false, "Please enter a password");

            try
            {
                // Form the SQL query
                string query = $"SELECT * FROM [dbo].[User] WHERE [Username] = '{username}'";

                // Gets a list of users that match the query.  Should have only one user, unless we mess up somewhere
                UserModel? selectedUser = await _db.LoadSingle<UserModel>(query);

                string hashedPW = Utils.Security.Hash(password);    // Hash the password for submission

                bool isValid = selectedUser?.Password == hashedPW;  // Get whether or not the password mathces

                if (isValid)
                {
                    _model = selectedUser;
                    return new(true, "Successful sign in");
                }
                else
                {
                    return new(false, "Credentials are invalid");
                }

            }
            catch (SqlException)
            {
                return new(false, "IP not allowed");
            }
            catch (InvalidOperationException)
            {
                return new(false, "Credentials are invalid");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new(false, "Unhandled error - contact administrator for details");
            }
        }

        /// <summary> Signs the user out by setting the selected model to null. </summary>
        public void SignOut() => _model = null;

        /// <summary>
        ///     Creates the user in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(UserModel user)
        {
            string existsQuery = $"SELECT * FROM [dbo].[User] WHERE [Username] = '{user.Username}' OR [Email] = '{user.Email}'";

            // Attempt to load a list.  If fails, then there is no associated user
            try
            {
                // Gets a list of users that match the query.  Should have only one user, unless we mess up somewhere
                await _db.LoadSingle<UserModel>(existsQuery);
                return false;
            }
            catch (Exception) {}

            // Hash the password before submission
            string hashedPW = Utils.Security.Hash(user.Password);

            string query = $"INSERT INTO [dbo].[User] ([Username], [Password], [Email], [UserType]) VALUES ('{user.Username}', '{hashedPW}', '{user.Email}', '{user.Type}')";

            await _db.SaveData(query);

            // Check if the user is now it the database
            try
            {
                // Gets a list of users that match the query.  Should have only one user, unless we mess up somewhere
                await _db.LoadSingle<UserModel>(existsQuery);
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        ///     Gets the user's current sign on information
        /// </summary>
        /// <returns> Currently signed in user model </returns>
        public UserModel? GetUserSignOnInfo() { return _model; }
    }
}
