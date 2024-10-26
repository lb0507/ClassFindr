/*
 *  Class for creating the basic foundation of a SQL 
 *  query to the ClassFindr database.
 *  
 *  Version 10.26.24
 */

using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary
{
    public class SqlDataAccess : ISqlDataAccess
    {

        private readonly IConfiguration _config;

        // Gets the connection string under "Default" in appsettings.json
        public string ConnectionStringName { get; set; } = "Default";

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        ///     Loads a singular item from the passed SQL query
        /// </summary>
        /// <typeparam name="T"> Generic object.  Can be any object model </typeparam>
        /// <param name="sql"> The SQL query to be executed </param>
        /// <param name="message"> The outputted success/warning/failure message </param>
        /// <returns></returns>
        public async Task<T?> LoadSingle<T>(string sql)
        {
            // Gets the connection string
            string connectionString = _config.GetConnectionString(ConnectionStringName) ?? "";

            try
            {

                // Open the connection and use it
                IDbConnection connection = new SqlConnection(connectionString);

                var data = await connection.QuerySingleAsync<T>(sql);
                connection.Close();     // Close connection, return space to pool

                return data;

            }
            catch (Exception)
            {
                throw;      // Throw the exception to be handeled by a specific method
            }
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            // Gets the connection string
            string connectionString = _config.GetConnectionString(ConnectionStringName) ?? "";

            // Open the connection and use it
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            // Gets the connection string
            string connectionString = _config.GetConnectionString(ConnectionStringName) ?? "";

            // Open the connection and use it
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

    }
}