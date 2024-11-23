using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary.Models
{
    public class ClassModel : ISearchableItem
    {
        public int CID { get; set; }

        public string? CourseCode { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Room { get; set; }

        public DateTime Time { get; set; }

        public string? Days { get; set; }

        public int BRef { get; set; }

        public string? GetSearchableAspect()
        {
            return $"{Name} ({CourseCode})";
        }
    }
}
