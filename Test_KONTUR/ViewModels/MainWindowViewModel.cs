using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Test_KONTUR.Services;


namespace Test_KONTUR.ViewModels
{
    public class MainWindowViewModel : IDisposable
    {
        private readonly SaveService _settingsService;

        public FirstTaskViewModel Task1VM { get; }
        public SecondTaskViewModel Task2VM { get; }
        public ICommand OpenLogFileCommand { get; }

        public MainWindowViewModel()
        {
            _settingsService = new SaveService();
            Task1VM = new FirstTaskViewModel();
            Task2VM = new SecondTaskViewModel();

            OpenLogFileCommand = new RelayCommand(_ => OpenLogFile());

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
            var settings = new PathSettings
            {
                Window1InputFile = Task1VM.InputFilePath,
                Window1OutputFile = Task1VM.OutputFilePath,
                Window2InputFile = Task2VM.InputFilePath,
                Window2OutputFile = Task2VM.OutputFilePath
            };
            _settingsService.Save(settings);
        }

        private void OpenLogFile()
        {
            try
            {
                string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "log.json");
                if (File.Exists(logFile))
                {
                    Process.Start("notepad.exe", logFile);
                }
                else
                {
                    MessageBox.Show("Файл лога не найден", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public void Dispose()
        {
            SaveSettings();
        }
    }
}