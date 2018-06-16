using CopyFolder.Datas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CopyFolder.Windows
{
    public partial class CopyLogWindow : Form
    {
        private FolderInfo _folderInfo;
        private List<CopyFileInfo> _sourceFileInfos;
        private Timer _timer;

        public CopyLogWindow()
        {
            InitializeComponent();
        }

        public void SetCopyInfo(FolderInfo folderInfo, List<CopyFileInfo> copyFileInfos)
        {
            this.button1.Enabled = false;
            _folderInfo = folderInfo;
            _sourceFileInfos = copyFileInfos.OrderByDescending(q => q.IsFile).ThenByDescending(q => q.FilePath).ToList();
        }

        private void CopyLogWindow_Load(object sender, EventArgs e)
        {
            _timer = new Timer();
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            int Success = 0;
            int Error = 0;
            foreach (var file in _sourceFileInfos) {
                var source = Path.Combine(_folderInfo.SourceFolder, file.FilePath);
                var target = Path.Combine(_folderInfo.TargetFolder, file.FilePath);
                try {
                    if (file.IsFile) {
                        if (file.FileStatus == 0) {
                            File.Delete(target);
                            WriteText($"成功删除文件[{file.FilePath}]");
                        } else {
                            Directory.CreateDirectory(Path.GetDirectoryName(target));
                            File.Copy(source, target, true);
                            WriteText($"成功复制文件[{file.FilePath}]");
                        }
                    } else {
                        if (file.FileStatus == 0) {
                            if (Directory.Exists(target)) {
                                Directory.Delete(target);
                                WriteText($"成功删除文件夹[{file.FilePath}]");
                            } else {
                                continue;
                            }
                        } else {
                            if (Directory.Exists(target) == false) {
                                Directory.CreateDirectory(target);
                                WriteText($"成功创建文件夹[{file.FilePath}]");
                            } else {
                                continue;
                            }
                        }
                    }
                    label2.Text = $"成功 {++Success} ，失败 {Error} 。双击复制文字。";
                } catch (Exception) {
                    if (file.FileStatus == 0) {
                        WriteText($"无法删除[{target}]");
                        WriteLog($"无法删除 [{target}]");
                    } else {
                        WriteText($"无法复制 [{source}] 到 [{target}]");
                        WriteLog($"无法复制 [{source}] 到 [{target}]");
                    }
                    label2.Text = $"成功 {Success} ，失败 {++Error} 。双击复制文字。";
                }
            }
            this.button1.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WriteText(string msg)
        {
            Application.DoEvents();
            listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg);
            Application.DoEvents();
        }
        private void WriteLog(string msg)
        {
            var path = "CopyFolder_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            File.AppendAllText(path, "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex>=0) {
                Clipboard.SetText(listBox1.SelectedItem.ToString());
            }
        }
    }
}
