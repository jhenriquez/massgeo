using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassGeo.Domain
{
    public class OutboundMessage
    {
        public String PageView { get; set; }
        public String IpAddress { get; set; }
        public String Latitude { get; set; }
        public String Longitude { get; set; }
    }
}
