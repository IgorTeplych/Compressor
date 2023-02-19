using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPath
    {
        void Path(FileInfo input);
        void Path(DirectoryInfo output);
    }
}
