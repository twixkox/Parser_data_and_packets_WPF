using Serilog;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Test_KONTUR.Services;

namespace Test_KONTUR.ViewModels
{
    public class FirtsTaskViewModel : INotifyPropertyChanged
    {
        private readonly DataParser _parser;
        private readonly FileProvider _fileProvider;
        private readonly ILogger _logger;

        private string _inputFilePath;
        private string _outputFilePath;
        private int _progress;
        private bool _isProcessing;
        private string _statusMessage;

        public FirtsTaskViewModel()
        {
            _parser = new DataParser();

            SelectInputCommand = new RelayCommand(_ => SelectInputFile());
            SelectOutputCommand = new RelayCommand(_ => SelectOutputFile());
            ProcessCommand = new RelayCommand(async _ => await ProcessAsync(), _ => !_isProcessing);
        }

        public string InputFilePath
        {
            get => _inputFilePath;
            set
            {
                _inputFilePath = value;
                OnPropertyChanged();
            }
        }

        public string OutputFilePath
        {
            get => _outputFilePath;
            set
            {
                _outputFilePath = value;
                OnPropertyChanged();
            }
        }

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
                (ProcessCommand as RelayCommand).CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectInputCommand { get; }
        public ICommand SelectOutputCommand { get; }
        public ICommand ProcessCommand { get; }

        private void SelectInputFile()
        {
            var dialogWindow = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выберите файл с данными",
                Filter = "DAT files (*.dat)"
            };

            if (dialogWindow.ShowDialog() == true)
            {
                InputFilePath = dialogWindow.FileName;
            }
        }

        private void SelectOutputFile()
        {
            var dialogWindow = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Сохранить CSV файл",
                Filter = "CSV files (*.csv)"
            };

            if (dialogWindow.ShowDialog() == true)
            {
                InputFilePath = dialogWindow.FileName;
            }
        }

        private async Task ProcessAsync()
        {
            if (string.IsNullOrEmpty(InputFilePath))
            {
                StatusMessage = "Файл не выбран!";
                return;
            }

            if (string.IsNullOrEmpty(OutputFilePath))
            {
                StatusMessage = "Не выбрано место сохранения";
                return;
            }

            IsProcessing = true;
            Progress = 0;
            StatusMessage = "Начало обработки";

            try
            {
                var progress = new Progress<int>(percent =>
                {
                    Progress = percent;
                    StatusMessage = $"Ход выполнения - {percent}%";
                });

                var packets = await Task.Run(() => _parser.ParseFirstFile(InputFilePath, progress));

                await Task.Run(() => _fileProvider.ExportToCsv(packets, OutputFilePath));

                StatusMessage = $"Обработка - {packets.Count} пакетов завершена.";
                Progress = 100;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Произошла ошибка при выполнении";
                MessageBox.Show(ex.Message, "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Progress = 0;
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}
