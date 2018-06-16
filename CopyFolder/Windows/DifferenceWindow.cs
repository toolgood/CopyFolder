using CopyFolder.Datas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CopyFolder.Windows
{
    public partial class DifferenceWindow : Form
    {
        private FolderInfo _folderInfo;
        private List<CopyFileInfo> _sourceFileInfos;
        private Timer _timer;


        public DifferenceWindow()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        public void SetFolderInfo(FolderInfo folderInfo)
        {
            _folderInfo = folderInfo;

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            var sourceFileInfos = new List<CopyFileInfo>();
            GetCopyFileInfo(sourceFileInfos, _folderInfo.SourceFolder, _folderInfo.SourceFolder, 2, 0);
            var targetFileInfos = new List<CopyFileInfo>();
            GetCopyFileInfo(targetFileInfos, _folderInfo.TargetFolder, _folderInfo.TargetFolder, 0, 0);

            _sourceFileInfos = SetFileStatus(sourceFileInfos, targetFileInfos);
            sourceFileInfos = null;
            targetFileInfos = null;

            List<CopyFileInfo> infos = new List<CopyFileInfo>();
            foreach (var item in _sourceFileInfos) {
                infos.Add(item);
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = infos;
        }

        private void GetCopyFileInfo(List<CopyFileInfo> infos, string rootFolder, string folder, int type, int count)
        {
            var files = Directory.GetFiles(folder);
            foreach (var file in files) {
                var fi = new FileInfo(file);
                if (fi.Attributes.HasFlag(FileAttributes.Hidden) == false) {
                    infos.Add(new CopyFileInfo() {
                        FilePath = file.Substring(rootFolder.Length).TrimStart('\\'),
                        FileSize = fi.Length,
                        FileLastWriteTime = fi.LastWriteTime,
                        FileStatus = type,
                        IsFile = true,
                        Count = count
                    });
                }
            }
            var folders = Directory.GetDirectories(folder);
            foreach (var item in folders) {
                var di = new DirectoryInfo(item);
                if (di.Attributes.HasFlag(FileAttributes.Hidden) == false) {
                    infos.Add(new CopyFileInfo() {
                        FilePath = item.Substring(rootFolder.Length).TrimStart('\\').TrimEnd('\\') + "\\",
                        FileSize = 0,
                        FileLastWriteTime = di.LastWriteTime,
                        FileStatus = type,
                        IsFile = false,
                        Count = 1
                    });
                    GetCopyFileInfo(infos, rootFolder, item, type, 1);
                }
            }
        }
        private List<CopyFileInfo> SetFileStatus(List<CopyFileInfo> sourceFileInfos, List<CopyFileInfo> targetFileInfos)
        {
            Dictionary<string, CopyFileInfo> dictionary = new Dictionary<string, CopyFileInfo>();
            foreach (var item in targetFileInfos) {
                dictionary.Add(item.FilePath, item);
            }
            foreach (var fileInfo in sourceFileInfos) {
                if (dictionary.TryGetValue(fileInfo.FilePath, out CopyFileInfo targetFileInfo)) {
                    if (targetFileInfo.IsFile == false) {
                        dictionary.Remove(fileInfo.FilePath);
                    } else if (targetFileInfo.FileLastWriteTime == fileInfo.FileLastWriteTime && targetFileInfo.FileSize == fileInfo.FileSize) {
                        dictionary.Remove(fileInfo.FilePath);
                    } else {
                        targetFileInfo.CheckBox = true;
                        targetFileInfo.FileLastWriteTime = fileInfo.FileLastWriteTime;
                        targetFileInfo.FileSize = fileInfo.FileSize;
                        targetFileInfo.FileStatus = 1;
                    }
                } else {
                    fileInfo.CheckBox = true;
                    dictionary.Add(fileInfo.FilePath, fileInfo);
                }
            }
            return dictionary.Select(q => q.Value).OrderBy(q => q.Count).ThenBy(q => q.FilePath).ToList();
        }


        private void DifferenceWindow_Load(object sender, EventArgs e)
        {
            _timer = new Timer();
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows) {
                var cell = item.Cells[0] as DataGridViewCheckBoxCell;
                cell.Value = true;
            }
            dataGridView1.Refresh();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows) {
                var cell = item.Cells[0] as DataGridViewCheckBoxCell;
                cell.Value = false;
            }
            dataGridView1.Refresh();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //dataGridView1.Rows.Clear();
            List<CopyFileInfo> infos = new List<CopyFileInfo>();
            foreach (var item in _sourceFileInfos) {
                if (item.FileStatus == 2) {
                    infos.Add(item);
                }
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = infos;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //dataGridView1.Rows.Clear();
            List<CopyFileInfo> infos = new List<CopyFileInfo>();
            foreach (var item in _sourceFileInfos) {
                if (item.FileStatus == 1) {
                    infos.Add(item);
                }
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = infos;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //dataGridView1.Rows.Clear();
            List<CopyFileInfo> infos = new List<CopyFileInfo>();
            foreach (var item in _sourceFileInfos) {
                if (item.FileStatus == 0) {
                    infos.Add(item);
                }
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = infos;
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //dataGridView1.Rows.Clear();
            List<CopyFileInfo> infos = new List<CopyFileInfo>();
            foreach (var item in _sourceFileInfos) {
                infos.Add(item);
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = infos;
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<CopyFileInfo> infos = new List<CopyFileInfo>();
            foreach (var item in _sourceFileInfos) {
                if (item.FileStatus != 0) {
                    infos.Add(item);
                }
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = infos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            List<CopyFileInfo> infos = new List<CopyFileInfo>();
            foreach (DataGridViewRow item in dataGridView1.Rows) {
                var cell = item.Cells[0] as DataGridViewCheckBoxCell;
                if ((bool)cell.Value) {
                    var info = item.DataBoundItem as CopyFileInfo;
                    infos.Add(info);
                }
            }
            CopyLogWindow window = new CopyLogWindow();
            window.SetCopyInfo(_folderInfo, infos);
            window.ShowDialog();
            this.Close();
        }


    }
}
