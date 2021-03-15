using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SteamSSFN_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //禁止动画
            sureButton.IsEnabled = false;
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(sureButton, true);
        }
        //程序窗口显示完毕
        private void MainWindowStart(object sender, EventArgs e)
        {

            Load();
            checkSteamPath();
            //创建ssfn文件夹
            if (Directory.Exists("./SteamSSFN")==false)
            {
                Directory.CreateDirectory("./SteamSSFN");
            }

        }

        private void checkSteamPath()
        {
            Variable.steamPath = Config.Read("SteamPath");
            if (Variable.steamPath != "null")
            {
                ztLabel.Content = "Steam路径: "+Variable.steamPath;
                
                //恢复动画
                sureButton.IsEnabled = true;
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(sureButton, false);
            }
            else
            {
                Variable.steamPath = checkRegistryPath();

                if (Variable.steamPath == null)
                {
                    ztLabel.Content = "异常: 无法从注册表获取您的Steam路径,请手动设置！";
                }
                else
                {
                    ztLabel.Content = "Steam路径: " + Variable.steamPath;
                    Config.Save("SteamPath", Variable.steamPath);
                    //恢复动画
                    sureButton.IsEnabled = true;
                    MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(sureButton, false);
                }
            }
            
        }

        //GET.当steam进程存在时，读取进程路径
        private string GetProcessPath_One()
        {
            Process[] processes = Process.GetProcessesByName("steam");

            if (processes.Length > 0)
            {
                string path = processes[0].MainModule.FileName;
                string result = path.Replace(@"\Steam.exe", "");
                result = result.Replace(@"\steam.exe", "");
                return result;
            }
            else
            {
                return null;
            }
        }
        //GET.根据注册表读取Steam路径
        private string checkRegistryPath()
        {
            Object obj = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return null;
                //未查询到注册表路径;
            }
        }
        //GET.手动设置路径

        public void Load()
        {
            //test
            reHttpSelected();//更新下载源选择框
        }

        private void reHttpSelected()
        {
            NoticeLabel.Text = "正在更新通知......";
            int a = int.Parse(Config.Read("HttpSource"));//String 转 Int
            if (a == httpSourceCombBox.SelectedIndex)
            {
                getNotice(0);
            }
            else
            {
                httpSourceCombBox.SelectedIndex = a;
            }
            Thread.Sleep(300);
        }

        private void getNotice(int Style)
        {
            String text = Net.getNotice(Style);
            NoticeLabel.Text = text;
        }

        private void sureButton_Click(object sender, RoutedEventArgs e)
        {
            //禁止动画
            sureButton.IsEnabled = false;
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(sureButton, true);
            if (mainInput.Text == "")
            {
                MessageBox.Show("抱歉，请输入SSFN码后再次点击授权按钮！", "提示: ");
            }
            else
            {
                //授权开始————————
                //[本地授权]
                LocalSSFNset(mainInput.Text);
            }
            

            //恢复动画
            sureButton.IsEnabled = true;
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(sureButton, false);
        }

        //本地授权
        private bool LocalSSFNset(String SSFN)
        {
            if (File.Exists("./SteamSSFN/"+SSFN) == true)
            {
                try
                {
                    //删除旧文件
                    foreach (var oldSSFN in Directory.GetFiles(Variable.steamPath, "ssfn*"))
                    {
                        ztLabel.Content = "状态: 删除旧授权-"+oldSSFN.ToString();
                        File.Delete(oldSSFN);
                    }
                    //复制文件
                    File.Copy( "./SteamSSFN/" + SSFN, Variable.steamPath+ "/" + SSFN, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                    ztLabel.Content = "状态: 成功授权-" + SSFN;
                    mainInput.Text = "";
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.ToString(), "异常: 授权失败");
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        //http源选择 改变
        int count = 0;
        private void httpSourceCombBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (count > 0)
            {
                /*String hs = Config.Read("httpSource");
                int index = httpSourceCombBox.SelectedIndex;
                switch (hs)
                {
                    case "0":
                        Variable.httpSource = "0";
                        Config.Save("HttpSource", index + "");
                        break;
                    case "1":
                        Variable.httpSource = "1";
                        Config.Save("HttpSource", index + "");
                        break;
                    default:
                        break;*/
                //Config.Save("HttpSoucre", httpSourceCombBox.SelectedIndex+"");
                Config.Save("HttpSource", httpSourceCombBox.SelectedIndex.ToString());
                Variable.httpSource = httpSourceCombBox.SelectedIndex.ToString();
            }
            NoticeLabel.Text = "正在更新通知......";
            //MessageBox.Show("test");
            getNotice(1);
            //因为 每次开启软件都会更改，所以判断第一次启动的更改无效
            ++count;
        }
        //按下设置键
        private void Settting_Buttton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingGrid.Visibility == Visibility.Visible)
            {
                SettingGrid.Visibility = Visibility.Collapsed;
                SettingBut_ICO.Kind = MaterialDesignThemes.Wpf.PackIconKind.Cog;
            }
            else
            {
                SettingGrid.Visibility = Visibility.Visible;
                SettingBut_ICO.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowBack;
                SetSteamPath_IN.Text = Variable.steamPath;
            }
        }
        
        //设置路径
        private void SetSteamPath_Bu_Click(object sender, RoutedEventArgs e)
        {
            String x = GetProcessPath_One();
            if (x == null)
            {
                SetSteamPath_Bu.IsEnabled = false;
                MessageCard.Visibility = Visibility.Visible;
                Label_1.Content = "抱歉，此功能需要您先打开 ‘Steam.exe’ \n再次点击按钮会自动读取路径。\n如果您想手动设置\n请复制Steam根目录路径复制\n粘贴进输入框即可。";
                
            }
            else
            {
                SetSteamPath_IN.Text = x;
                //恢复动画
                sureButton.IsEnabled = true;
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(sureButton, false);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageCard.Visibility = Visibility.Collapsed;
            SetSteamPath_Bu.IsEnabled = true;
        }

        //路径改变
        private void SetSteamPath_IN_CG(object sender, TextChangedEventArgs e)
        {
            Variable.steamPath = SetSteamPath_IN.Text;
            ztLabel.Content = "Steam路径: " + Variable.steamPath;
            Config.Save("SteamPath", Variable.steamPath);
        }
        
        //打开ssfn存放文件夹
        private void OpenSSFNPath_Button_Click(object sender, RoutedEventArgs e)
        {
            String path = Directory.GetCurrentDirectory() + @"\SteamSSFN";
            MessageBox.Show(path);
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }
}

