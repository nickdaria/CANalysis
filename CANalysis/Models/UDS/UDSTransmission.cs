using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CANalysis.Models.UDS.Standards;

namespace CANalysis.Models.UDS
{
    public abstract class UDSTransmission
    {
        //  ISO-TP Transmission Used
        public ISOTPTransmission? ISOTPTransmission { get; internal set; }

        //  UDS Service ID
        public SID ServiceID { get; set; }

        //  Data
        public byte[]? Data { get; set; }

        //  Base Constructor
        public UDSTransmission(SID serviceID, byte[] data)
        {
            ServiceID = serviceID;
            Data = data;
        }
    }

    public class UDSRequest : UDSTransmission {
        public UDSRequest(SID serviceID, byte[] data)
            : base(serviceID, data) { }
    }

    public class UDSPositiveResponse : UDSTransmission {
        public UDSPositiveResponse(SID serviceID, byte[] data)
            : base(serviceID, data) { }
    }

    public class UDSNegativeResponse : UDSTransmission {
        public UDSNegativeResponse(SID serviceID, byte[] data)
            : base(serviceID, data) { }
    }
}
