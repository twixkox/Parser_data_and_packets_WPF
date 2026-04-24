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

        public List<DataList> ParseSecondFile(string inputFilePath, IProgress<int> progress)
        {
            var result = new List<DataList>();

            byte[] fileData = File.ReadAllBytes(inputFilePath);

            int totalItems = fileData.Length / 4;
            int currentItem = 0;

            for(int offset = 0; offset +4 <= fileData.Length; offset += 4)
            {
                uint value = BitConverter.ToUInt32(fileData, offset);

                var data = ParseUint32ToFields(value);
                result.Add(data);

                currentItem++;
                progress.Report((currentItem * 100) / totalItems);
            }
            return result;
        }

        private DataList ParseUint32ToFields(uint value)
        {
            var data = new DataList();

            data.IsEnabled1 = (value & 0x1) == 1;
            data.Value11 = (value >> 1) & 0x7;
            data.Value12 = (value >> 4) & 0x7;
            data.Value13 = (value >> 7) & 0x7;
            data.IsEnabled2 = ((value >> 16) & 0x1) == 1;
            data.Value21 = (value >> 17) & 0x7;
            data.Value22 = (value >> 20) & 0x7;

            return data;
        }
    }
}
