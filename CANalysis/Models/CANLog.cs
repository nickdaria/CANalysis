using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANalysis.Models
{
    public class CANLog
    {
        public DateTime log_start;
        public List<CANFrame> frames = new();
    }
}
