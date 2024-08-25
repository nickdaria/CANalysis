using CANalysis.Models;
using CANalysis.Filtering;
using CANalysis.Parsing;
using System;
using System.IO;
using System.Linq;

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
            Console.WriteLine("File Test");

            string path = @"I:\My Drive\Q50 CAN Reverse Engineering\can-logs\q50-ecutek-nodongle\wake-from-sleep-no-acc-or-ignition.csv";
            string savePath = @"I:\My Drive\Q50 CAN Reverse Engineering\can-logs\q50-ecutek-nodongle\wake-from-sleep-no-acc-or-ignition-PARSED.csv";

            //  Parse the file at path
            ParserBase parser = new GVRET_CSV();
            CANLog log;

            using (var fileStream = File.OpenRead(path))
            {
                log = parser.Parse(fileStream);
            }

            //  List all CAN frames in the log
            foreach (var frame in log.frames)
            {
                PrintCANFrame(frame);
            }

            //  Create a filtered log that only includes frames with ID 0x7E0
            var filter = new CANFrameFilter().ByID(id => id == 0x541);
            var filteredFrames = filter.Apply(log.frames).ToList();

            //  List all CAN frames in the filtered log
            Console.WriteLine("\nFiltered CAN Frames (ID == 0x541):");
            foreach (var frame in filteredFrames)
            {
                PrintCANFrame(frame);
            }

            //  Save the unfiltered log to savePath
            if(File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            using (var fileStream = File.Create(savePath))
            {
                parser.Encode(fileStream, log);
            }

            //  Print the start time
            Console.WriteLine($"Log Start Time: {log.log_start}");

            Console.WriteLine($"Unfiltered log saved to: {savePath}");
            Console.ReadLine();
        }

        static void PrintCANFrame(CANFrame frame)
        {
            Console.WriteLine($"Timestamp: {frame.TimestampMicros}, ID: 0x{frame.ID:X4}, Bus: {frame.Bus}, Length: {frame.Length}, Data: {BitConverter.ToString(frame.Data)}");
        }
    }
}
