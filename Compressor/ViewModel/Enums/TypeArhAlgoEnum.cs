using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compressor.ViewModel.Enums
{
    [Serializable]
    public enum TypeArhAlgoEnum
    {
        [Description("RLE")]
        rle = 0,
        [Description("Huffman")]
        hufmann = 1,
        [Description("LZW")]
        lzw = 2
    }
}
