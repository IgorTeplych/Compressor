using Compressor.ViewModel.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Huff;
using Interfaces;
using LZW;
using Microsoft.Win32;
using RLE;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Compressor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            CommandsSet = new ObservableCollection<IShow>();
            StartCommand = new RelayCommand(() => StartMethod());
            ClearCommand = new RelayCommand(() => ClearMethod());
            DragEnterFilesCommand = new RelayCommand(() => DragEnterFilesMethod());
            DragLeaveFilesCommand = new RelayCommand(() => DragLeaveFilesMethod());
            DropFilesCommand = new RelayCommand<object>((obj) => DropFilesMethod(obj));
        }

        string _ColorByDragFiles;
        public string ColorByDragFiles
        {
            get { return _ColorByDragFiles; }
            set
            {
                _ColorByDragFiles = value;
                RaisePropertyChanged(() => ColorByDragFiles);
            }
        }

        TypeArhAlgoEnum selectedArhAlgo;
        public TypeArhAlgoEnum SelectedArhAlgo
        {
            get { return selectedArhAlgo; }
            set
            {
                selectedArhAlgo = value;
                RaisePropertyChanged(() => SelectedArhAlgo);
            }
        }

        OperationsEnum selectedAction;
        public OperationsEnum SelectedAction
        {
            get { return selectedAction; }
            set
            {
                selectedAction = value;
                RaisePropertyChanged(() => SelectedAction);
            }
        }
        public RelayCommand<object> DropFilesCommand { get; set; }
        public RelayCommand StartCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand DragEnterFilesCommand { get; set; }
        public RelayCommand DragLeaveFilesCommand { get; set; }
        public ObservableCollection<IShow> CommandsSet { get; set; }

        void StartMethod()
        {
            foreach (var item in CommandsSet)
                ((ICommand)item).Execute();
        }
        void ClearMethod()
        {
            var toClear = CommandsSet.Where((p => ((IState)p).GetState() != Interfaces.Enums.StateEnum.IsRun)).ToList();
            foreach (var item in toClear)
            {
                CommandsSet.Remove(item);
            }
        }
        void DragEnterFilesMethod()
        {
            ColorByDragFiles = Brushes.Gray.ToString();
        }
        void DragLeaveFilesMethod()
        {
            ColorByDragFiles = Brushes.LightGray.ToString();
        }
        void DropFilesMethod(object obj)
        {
            var e = obj as System.Windows.DragEventArgs;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var item in files)
                {
                    var fi = new FileInfo(item);
                    if (!fi.Exists)
                        return;

                    IShow algo = null;
                    if (SelectedArhAlgo == TypeArhAlgoEnum.rle)
                        algo = new AlgoritmRLE();

                    if (SelectedArhAlgo == TypeArhAlgoEnum.hufmann)
                        algo = new HuffmanAlgo();

                    if (SelectedArhAlgo == TypeArhAlgoEnum.lzw)
                        algo = new AlgoritmLZW();

                    if (SelectedAction == OperationsEnum.compression)
                        ((IOperation)algo).Operation(Interfaces.Enums.OperationEnum.Encode);

                    if (SelectedAction == OperationsEnum.unpacking)
                        ((IOperation)algo).Operation(Interfaces.Enums.OperationEnum.Decode);

                    ((IPath)algo).Path(fi);
                    ((IPath)algo).Path(fi.Directory);

                    CommandsSet.Add(algo);
                }
            }
            ColorByDragFiles = Brushes.LightGray.ToString();
        }
    }
}