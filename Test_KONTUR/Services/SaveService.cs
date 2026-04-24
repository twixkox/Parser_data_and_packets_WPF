using Serilog;
using System;
using System.IO;
using System.Text.Json;
namespace Test_KONTUR.Services
{
    public class SaveService
    {
        private readonly string SettingsFile = Path.Combine("appsettings.json");
        private readonly ILogger _logger;

        public SaveService()
        {
            _logger = Log.ForContext<SaveService>();
        }

        public PathSettings Load()
        {
            if (File.Exists(SettingsFile))
            {
                _logger.Information($"Загрузка путей к файлам");
                var json = File.ReadAllText(SettingsFile);

                var path = JsonSerializer.Deserialize<PathSettings>(json);

                return path;
            }
            _logger.Information($"Невозможно получить пути файлов");
            return new PathSettings();
        }

        public void Save(PathSettings settings)
        {
            _logger.Information($"Сохранение путей к файлам");
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(SettingsFile, json);
            _logger.Information($"Сохранение успешно");
        }
    }
}
