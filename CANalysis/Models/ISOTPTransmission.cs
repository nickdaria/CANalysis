using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * UDS Transmission Model
 * 
 * Bidirectional model for data recieved and send over UDS
 */

namespace CANalysis.Models
{
    public class ISOTPTransmission
    {
        public SignalDirection Direction { get; set; }
        public uint? SendingID { get; set; }
        public uint RecievingID { get; set; }
        public byte[] Data { get; set; } = { };
        public List<CANFrame> Frames { get; set; }
        public uint Length { get; set; }
        public ulong TimestampMicros { get; set; }

    }
}
