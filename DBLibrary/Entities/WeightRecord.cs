using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLibrary.Entities
{
    public class WeightRecord
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
    }
}
