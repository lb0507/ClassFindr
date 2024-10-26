
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
        /// <returns> True if the user can log in </returns>
        public async Task<bool> SignIn(string username, string password)
        {
            // Username sanitization.  Prevents any [redacted] in the class from doing SQL injection.
            if (username.Contains('\'')) return false;

            // Form the SQL query
            string query = $"SELECT * FROM [dbo].[User] WHERE [Username] = '{username}'";

            // Gets a list of users that match the query.  Should have only one user, unless we mess up somewhere
            UserModel? selectedUser = await _db.LoadSingle<UserModel>(query);

            Console.WriteLine(selectedUser?.ToString());

            string hashedPW = Utils.Security.Hash(password);    // Hash the password for submission

            return selectedUser?.Password == hashedPW;
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
