using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrace
{
    public class Cargo
    {
        public string OwnerName { get; set; }
        public string OwnerSurname { get; set; }
        public string CargoRFIDNumber { get; set; }
        public string OwnerEmail { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
