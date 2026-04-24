using Serilog;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Test_KONTUR.Models;
using Test_KONTUR.Services;

namespace Test_KONTUR.ViewModels
{
    public class SecondTaskViewModel : INotifyPropertyChanged
    {
        private readonly DataParser _parser;
        private readonly FileProvider _fileProvider;
        private readonly ILogger _logger;

        private string _inputFilePath;
        private string _outputFilePath;
        private int _progress;
        private bool _isProcessing;
        private string _statusMessage;

        public SecondTaskViewModel()
        {
            _logger = Log.ForContext<SecondTaskViewModel>();
            _parser = new DataParser();
            _fileProvider = new FileProvider();
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
                OnPropertyChanged(nameof(IsNotProcessing));
                (ProcessCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        public bool IsNotProcessing => !IsProcessing;

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
            var window = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выберите файл",
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*"
            };

            if (window.ShowDialog() == true)
            {
                InputFilePath = window.FileName;
                _logger.Information($"Выбран файл для обработки {window.FileName}");
            }
        }

        private void SelectOutputFile()
        {
            var window = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Сохранить CSV файл",
                Filter = "CSV files (*.csv)|*.csv",
                DefaultExt = ".csv"
            };

            if (window.ShowDialog() == true)
            {
                OutputFilePath = window.FileName;
                _logger.Information($"Выбран файл для сохранения {window.FileName}");
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
            _logger.Information($"Начало обработки");

            try
            {
                var progress = new Progress<int>(percent =>
                {
                    Progress = percent;
                    StatusMessage = $"Ход выполнения - {percent}%";
                });

                var dataList = await Task.Run(() => _parser.ParseSecondFile(InputFilePath, progress));

                await Task.Run(() => _fileProvider.ExportDataListToCsv(dataList, OutputFilePath));

                StatusMessage = $"Обработка - {dataList.Count} записей завершена.";
                _logger.Information($"Обработка файла завершена. Обработано {dataList.Count} записей");
                Progress = 100;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Произошла ошибка при выполнении";
                MessageBox.Show(ex.Message, "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error($"Произошла ошибка при выполнении обработки {ex.Message}. SecondTaskViewModel");
                Progress = 0;
            }
            finally
            {
                IsProcessing = false;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
