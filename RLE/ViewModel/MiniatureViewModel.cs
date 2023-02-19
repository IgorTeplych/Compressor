using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RLE.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLE.ViewModel
{
    public class MiniatureViewModel : ViewModelBase
    {
        public MiniatureViewModel()
        {
            ButtonCommand = new RelayCommand(() => ButtonMethod());

            NameProgress = "Прогресс";
            NameRatio = "Сжатие";
        }

        public RelayCommand ButtonCommand { get; set; }

        public string NameRatio { get; set; }
        public string NameProgress { get; set; }

        string progressValueText;
        public string ProgressValueText
        {
            get { return progressValueText; }
            set
            {
                progressValueText = value;
                RaisePropertyChanged(() => ProgressValueText);
            }
        }
        string rateValueText;
        public string RateValueText
        {
            get { return rateValueText; }
            set
            {
                rateValueText = value;
                RaisePropertyChanged(() => RateValueText);
            }
        }
        int rate;
        public int Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                RaisePropertyChanged(() => Rate);
            }
        }
        int progress;
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                RaisePropertyChanged(() => Progress);
            }
        }
        void ButtonMethod()
        {
            ProgressValueText = "Button was push";
        }
    }
}
