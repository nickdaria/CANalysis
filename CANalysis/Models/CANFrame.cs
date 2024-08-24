using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CANalysis.Models
{
    public class CANFrame
    {
        public ushort ID { get; set; }
        public byte[] Data { get; set; } = { };
        public ushort Length { get; set; }
        public FrameFlags Flags { get; set; }
        public ulong TimestampMicros { get; set; }

        [StructLayout(LayoutKind.Explicit)]
        public struct FrameFlags
        {
            [FieldOffset(0)] public bool Extended;
            [FieldOffset(1)] public bool FlexibleDataRateFrame;
            [FieldOffset(2)] public bool RemoteRequest;
            [FieldOffset(3)] public bool DirectionTx;
        }
    }
}
