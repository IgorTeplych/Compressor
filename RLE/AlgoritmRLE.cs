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
using System.Windows.Threading;

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
            try
            {
                string outputFile = output.FullName;
                string name = input.Name;
                byte[] bytes = File.ReadAllBytes(input.FullName);
                Rle rle = new Rle(CalcProgressAndRatio);
                rle.Decode(bytes, output.FullName + name.Replace(".crle", ""));
            }
            catch (Exception ex)
            {

            }
        }
        void Encode()
        {
            try
            {
                string outputFile = output.FullName;
                string name = input.Name;
                byte[] bytes = File.ReadAllBytes(input.FullName);
                Rle rle = new Rle(CalcProgressAndRatio);
                rle.Encode(bytes, output.FullName + @"/" + name + ".crle");
            }
            catch (Exception ex)
            {

            }
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

        Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        void CalcProgressAndRatio(int progress, int ratio)
        {
            dispatcher.Invoke(new Action(() =>
            {
                miniature.miniatureViewModel.RateValueText = ratio + "%";
                miniature.miniatureViewModel.Rate = ratio;

                miniature.miniatureViewModel.Progress = progress;
                miniature.miniatureViewModel.ProgressValueText = progress + "%";
            }));
        }

        public StateEnum GetState()
        {
            return state;
        }
    }
}
