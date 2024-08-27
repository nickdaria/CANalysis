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
        public bool DirectionTx { get; set; }
        public UInt32? SendingID { get; set; }
        public UInt32 RecievingID { get; set; }
        public byte[] Data { get; set; } = { };
        public List<CANFrame> Frames { get; set; }
        public uint Length { get; set; }
        public ulong TimestampMicros { get; set; }

    }
}
