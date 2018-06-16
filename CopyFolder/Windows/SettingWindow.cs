using CopyFolder.Datas;
using LitJson;
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
    public partial class SettingWindow : Form
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var folderInfo = new FolderInfo();
            folderInfo.Name = textBox3.Text;
            folderInfo.SourceFolder = textBox1.Text;
            folderInfo.TargetFolder = textBox2.Text;

            if (string.IsNullOrEmpty(folderInfo.Name)) {
                MessageBox.Show("请输入名称", "提示");
                return;
            }
            if (string.IsNullOrEmpty(folderInfo.SourceFolder)) {
                MessageBox.Show("选择来源文件夹", "提示");
                return;
            }
            if (string.IsNullOrEmpty(folderInfo.TargetFolder)) {
                MessageBox.Show("选择目标文件夹", "提示");
                return;
            }
            List<FolderInfo> infos = new List<FolderInfo>();
            if (File.Exists(Config.XmlFile)) {
                var txt = File.ReadAllText(Config.XmlFile);
                infos = JsonMapper.ToObject<List<FolderInfo>>(txt);
            }
            infos.Add(folderInfo);

            File.WriteAllText(Config.XmlFile, JsonMapper.ToJson(infos));
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择来源文件夹";
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog()== DialogResult.OK) {
                this.textBox1.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择目标文件夹";
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                this.textBox2.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
