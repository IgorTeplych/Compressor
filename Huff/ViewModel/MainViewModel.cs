using GalaSoft.MvvmLight;

namespace Huff.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            HeaderText = "Huffman";
        }

        string _ExtensionText;
        public string ExtensionText
        {
            get => _ExtensionText;
            set
            {
                _ExtensionText = value;
                RaisePropertyChanged(() => ExtensionText);
            }
        }

        string _HeaderText;
        public string HeaderText
        {
            get => _HeaderText;
            set
            {
                _HeaderText = value;
                RaisePropertyChanged(() => HeaderText);
            }
        }
        string _TextInfo;
        public string TextInfo
        {
            get => _TextInfo;
            set
            {
                _TextInfo = value;
                RaisePropertyChanged(() => TextInfo);
            }
        }
    }
}