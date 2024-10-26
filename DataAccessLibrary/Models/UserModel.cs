using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary.Models
{
    public class UserModel
    {
        public Int64 UID { get; set; }

        public string? Username { get; set; }
        
        public string? Password { get; set; }
        
        public string? Email { get; set; }
        
        public string? Type { get; set; }

        public int? ScheduleRef { get; set; }

        public override string ToString()
        {
            return $"({UID}) - {Username}  {Email}";
        }

    }
}
