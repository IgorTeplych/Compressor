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
            InfoText = "Info";
            ButtonCommand = new RelayCommand(()=> ButtonMethod());
        }

        public RelayCommand ButtonCommand { get; set; }

        string infoText;
        public string InfoText 
        {
            get { return infoText; }
            set
            {
                infoText = value;
                RaisePropertyChanged(() => InfoText);
            }
        }

        void ButtonMethod()
        {
            InfoText = "Button was push";
        }
    }
}
