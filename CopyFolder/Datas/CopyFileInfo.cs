using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CopyFolder.Datas
{
    public class CopyFileInfo
    {
        public bool CheckBox { get; set; }

        public string FilePath { get; set; }

        public int Count { get; set; }

        public long FileSize { get; set; }

        public bool IsFile { get; set; }

        public string FileType
        {
            get {
                return IsFile ? "文件" : "文件夹";
            }
        }


        public string GetFileSize
        {
            get {
                string m_strSize = "";
                if (FileSize < 1024.00)
                    m_strSize = FileSize.ToString("F2") + " B";
                else if (FileSize >= 1024.00 && FileSize < 1048576)
                    m_strSize = (FileSize / 1024.00).ToString("F2") + " K";
                else if (FileSize >= 1048576 && FileSize < 1073741824)
                    m_strSize = (FileSize / 1024.00 / 1024.00).ToString("F2") + " M";
                else if (FileSize >= 1073741824)
                    m_strSize = (FileSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
                return m_strSize;
            }
        }


        public DateTime FileLastWriteTime { get; set; }

        /// <summary>
        /// 0缺少，1修改，2新增
        /// </summary>
        public int FileStatus { get; set; }
        
        public string GetFileStatus
        {
            get {
                if (FileStatus==0) {
                    return "删除";
                }
                if (FileStatus == 1) {
                    return "修改";
                }
                return "新增";
            }
        }
    }
}
