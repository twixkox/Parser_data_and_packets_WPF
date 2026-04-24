using System;
using System.IO;
using System.Text.Json;
namespace Test_KONTUR.Services
{
    public class SaveService
    {
        private readonly string SettingsFile = Path.Combine("appsettings.json");

        public AppSettings Load()
        {
            if (File.Exists(SettingsFile))
            {
                var json = File.ReadAllText(SettingsFile);

                var path = JsonSerializer.Deserialize<AppSettings>(json);

                return path;
            }
            return new AppSettings();
        }

        public void Save(AppSettings settings)
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(SettingsFile, json);
        }
    }
}
