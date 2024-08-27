using CANalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANalysis.Filtering
{
    public class CANFrameFilter
    {
        private readonly List<Func<CANFrame, bool>> _filters = [];

        // Filter by specific CAN IDs
        public CANFrameFilter ByID(Func<UInt32, bool> idPredicate)
        {
            _filters.Add(frame => idPredicate(frame.ID));
            return this;
        }

        public CANFrameFilter ByData(Func<byte[], bool> dataPredicate)
        {
            _filters.Add(frame => dataPredicate(frame.Data));
            return this;
        }

        public CANFrameFilter ByLength(Func<ushort, bool> lengthPredicate)
        {
            _filters.Add(frame => lengthPredicate(frame.Length));
            return this;
        }

        public CANFrameFilter ByTimestamp(Func<ulong, bool> timestampPredicate)
        {
            _filters.Add(frame => timestampPredicate(frame.TimestampMicros));
            return this;
        }

        //  Apply selected filters
        public IEnumerable<CANFrame> Apply(IEnumerable<CANFrame> frames)
        {
            var filtered = frames.Where(frame => _filters.All(f => f(frame)));

            return filtered;
        }

        //  Overload for CANLog
        public IEnumerable<CANFrame> Apply(CANLog log)
        {
            return Apply(log.frames);
        }
    }
}
