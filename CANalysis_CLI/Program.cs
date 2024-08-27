using CANalysis.Models;
using CANalysis.Filtering;
using CANalysis.Parsing;
using System;
using System.IO;
using System.Linq;
using System.Text;

/*
 *  Nick Daria 2024
 * 
 *  This is currently a testbed for the CANalysis library. It will be turned into a proper CLI tool in the future.
 * 
 */

namespace CANalysis_CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("UDS Test");

            string path = @"I:\My Drive\Q50 CAN Reverse Engineering\can-logs\EcuTek flashing logs day 2\stock-to-stock very fast was likely just checking hashes.csv";
            string savePath = Path.Combine(Path.GetDirectoryName(path), "ISO-LOG.txt");

            //  Parse the file at path
            ParserBase parser = new GVRET_CSV();
            CANLog log;

            using (var fileStream = File.OpenRead(path))
            {
                log = parser.Parse(fileStream);
            }

            //  Create a filtered log that only includes frames with ID 0x7E0
            var filter = new CANFrameFilter().ByID(id => id == 0x7E0 || id == 0x7E8);
            var filteredFrames = filter.Apply(log.frames).ToList();

            CANLog filteredLog = new CANLog
            {
                log_start = log.log_start,
                frames = filteredFrames
            };


            List<ISOTPTransmission> isotp = CANalysis.Analysis.ISOTP.ISOTPExtractor.ReadTransmissions(filteredLog);

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            using (var fileStream = File.Create(savePath))
            using (var writer = new StreamWriter(fileStream, Encoding.ASCII))
            {
                for (int i = 0; i < isotp.Count; i++)
                {
                    var transmission = isotp[i];
                    writer.WriteLine($"Direction: {transmission.DirectionTx}, Sending ID: 0x{transmission.SendingID:X4}, Recieving ID: 0x{transmission.RecievingID:X4}, #Frames: {transmission.Frames.Count}, Bytes: {transmission.Data.Length}, Data: {BitConverter.ToString(transmission.Data)}");
                }
            }

            //  Print the start time
            Console.WriteLine($"Log Start Time: {log.log_start}");
            Console.WriteLine($"Parsed ISOTP saved to: {savePath}");
            Console.ReadLine();
        }

        static void PrintCANFrame(CANFrame frame)
        {
            Console.WriteLine($"Timestamp: {frame.TimestampMicros}, ID: 0x{frame.ID:X4}, Bus: {frame.Bus}, Length: {frame.Length}, Data: {BitConverter.ToString(frame.Data)}");
        }
    }
}
