using BasicStructures;
using Comp;
using Interfaces;
using Interfaces.Enums;
using RLE.View;
using System;
using System.IO;
using System.Windows.Controls;

namespace RLE
{
    public class AlgoritmRLE : ICommand, IShow, IOperation, PathFiles
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
            string outputFile = output.FullName;
            string name = input.Name;
            string ext = input.Extension;
            name = name.Replace(ext, "");

            byte[] bites = File.ReadAllBytes(input.FullName);

            Rle rle = new Rle();
            rle.Encode(bites, output.FullName + name + ".crle");

            byte[] encBites = File.ReadAllBytes(@"N:\tst.crle");

            rle.Decode(encBites, @"N:\decodeFile.docx");
        }
        public void Path(FileInfo input, DirectoryInfo output)
        {
            this.input = input;
            this.output = output;
        }


       
    }
}
