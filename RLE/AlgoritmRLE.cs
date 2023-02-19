using BasicStructures;
using Comp;
using Interfaces;
using Interfaces.Enums;
using RLE.View;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RLE
{
    public class AlgoritmRLE : ICommand, IShow, IOperation, IPath
    {
        OperationEnum operation;
        FileInfo input;
        DirectoryInfo output;
        public AlgoritmRLE()
        {
            miniature = new Miniature();
        }

        Miniature miniature;
        public UserControl Miniature
        {
            get
            {
                return miniature;
            }
        }
        public void Operation(OperationEnum operation)
        {
            this.operation = operation;
        }
        public void Execute()
        {
            if (output == null)
                return;
            if (input == null)
                return;
            Task task = new Task(() =>
            {
                string outputFile = output.FullName;
                string name = input.Name;
                string ext = input.Extension;
                name = name.Replace(ext, "");
                byte[] bites = File.ReadAllBytes(input.FullName);
                Rle rle = new Rle(Progress);
                rle.Encode(bites, output.FullName + name + ".crle");
            });
            task.Start();
        }
        public void Path(FileInfo input)
        {
            this.input = input;
        }
        public void Path(DirectoryInfo output)
        {
            this.output = output;
        }
        void Progress(int count, int lenght, int totalLength)
        {
            double rate = 100 * (double)Math.Round((double)lenght / (double)count, 3);
            miniature.miniatureViewModel.RateValueText = rate.ToString() + "%";
            miniature.miniatureViewModel.Rate = (int)rate;

            int progress = (count * 100) / totalLength;
            miniature.miniatureViewModel.Progress = progress;
            miniature.miniatureViewModel.ProgressValueText = progress.ToString() + "%";
        }

    }
}
