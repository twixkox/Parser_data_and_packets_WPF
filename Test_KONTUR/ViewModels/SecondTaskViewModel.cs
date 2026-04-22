using Test_KONTUR.Services;

namespace Test_KONTUR.ViewModels
{
    public class SecondTaskViewModel
    {
        private readonly DataParser _parser;

        private string _inputFilePath;
        private string _outputFilePath;
        private int _progress;
        private bool _isProcessing;
        private string _statusMessage;

        public SecondTaskViewModel()
        {
            _parser = new DataParser();

            SelectInputCommand = new RelayCommand(_ => SelectInputFile());
            SelectOutputCommand = new RelayCommand(_ => SelectOutputFile());
            ProcessCommand = new RelayCommand(async _ => await ProcessAsync(), _ => _isProcessing);
        }
    }
}
