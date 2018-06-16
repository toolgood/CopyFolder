using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CopyFolder.Datas
{
    public class FolderInfo
    {
        public string Name { get; set; }

        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }

        public override string ToString()
        {
            return $"【{Name}】{SourceFolder} -> {TargetFolder}";
        }
    }
}
