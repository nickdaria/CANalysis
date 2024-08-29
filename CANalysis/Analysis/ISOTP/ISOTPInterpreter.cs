using CANalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANalysis.Analysis.ISOTP
{
    public static class ISOTPInterpreter
    {
        public static List<ISOTPTransmission> ReadTransmissions(CANLog log)
        {
            var transmissions = new List<ISOTPTransmission>();

            // Iterate through each frame in the log
            foreach (var frame in log.frames)
            {
                var frameType = ISOTPFrame.GetFrameType(frame);

                // Skip non-ISO-TP frames
                if (frameType == null)
                {
                    continue;
                }

                //  Handle based on type
                switch (frameType)
                {
                    case ISOTPFrame.FrameType.SingleFrame:
                    {
                        var transmission = new ISOTPTransmission
                        {
                            Direction = frame.Direction,
                            SendingID = frame.ID,
                            TimestampMicros = frame.TimestampMicros,
                            Data = ISOTPFrame.GetSingleFrameData(frame),
                            Length = ISOTPFrame.GetSingleFrameLength(frame),
                            Frames = new List<CANFrame> { frame }
                        };

                        transmissions.Add(transmission);
                        break;
                    }
                    case ISOTPFrame.FrameType.FirstFrame:
                    {
                        // Create a new transmission for first frame
                        var transmission = new ISOTPTransmission
                        {
                            Direction = frame.Direction,
                            SendingID = frame.ID,
                            TimestampMicros = frame.TimestampMicros,
                            Data = ISOTPFrame.GetFirstFrameData(frame),
                            Length = ISOTPFrame.GetFirstFrameLength(frame),
                            Frames = new List<CANFrame> { frame }
                        };

                        transmissions.Add(transmission);
                        break;
                    }
                    case ISOTPFrame.FrameType.ConsecutiveFrame:
                    {
                        //  Append to last transmission on consecutive frames
                        var lastTransmission = transmissions.LastOrDefault(t => t.SendingID == frame.ID);

                        if (lastTransmission != null)
                        {
                            int remainingBytes = (int)(lastTransmission.Length - lastTransmission.Data.Length);

                            if (remainingBytes > 0)
                            {
                                var consecutiveData = ISOTPFrame.GetConsecutiveFrameData(frame);
                                lastTransmission.Data = lastTransmission.Data
                                    .Concat(consecutiveData.Take(remainingBytes))
                                    .ToArray();

                                lastTransmission.Frames.Add(frame);
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Consecutive frame received without a preceding first frame.");
                        }
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }

            // No need to sort, as frames are processed in order they appear in the log
            return transmissions;
        }
    }
}
