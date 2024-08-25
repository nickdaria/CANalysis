using System;
using System.IO;
using System.Linq;
using CANalysis.Models;

/*
 *  Nick Daria 2024
 * 
 *  Implementation for the format I personally use, SavvyCAN GVRET CSV.
 * 
 *  Very simple, but it seems as though it doesn't support CAN-FD frames or RTR flags. 
 *  I don't play with CAN-FD and nobody uses RTR so I never noticed until I wrote this.
 */

namespace CANalysis.Parsing
{
    public class GVRET_CSV : ParserBase
    {
        private static readonly string[] header = 
        [
            "Time Stamp",
            "ID",
            "Extended",
            "Dir",
            "Bus",
            "LEN",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8"
        ];

        public override bool ExtensionMatchesFormat(string fileName)
        {
            string? ret = Path.GetExtension(fileName);
            return !string.IsNullOrWhiteSpace(ret) && ret == ".csv";
        }

        public override bool FileMatchesFormat(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream, leaveOpen: true);
            fileStream.Seek(0, SeekOrigin.Begin);

            string? headerLine = reader.ReadLine();
            if (headerLine == null) return false;

            var headerColumns = headerLine.Split(',');

            if (headerColumns.Length != header.Length)
                return false;

            for (int i = 0; i < header.Length; i++)
            {
                if (!string.Equals(headerColumns[i], header[i], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public override CANLog Parse(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            fileStream.Seek(0, SeekOrigin.Begin);

            var canLog = new CANLog();
            string? line;
            bool firstLine = true;

            while ((line = reader.ReadLine()) != null)
            {
                //  Skip header
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                var columns = line.Split(',');

                //  Skip empty or invalid lines
                if (columns.Length < 2 || string.IsNullOrWhiteSpace(columns[0]))
                {
                    continue;
                }

                ulong timestampMicros = ParseTimestamp(columns[0]);
                if (canLog.log_start == default)
                {
                    canLog.log_start = DateTimeOffset.FromUnixTimeMilliseconds((long)timestampMicros / 1000).DateTime;
                }

                var frame = new CANFrame
                {
                    TimestampMicros = timestampMicros - ((ulong)((DateTimeOffset)canLog.log_start).ToUnixTimeMilliseconds() * 1000),
                    ID = ParseHexID(columns[1]),
                    Flags = new CANFrame.FrameFlags
                    {
                        Extended = bool.Parse(columns[2]),
                        FlexibleDataRateFrame = false,
                        RemoteRequest = false
                    },
                    Direction = ParseDirection(columns[3]),
                    Bus = ushort.Parse(columns[4]),
                    Length = ushort.Parse(columns[5]),
                    Data = ParseData(columns.Skip(6).Take(8))
                };

                canLog.frames.Add(frame);
            }

            return canLog;
        }

        public override bool Encode(Stream fileStream, CANLog log)
        {
            using var writer = new StreamWriter(fileStream);
            for(ushort i = 0; i < header.Length; i++)
            {
                writer.Write(header[i]);
                if(header.Length - 1 != i)
                {
                    writer.Write(',');
                }
            }
            writer.WriteLine();

            foreach (var frame in log.frames)
            {
                // Convert log_start to DateTimeOffset and then get Unix time in milliseconds
                var baseTimestampMicros = (ulong)(new DateTimeOffset(log.log_start).ToUnixTimeMilliseconds() * 1000);
                var timestamp = baseTimestampMicros + frame.TimestampMicros;
                var line = $"-{timestamp},{frame.ID:X8},{frame.Flags.Extended},{frame.Direction},{frame.Bus},{frame.Length},{FormatData(frame.Data)}";
                writer.WriteLine(line);
            }

            return true;
        }

        private static ulong ParseTimestamp(string timestamp)
        {
            if (!timestamp.StartsWith('-'))
            {
                throw new NotImplementedException("Unit not Unix epoch");
            }

            return ulong.Parse(timestamp.TrimStart('-'));
        }

        private static ushort ParseHexID(string id)
        {
            return ushort.Parse(id, System.Globalization.NumberStyles.HexNumber);
        }

        private static SignalDirection ParseDirection(string direction)
        {
            return direction == "Rx" ? SignalDirection.Rx : SignalDirection.Tx;
        }

        private static byte[] ParseData(IEnumerable<string> dataColumns)
        {
            return dataColumns.Select(d => byte.Parse(d, System.Globalization.NumberStyles.HexNumber)).ToArray();
        }

        private static string FormatData(byte[] data)
        {
            //  Yes, this adds a trailing comma. It's what SavvyCAN does. Idk why.
            return string.Join(",", data.Select(d => d.ToString("X2"))) + ",";
        }
    }
}
