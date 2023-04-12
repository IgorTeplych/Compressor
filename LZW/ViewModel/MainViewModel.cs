using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace LZW.ViewModel
{
    public delegate void UserActionHandler();
    public class MainViewModel : ViewModelBase
    {
        public UserActionHandler Cancel;
        public UserActionHandler Paused;
        public UserActionHandler Continue;
        public MainViewModel()
        {
            StartEnabled = true;
            HeaderText = "LZW";
            CanselCommand = new RelayCommand(() => CanselMethod());
            PauseCommand = new RelayCommand(() => PauseMethod());
            StartCommand = new RelayCommand(() => StartMethod());
        }
        public RelayCommand CanselCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand StartCommand { get; set; }

        string textInfo;
        public string TextInfo
        {
            get => textInfo;
            set
            {
                textInfo = value;
                RaisePropertyChanged(() => TextInfo);
            }
        }
        string headerText;
        public string HeaderText
        {
            get => headerText;
            set
            {
                headerText = value;
                RaisePropertyChanged(() => HeaderText);
            }
        }
        bool _CancelEnabled;
        public bool CancelEnabled
        {
            get => _CancelEnabled;
            set
            {
                _CancelEnabled = value;
                RaisePropertyChanged(() => CancelEnabled);
            }
        }
        bool _StartEnabled;
        public bool StartEnabled
        {
            get => _StartEnabled;
            set
            {
                _StartEnabled = value;
                RaisePropertyChanged(() => StartEnabled);
            }
        }
        void CanselMethod()
        {
            StartEnabled = false;
            CancelEnabled = false;
            Cancel.Invoke();
        }
        void PauseMethod()
        {
            StartEnabled = true;
            Paused.Invoke();
        }
        void StartMethod()
        {
            CancelEnabled = true;
            Continue.Invoke();
        }
    }
}