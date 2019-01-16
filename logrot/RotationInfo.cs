using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace logrot
{
    public class RotationInfo
    {
        public string Directory;
        public string Extension;
        public string File;
        public string FullPath;
        public string Name;

        public override string ToString() => FullPath;
    }
}