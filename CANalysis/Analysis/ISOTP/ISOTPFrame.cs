using CANalysis.Models;
using System;

namespace CANalysis.Analysis.ISOTP
{
    public static class ISOTPFrame
    {
        public enum FrameType
        {
            SingleFrame = 0,
            FirstFrame = 1,
            ConsecutiveFrame = 2,
            FlowControlFrame = 3
        };

        public enum FlowControlRequest
        {
            ContinueToSend = 0,
            Wait = 1,
            Abort = 2
        };

        public static FrameType? GetFrameType(CANFrame frame)
        {
            if (frame.Length < 1) { return null; }
            int extractedBits = (frame.Data[0] >> 4) & 0x0F;
            FrameType frameType = (FrameType)extractedBits;

            return frameType;
        }

        public static uint GetSingleFrameLength(CANFrame frame)
        {
            int extractedBits = frame.Data[0] & 0x0F;
            uint frameLength = (ushort)extractedBits;
            //  TODO: Implement CAN-FD first frame (https://en.wikipedia.org/wiki/CAN_FD#CAN_&_CAN_FD_TP_Headers
            return frameLength;
        }

        public static uint GetFirstFrameLength(CANFrame frame)
        {
            int extractedBits = frame.Data[0] & 0x0F;
            int extractedBits2 = frame.Data[1] & 0xFF;
            uint frameLength = (ushort)(extractedBits << 8 | extractedBits2);
            //  TODO: Implement CAN-FD first frame (https://en.wikipedia.org/wiki/CAN_FD#CAN_&_CAN_FD_TP_Headers

            return frameLength;
        }

        public static uint? GetConsecutiveFrameIndex(CANFrame frame)
        {
            uint extractedBits = (uint)(frame.Data[0] & 0x0F);
            if (extractedBits > 15) { return null; }
            return extractedBits;
        }

        private static FlowControlRequest? GetFlowControlRequest(CANFrame frame)
        {
            if (frame.Length < 1) { return null; }
            int extractedBits = frame.Data[0] & 0x0F;
            if (extractedBits > 2) { return null; }
            FlowControlRequest flowControlRequest = (FlowControlRequest)extractedBits;
            return flowControlRequest;
        }

        private static bool ValidSingleFrame(CANFrame frame)
        {
            if (frame.Length < 3) { return false; }
            if (GetFrameType(frame) != FrameType.SingleFrame) { return false; }
            if (GetSingleFrameLength(frame) > frame.Length - 1) { return false; }
            //  TODO: Implement CAN-FD single frame (https://en.wikipedia.org/wiki/CAN_FD#CAN_&_CAN_FD_TP_Headers)
            return true;
        }

        private static bool ValidFirstFrame(CANFrame frame)
        {
            if (frame.Length < 1) { return false; }
            if (GetFrameType(frame) != FrameType.FirstFrame) { return false; }
            if (GetFirstFrameLength(frame) <= frame.Length - 2) { return false; }
            //  TODO: Implement CAN-FD first frame (https://en.wikipedia.org/wiki/CAN_FD#CAN_&_CAN_FD_TP_Headers
            return true;
        }

        private static bool ValidConsecutiveFrame(CANFrame frame)
        {
            if (frame.Length < 1) { return false; }
            if (GetFrameType(frame) != FrameType.ConsecutiveFrame) { return false; }
            if (GetConsecutiveFrameIndex(frame) == null) { return false; }
            return true;
        }

        private static bool ValidFlowControlFrame(CANFrame frame)
        {
            if (frame.Length < 1) { return false; }
            if (GetFrameType(frame) != FrameType.FlowControlFrame) { return false; }
            if (GetFlowControlRequest(frame) == null) { return false; }
            return true;
        }

        public static byte[] GetFirstFrameData(CANFrame frame)
        {
            if (!ValidFirstFrame(frame))
            {
                throw new ArgumentException("Invalid Single Frame");
            }

            uint length = GetFirstFrameLength(frame);
            return frame.Data.Skip(2).Take((int)length).ToArray();
        }

        public static byte[] GetSingleFrameData(CANFrame frame)
        {
            if (!ValidSingleFrame(frame))
            {
                throw new ArgumentException("Invalid Single Frame");
            }

            uint length = GetSingleFrameLength(frame);
            return frame.Data.Skip(1).Take((int)length).ToArray();
        }

        public static byte[] GetConsecutiveFrameData(CANFrame frame)
        {
            if (!ValidConsecutiveFrame(frame))
            {
                throw new ArgumentException("Invalid Consecutive Frame");
            }

            return frame.Data.Skip(1).ToArray();
        }

        public static bool MatchesISOTPStandard(CANFrame frame)
        {
            FrameType? frameType = GetFrameType(frame);
            if (frameType == null) { return false; }

            switch (frameType)
            {
                case FrameType.SingleFrame:
                    return ValidSingleFrame(frame);
                case FrameType.FirstFrame:
                    return ValidFirstFrame(frame);
                case FrameType.ConsecutiveFrame:
                    return ValidConsecutiveFrame(frame);
                case FrameType.FlowControlFrame:
                    return ValidFlowControlFrame(frame);
                default:
                    return false;
            }
        }
    }
}
