using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SteamSSFN_v2
{
    //读取 远程XAML
    //来自 https://www.cnblogs.com/hayden/archive/2009/08/17/1548276.html

    //读取 本地配置文件
    //来自 https://www.cnblogs.com/fuchongjundream/p/4206314.html

    /// <summary>
    /// 配置文件 读/写
    /// </summary>
    public class Config
    {
        //读配置文件
        public static string Read(string get)
        {
            return ConfigurationManager.AppSettings[get];
        }

        //写配置文件
        public static void Save(string set, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[set].Value = value;
            config.Save();
        }
    }
}
