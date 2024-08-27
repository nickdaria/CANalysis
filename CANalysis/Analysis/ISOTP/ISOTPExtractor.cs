using CANalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANalysis.Analysis.ISOTP
{
    public static class ISOTPExtractor
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

                if (frameType == ISOTPFrame.FrameType.SingleFrame)
                {
                    // Create a new transmission for single frame
                    var transmission = new ISOTPTransmission
                    {
                        DirectionTx = frame.Direction == SignalDirection.Tx,
                        SendingID = frame.ID,
                        TimestampMicros = frame.TimestampMicros,
                        Data = ISOTPFrame.GetSingleFrameData(frame),
                        Length = ISOTPFrame.GetSingleFrameLength(frame),
                        Frames = new List<CANFrame> { frame }
                    };

                    transmissions.Add(transmission);
                }
                else if (frameType == ISOTPFrame.FrameType.FirstFrame)
                {
                    // Create a new transmission for first frame
                    var transmission = new ISOTPTransmission
                    {
                        DirectionTx = frame.Direction == SignalDirection.Tx,
                        SendingID = frame.ID,
                        TimestampMicros = frame.TimestampMicros,
                        Data = ISOTPFrame.GetFirstFrameData(frame),
                        Length = ISOTPFrame.GetFirstFrameLength(frame),
                        Frames = new List<CANFrame> { frame }
                    };

                    transmissions.Add(transmission);
                }
                else if (frameType == ISOTPFrame.FrameType.ConsecutiveFrame)
                {
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
                        throw new InvalidOperationException("Consecutive frame received without a preceding First Frame.");
                    }
                }
            }

            // No need to sort, as frames are processed in order they appear in the log
            return transmissions;
        }
    }
}
