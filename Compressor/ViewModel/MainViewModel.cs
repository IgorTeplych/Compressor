using Compressor.ViewModel.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interfaces;
using Microsoft.Win32;
using RLE;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Compressor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            CommandsSet = new ObservableCollection<IShow>();
            OpenFilesCommand = new RelayCommand(() => OpenFilesMethod());
            OpenDirectoryCommand = new RelayCommand(() => OpenDirectoryMethod());
            StartCommand = new RelayCommand(() => StartMethod());
            FilesListText = "   ...";
            DirectoryText = "   ...";
        }
        string filesListText;
        public string FilesListText
        {
            get { return filesListText; }
            set
            {
                filesListText = value;
                RaisePropertyChanged(() => FilesListText);
            }
        }
        string directoryText;
        public string DirectoryText
        {
            get { return directoryText; }
            set
            {
                directoryText = value;
                RaisePropertyChanged(() => DirectoryText);
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

        public RelayCommand StartCommand { get; set; }
        public RelayCommand OpenFilesCommand { get; set; }
        public RelayCommand OpenDirectoryCommand { get; set; }
        public ObservableCollection<IShow> CommandsSet { get; set; }

        string[] selectFiles;
        void OpenFilesMethod()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = @"C:\";
            if (openFileDialog.ShowDialog() == true)
            {
                selectFiles = openFileDialog.FileNames;
                FilesListText = new FileInfo(selectFiles[0]).DirectoryName;
                foreach (var item in selectFiles)
                {
                    IShow algo = null;
                    if (SelectedArhAlgo == TypeArhAlgoEnum.rle)
                        algo = new AlgoritmRLE();

                    ((IPath)algo).Path(new FileInfo(item));
                    CommandsSet.Add(algo);
                }
            }
        }
        DirectoryInfo selectDirectory;
        void OpenDirectoryMethod()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                selectDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                DirectoryText = selectDirectory.FullName;

                foreach (var item in CommandsSet.ToList())
                {
                    ((IPath)item).Path(selectDirectory);
                }
            }
        }
        void StartMethod()
        {
            foreach(var item in CommandsSet)
            {
                ((ICommand)item).Execute();
            }
        }
    }
}