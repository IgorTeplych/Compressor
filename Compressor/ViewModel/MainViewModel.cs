using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interfaces;
using RLE;
using System.Collections.ObjectModel;

namespace Compressor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            CommandsSet = new ObservableCollection<IShow>();
            AddCommand = new RelayCommand(() => AddMethod());
        }

        public RelayCommand AddCommand { get; set; }
        public ObservableCollection<IShow> CommandsSet { get; set; }
        void AddMethod()
        {
            IShow algoritm = new AlgoritmRLE();
            CommandsSet.Add(algoritm);

            string path = @"N:\tst.docx";
            string outputDir = @"N:\";

            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(outputDir);


           ((PathFiles)algoritm).Path(fi, di);

            ((ICommand)algoritm).Execute();
        }
    }
}