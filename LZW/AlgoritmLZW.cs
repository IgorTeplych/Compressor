using Interfaces;
using Interfaces.Enums;
using LZW.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LZW
{
    public class AlgoritmLZW : ICommand, IShow, IOperation, IPath, IState
    {
        StateEnum state;
        OperationEnum operation;
        FileInfo input;
        DirectoryInfo output;
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;
        public AlgoritmLZW()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

            miniature = new Miniature();
            miniature.mainViewModel.Cancel = Cancel;
            miniature.mainViewModel.Paused = Paused;
            miniature.mainViewModel.Continue = Continue;
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

            lzw = new Model.CompressAlgo.LZW();
            state = StateEnum.IsRun;
            miniature.mainViewModel.CancelEnabled = true;

            Task task = new Task(() =>
            {
                if (operation == OperationEnum.Encode)
                    Encode();

                if (operation == OperationEnum.Decode)
                    Decode();

                
                miniature.mainViewModel.TextInfo += Environment.NewLine;

                if (token.IsCancellationRequested)
                {
                    miniature.mainViewModel.TextInfo += "Операция отменена!";
                    state = StateEnum.Canseled;
                }
                else
                {
                    miniature.mainViewModel.TextInfo += "Выполнено!";
                    state = StateEnum.Complited;
                }
                miniature.mainViewModel.CancelEnabled = false;
                miniature.mainViewModel.StartEnabled = false;
            }, token);
            task.Start();
        }

        Model.CompressAlgo.LZW lzw;
        void Encode()
        {
            string outputFile = output.FullName;
            string name = input.Name;
            lzw.EncodeProgressHandler = EncodeProgress;
            lzw.CancelToken = token;
            var bytes = File.ReadAllBytes(input.FullName);
            lzw.Encode(bytes, outputFile + @"\" + name + ".lzw");
        }
        void Decode()
        {
            lzw.DecodeProgressHandler = DecodeProgress;
            lzw.CancelToken = token;
            lzw.Decode(input.FullName, output.FullName + @"\" + input.Name.Replace(".lzw", ""));
        }
        public StateEnum GetState()
        {
            return state;
        }

        public void Operation(OperationEnum whatToDo)
        {
            this.operation = whatToDo;
        }

        public void Path(FileInfo input)
        {
            this.input = input;
        }

        public void Path(DirectoryInfo output)
        {
            this.output = output;
        }

        void EncodeProgress(int progress, int ratio)
        {
            miniature.mainViewModel.TextInfo = "Степень сжатия " + ratio + "%";
            miniature.mainViewModel.TextInfo += Environment.NewLine;
            miniature.mainViewModel.TextInfo += "Прогресс " + progress + "%";
        }
        void DecodeProgress(int countDecodeBytes, int ratio)
        {
            miniature.mainViewModel.TextInfo = "Декодировано байт " + countDecodeBytes;
        }

        void Cancel()
        {
            cancelTokenSource.Cancel();
            state = StateEnum.Canseled;
        }
        void Paused()
        {
            lzw.IsPaused = true;
            state = StateEnum.Paused;
        }
        void Continue()
        {
            Execute();
            lzw.IsPaused = false;
            state = StateEnum.IsRun;
        }
    }
}
