
using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;    // Instance of the database connection

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

            try
            {
                // Form the SQL query
                string query = $"SELECT * FROM [dbo].[User] WHERE [Username] = '{username}'";

                // Gets a list of users that match the query.  Should have only one user, unless we mess up somewhere
                UserModel? selectedUser = await _db.LoadSingle<UserModel>(query);

                Console.WriteLine(selectedUser?.ToString());

                string hashedPW = Utils.Security.Hash(password);    // Hash the password for submission

                bool isValid = selectedUser?.Password == hashedPW;  // Get whether or not the password mathces

                if (isValid)
                {
                    return new(true, "Successful sign in");
                }
                else
                {
                    return new(false, "Invalid password");
                }

            }
            catch (Exception)
            {
                return new(false, "Failure logging in");
            }
        }

        /// <summary>
        ///     Creates the user in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateUser(UserModel user)
        {
            string query = @"INSERT INTO [dbo].[User] (Username, Password, Email, UserType, SRef) 
                             VALUES (@Username, @Password, @Email, @UserType, @SRef)";

            return _db.SaveData(query, user);
        }
    }
}
