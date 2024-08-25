using CANalysis.Models;

/*
 *  Nick Daria 2024
 * 
 *  Base class for all parsers. The goal is for a tool to be able to enumerate through all parser options and determine a log format.
 */

namespace CANalysis.Parsing
{
    public abstract class ParserBase
    {
        public abstract bool ExtensionMatchesFormat(string fileName);
        public abstract bool FileMatchesFormat(Stream fileStream);
        public abstract CANLog Parse(Stream fileStream);
        public abstract bool Encode(Stream fileStream, CANLog log);
    }
}
