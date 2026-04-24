using System;
using Test_KONTUR.Services;


namespace Test_KONTUR.ViewModels
{
    public class MainWindowViewModel : IDisposable
    {
        private readonly SaveService _settingsService;

        public FirstTaskViewModel Task1VM { get; }
        public SecondTaskViewModel Task2VM { get; }

        public MainWindowViewModel()
        {
            _settingsService = new SaveService();
            Task1VM = new FirstTaskViewModel();
            Task2VM = new SecondTaskViewModel();

            LoadSettings();
        }

        private void LoadSettings()
        {
            var settings = _settingsService.Load();
            Task1VM.InputFilePath = settings.Window1InputFile ?? "";
            Task1VM.OutputFilePath = settings.Window1OutputFile ?? "";
            Task2VM.InputFilePath = settings.Window2InputFile ?? "";
            Task2VM.OutputFilePath = settings.Window2OutputFile ?? "";
        }

        private void SaveSettings()
        {
            var settings = new AppSettings
            {
                Window1InputFile = Task1VM.InputFilePath,
                Window1OutputFile = Task1VM.OutputFilePath,
                Window2InputFile = Task2VM.InputFilePath,
                Window2OutputFile = Task2VM.OutputFilePath
            };
            _settingsService.Save(settings);
        }

        public void Dispose()
        {
            SaveSettings();
        }
    }
}