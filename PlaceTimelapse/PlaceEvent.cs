using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse
{
    public class PlaceEvent
    {
        public long Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Username { get; set; }
        public int ColorId { get; set; }
        public DateTime Date { get; set; }
    }
}
