using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_KONTUR.Models;

namespace Test_KONTUR.Services
{
    public class DataParser
    {
        private const int PacketSize = 1456;
        private const int HeaderSize = 16;
        private const int BlockSize = 24;
        private const int BlockPerPacket = 60;

        public List<Packet> ParseFirstFile(string filePath, IProgress<int> progress)
        {
            var packets = new List<Packet>();

            byte[] fileData = File.ReadAllBytes(filePath);
            int totalPackets = fileData.Length / PacketSize;

            int currentPacket = 0;

            for (int offset = 0; offset + PacketSize <= fileData.Length; offset += PacketSize)
            {
                var packet = new Packet();

                packet.Size = BitConverter.ToUInt32(fileData, offset); // 0-3
                packet.Type = BitConverter.ToUInt32(fileData, offset + 4); // 4-7
                packet.PacketNumber = BitConverter.ToUInt32(fileData, offset + 8); // 8-11
                packet.TimeMs = BitConverter.ToUInt32(fileData, offset + 12); // 12-15

                int dataOffset = offset + HeaderSize;

                for (int currentBlock = 0; currentBlock < BlockPerPacket; currentBlock++)
                {
                    int startBlock = dataOffset + currentBlock * BlockSize;

                    var dataBlock = new DataBlock();
                    dataBlock.Channel1 = BitConverter.ToInt32(fileData, startBlock);
                    dataBlock.Channel2 = BitConverter.ToInt32(fileData, startBlock + 4);
                    dataBlock.Channel3 = BitConverter.ToInt32(fileData, startBlock + 8);
                    dataBlock.Channel4 = BitConverter.ToInt32(fileData, startBlock + 12);
                    dataBlock.Channel5 = BitConverter.ToInt32(fileData, startBlock + 16);
                    dataBlock.Channel6 = BitConverter.ToInt32(fileData, startBlock + 20);

                    packet.DataBlocks.Add(dataBlock);
                }
                packets.Add(packet);

                currentPacket++;
                progress.Report((currentPacket * 100) / totalPackets);
            }
            return packets;
        }
    }
}
