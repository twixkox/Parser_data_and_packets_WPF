using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_KONTUR.Models;

namespace Test_KONTUR.Services
{
    public class FileProvider
    {
        public void ExportToCsv(List<Packet> packets, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Packet,Channel 1,Channel 2,Channel 3,Channel 4,Channel 5,Channel 6");

                foreach (var packet in packets)
                {
                    writer.WriteLine($"{packet.PacketNumber}," +
                        $"{packet.AvgChannel1}," +
                        $"{packet.AvgChannel2}," +
                        $"{packet.AvgChannel3}," +
                        $"{packet.AvgChannel4}," +
                        $"{packet.AvgChannel5}," +
                        $"{packet.AvgChannel6}");
                }
            }
        }
    }
}
