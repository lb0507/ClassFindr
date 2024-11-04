using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary.Models
{
    public class BuildingModel
    {
        public int BID { get; set; }

        public string? Name { get; set; }

        public string? Description{ get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public DateTime DateErected { get; set; }
    }
}
