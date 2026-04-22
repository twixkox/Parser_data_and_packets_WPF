using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_KONTUR.Models
{
    public class Packet
    {
        public uint Size { get; set; }
        public uint Type { get; set; }
        public uint PacketNumber { get; set; }
        public uint TimeMs { get; set; }
        public List<DataBlock> DataBlocks { get; set; } = new List<DataBlock>();

        public double AvgChannel1 => DataBlocks.Average(b => b.Channel1);
        public double AvgChannel2 => DataBlocks.Average(b => b.Channel2);
        public double AvgChannel3 => DataBlocks.Average(b => b.Channel3);
        public double AvgChannel4 => DataBlocks.Average(b => b.Channel4);
        public double AvgChannel5 => DataBlocks.Average(b => b.Channel5);
        public double AvgChannel6 => DataBlocks.Average(b => b.Channel6);
    }
}
