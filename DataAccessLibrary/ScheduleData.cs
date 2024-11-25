using ClassFindrDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary
{
    public class ScheduleData : IScheduleData
    {
        private readonly ISqlDataAccess _db;    // Instance of the database connection
        private bool showScheduleRoute = false;

        public ScheduleData(ISqlDataAccess db)
        {
            _db = db;
        }

        public bool GetRouteShowable()
        {
            return showScheduleRoute;
        }


        /// <summary>
        ///     Fetches the classes in the user's built schedule
        /// </summary>
        /// <param name="user"> The use to search upon </param>
        /// <returns> A list of classes in the user's schedule </returns>
        public async Task<List<ClassModel>> GetUserSchedule(UserModel? user)
        {
            List<ClassModel> classes = new();

            try
            {
                // Form the SQL query
                string query = "SELECT (CID), (CourseCode), (Name), (Description), (Room), (Time), (Days), (BRef) " +
                                $"FROM [dbo].[Schedule] JOIN [dbo].[Class] ON CRef = CID WHERE URef = {user?.UID};";

                // Gets a list of all classes in the user's schedule
                classes = await _db.LoadData<ClassModel>(query);  
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return classes;
        }

        /// <summary>
        ///     Gets the classes NOT in the user's buiult schedule
        /// </summary>
        /// <param name="user"> The user to search upon </param>
        /// <returns> A list of classes that are not in the user's schedule </returns>
        public async Task<List<ClassModel>> GetClassesNotInSchedule(UserModel? user)
        {
            List<ClassModel> classes = new();

            try
            {
                // Form the SQL query
                string query = "SELECT * FROM " +
                              $"(SELECT * FROM[dbo].[Schedule] WHERE URef = {user?.UID}) S " +
                              @"RIGHT JOIN[dbo].[Class] C
                                ON S.CRef = C.CID WHERE S.CRef IS NULL; ";

                // Gets a list of all classes in the user's schedule
                classes = await _db.LoadData<ClassModel>(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return classes;
        }

        public async Task<bool> SaveSchedule(UserModel? user, List<ClassModel> schedule)
        {
            string query = @"BEGIN TRANSACTION [reset]
                                            BEGIN TRY " +
                                          $"DELETE FROM [dbo].[Schedule] WHERE URef = {user?.UID};" +
                                           "INSERT INTO [dbo].[Schedule] VALUES";

            try
            {
                // Form the query until the last item
                for (int i = 0; i < schedule.Count() - 1; i++)
                {
                    query += $" ({user?.UID}, {schedule.ElementAt(i).CID}),";
                }

                // Add the remainder of the query
                query += $" ({user?.UID}, {schedule.Last().CID});" +
                         @" COMMIT TRANSACTION [reset]
                            END TRY
                            BEGIN CATCH
                                ROLLBACK TRANSACTION [reset]
                            END CATCH";

                // Gets a list of users that match the query.  Should have only one user, unless we mess up somewhere
                await _db.SaveData(query);
                return true;
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool ScheduleMatches(int UID)
        {
            return true;
        }

        public void SetShowable(bool isShowable)
        {
            showScheduleRoute = isShowable;
        }
    }
}
