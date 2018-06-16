using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CopyFolder.Datas;
using LitJson;

namespace CopyFolder.Windows
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            RefreshDatas();
        }
        private void RefreshDatas()
        {
            if (File.Exists(Config.XmlFile)) {
                var txt = File.ReadAllText(Config.XmlFile);
                var infos = JsonMapper.ToObject<List<FolderInfo>>(txt);

                listBox1.Items.Clear();
                foreach (var item in infos) {
                    listBox1.Items.Add(item);
                }
                if (infos.Count > 0) {
                    listBox1.SelectedIndex = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var index = this.listBox1.SelectedIndex;
            if (index >= 0) {
                var info = (FolderInfo)this.listBox1.SelectedItem;
                this.Hide();
                DifferenceWindow window = new DifferenceWindow();
                window.SetFolderInfo(info);
                window.ShowDialog();
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SettingWindow window = new SettingWindow();
            window.ShowDialog();
            RefreshDatas();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var index = this.listBox1.SelectedIndex;
            if (index >= 0) {
                this.listBox1.Items.RemoveAt(index);

                List<FolderInfo> infos = new List<FolderInfo>();
                foreach (FolderInfo item in this.listBox1.Items) {
                    infos.Add(item);
                }

                File.WriteAllText(Config.XmlFile, JsonMapper.ToJson(infos));
            }

            RefreshDatas();
        }


    }
}
