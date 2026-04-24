using Serilog;
using System.Collections.Generic;
using System.IO;
using Test_KONTUR.Models;

namespace Test_KONTUR.Services
{
    public class FileProvider
    {
        private readonly ILogger _logger;

        public FileProvider()
        {
            _logger = Log.ForContext<FileProvider>();
        }

        public void ExportToCsv(List<Packet> packets, string filePath)
        {
            _logger.Information($"Начат экспорт пакетов в CSV файл");
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Packet;Channel 1;Channel 2;Channel 3;Channel 4;Channel 5;Channel 6");

                foreach (var packet in packets)
                {
                    writer.WriteLine($"{packet.PacketNumber};" +
                        $"{packet.AvgChannel1};" +
                        $"{packet.AvgChannel2};" +
                        $"{packet.AvgChannel3};" +
                        $"{packet.AvgChannel4};" +
                        $"{packet.AvgChannel5};" +
                        $"{packet.AvgChannel6}");
                }
            }
            _logger.Information($"Экспорт выполнен");
        }

        public void ExportDataListToCsv(List<DataList> dataList, string filePath)
        {
            _logger.Information($"Начат экспорт записей в CSV файл");
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("IsEnabled1;Value11;Value12;Value13;IsEnabled2;Value21;Value22");

                foreach (var data in dataList)
                {
                    writer.WriteLine($"{data.IsEnabled1};" +
                 $"{data.Value11};" +
                 $"{data.Value12};" +
                 $"{data.Value13};" +
                 $"{data.IsEnabled2};" +
                 $"{data.Value21};" +
                 $"{data.Value22}");
                }
            }
            _logger.Information($"Экспорт выполнен");
        }
    }
}
