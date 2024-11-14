using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary.Models
{
    public class BuildingModel : ISearchableItem
    {
        public int BID { get; set; }

        public string? Name { get; set; }

        public string? Description{ get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public DateTime? DateErected { get; set; }

        public string? ImageSource { get; set; }

        public override string ToString()
        {
            return $"({BID}) {Name}  [{Latitude}, {Longitude}] {DateErected}";
        }

        public string? GetSearchableAspect()
        {
            return Name;
        }
    }
}
