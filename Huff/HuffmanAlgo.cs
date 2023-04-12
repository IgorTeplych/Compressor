using Huff.View;
using Interfaces;
using Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Huff
{
    public class HuffmanAlgo : ICommand, IShow, IOperation, IPath, IState
    {
        StateEnum state;
        OperationEnum operation;
        FileInfo input;
        DirectoryInfo output;

        public HuffmanAlgo()
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
            Algoritms.Compress compress = new Algoritms.Compress();
            compress.ProgressDecodingMessageHandler = DecodingWriteMessage;
            compress.Decode(input.FullName, output.FullName + @"\" + name.Replace(".huff", ""));
        }
        void Encode()
        {
            string outputFile = output.FullName;
            string name = input.Name;
            Algoritms.Compress compress = new Algoritms.Compress();
            compress.ProgressEncodingMessageHandler = CalcEncodeProgress;
            compress.EncodingMessageHandler = CalcFinalCompressRatio;
            byte[] bytes = File.ReadAllBytes(input.FullName);
            compress.Encode(bytes, outputFile + @"\" + name + ".huff");
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
        void WriteToFile(byte[] bytes, int len, string path)
        {
            using (FileStream fstream = new FileStream(path, FileMode.Append))
            {
                for (int i = 0; i < len; i++)
                    fstream.WriteByte(bytes[i]);
            }
        }

        Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        void CalcEncodeProgress(string message)
        {
            dispatcher.Invoke(new Action(() =>
            {
                miniature.mainViewModel.TextInfo = message;
            }));
        }
        void CalcFinalCompressRatio(string message)
        {
            dispatcher.Invoke(new Action(() =>
            {
                miniature.mainViewModel.TextInfo += message;
            }));
        }

        void DecodingWriteMessage(string message)
        {
            dispatcher.Invoke(new Action(() =>
            {
                miniature.mainViewModel.TextInfo = message;
            }));
        }
    }
}
