using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SteamSSFN
{
    public partial class Form1 : Form
    {
        private static float version = 1.0f;
        private static string ssfnPath = "./ssfn";
        private static string steamPath;
        private static readonly Guid CLSID_WshShell = new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = this.Text +" Ver-"+version+" | www.andtun.top";
            autoSetSteamPath(getSteamPath());
            


        }

        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("开发者：Andtun(和豚)\n项目网址：www.andtun.top\n项目描述：利用SSFN授权文件对Steam进行免验证登录\n项目版本：" + version + "\n开发语言：C# 基于Net.(4.7.2)", "关于");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(ssfnPath + "/" + textBox1.Text))
            {
                try
                {
                    label2.Text = "\n删除旧SSFN文件:";
                    foreach (var oldSSFN in Directory.GetFiles(steamPath, "ssfn*"))
                    {
                        label2.Text = label2.Text + "\n" + oldSSFN;
                        File.Delete(oldSSFN);
                    }
                    File.Copy(ssfnPath + "/" + textBox1.Text, steamPath +textBox1.Text, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                    label2.Text = "\n新SSFN文件:";
                    label2.Text = steamPath +textBox1.Text;
                    MessageBox.Show("成功授权！", "信息");
                }
                catch (Exception)
                {
                    MessageBox.Show("授权失败，原因：未知！","错误");
                }
            }
            else
            {
                MessageBox.Show("未查询到此SSFN文件，无法提供授权！","错误");
            }
        }

        private void autoSetSteamPath(string exePath)
        {
            try
            {
                steamPath = exePath.Replace("Steam.exe", "");
                label2.Text = "\nSteam目录：\n" + steamPath;
            }
            catch (Exception)
            {
                MessageBox.Show("未获取到Steam目录，请手动选择路径！","错误");
            }
        }

        private void handSetSteamPath(string exePath)
        {
            try
            {
                steamPath = exePath.Replace("steam.exe", "");
                label2.Text = "\nSteam目录：\n" + steamPath;
            }
            catch (Exception)
            {
                MessageBox.Show("未知错误！", "错误");
            }
        }

        private static string getSteamPath()
        {
            string lnk = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Steam\steam.lnk";
            if (System.IO.File.Exists(lnk))
            {
                dynamic objWshShell = null, objShortcut = null;
                try
                {
                    objWshShell = Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_WshShell));
                    objShortcut = objWshShell.CreateShortcut(lnk);
                    return objShortcut.TargetPath;
                }
                finally
                {
                    Marshal.ReleaseComObject(objShortcut);
                    Marshal.ReleaseComObject(objWshShell);
                }
            }
            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Files (steam.exe)|steam.exe";//如果需要筛选txt文件（"Files (*.txt)|*.txt"）
            var result = openFileDialog1.ShowDialog();
            if (result==DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                handSetSteamPath(path);
            }


        }
    }
}
