using Compressor.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compressor.ViewModel.Enums
{
    [Serializable]
    public enum OperationsEnum
    {
        [LocDescriptionAttribute(@"Compression", typeof(Lang))]
        compression = 0,
        [LocDescriptionAttribute(@"Unpacking", typeof(Lang))]
        unpacking = 1,
    }
}
