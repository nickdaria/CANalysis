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
    public class UDSTransmission
    {
        public bool DirectionTx { get; set; }
        public ushort? SendingID { get; set; }
        public ushort RecievingID { get; set; }
        public byte[] Data { get; set; } = { };
        public ushort Length { get; set; }
    }
}
