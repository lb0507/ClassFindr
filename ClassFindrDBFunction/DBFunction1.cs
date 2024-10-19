using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace ClassFindrDBFunction
{
    public class DBFunction1
    {
        private readonly ILogger<Function1> _logger;

        public DBFunction1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("DBFunction1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            var str = Environment.GetEnvironmentVariable("SqlServerCOnnection");
            using (SqlConnection conn = new(str))
            {
                conn.Open();
               // var query =
                   // "drop table is exists Product;" +
                   // "create table Class (cId primary key identity, Course int, Section Int, Name varchar(50), Building varchar(50), Room varchar(25), Time varchar(5)";
           
            }

            using SqlCommand cmd = new(query, conn);
            await cmd.ExecuteNonQueryAsync();
            return new OkObjectResult("Database updated.");
        }
    }
}
