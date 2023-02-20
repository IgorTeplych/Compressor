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
    public class AlgoritmRLE : ICommand, IShow, IOperation, IPath, IState
    {
        StateEnum state;
        OperationEnum operation;
        FileInfo input;
        DirectoryInfo output;
        public AlgoritmRLE()
        {
            miniature = new Miniature();
            state = StateEnum.WaitingRun;
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
            if (state != StateEnum.WaitingRun)
                return;

            if (output == null)
            {
                return;
            }
            if (input == null)
            {
                return;
            }

            state = StateEnum.IsRun;

            Task task = new Task(() =>
            {
                if (operation == OperationEnum.Encode)
                    Encode();

                if (operation == OperationEnum.Decode)
                    Decode();

                state = StateEnum.Complited;
            });
            task.Start();
        }

        void Decode()
        {
            string outputFile = output.FullName;
            string name = input.Name;
            byte[] bites = File.ReadAllBytes(input.FullName);
            Rle rle = new Rle(Progress);
            rle.Decode(bites, output.FullName + name.Replace(".crle", ""));
        }
        void Encode()
        {
            string outputFile = output.FullName;
            string name = input.Name;
            byte[] bites = File.ReadAllBytes(input.FullName);
            Rle rle = new Rle(Progress);
            rle.Encode(bites, output.FullName + name + ".crle");
        }
        public void Path(FileInfo input)
        {
            this.input = input;
            miniature.miniatureViewModel.ExtensionText = input.Extension;
        }
        public void Path(DirectoryInfo output)
        {
            this.output = output;
        }
        void Progress(int count, int lenght, int totalLength)
        {
            if (count == 0)
                return;
            if (totalLength == 0)
                return;

            double rate = 100 * (double)Math.Round((double)lenght / (double)count, 3);
            miniature.miniatureViewModel.RateValueText = rate.ToString() + "%";
            miniature.miniatureViewModel.Rate = (int)rate;

            int progress = (count * 100) / totalLength;
            miniature.miniatureViewModel.Progress = progress;
            miniature.miniatureViewModel.ProgressValueText = progress.ToString() + "%";
        }

        public StateEnum GetState()
        {
            return state;
        }
    }
}
