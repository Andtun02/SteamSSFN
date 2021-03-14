using System;
using System.Collections.Generic;
using System.Text;

namespace SteamSSFN_v2
{
    public class Variable
    {
        //存放全局变量
        public static String github_Http = "https://raw.githubusercontent.com/AndtunO/SteamSSFN-ssfn/main/ssfn/";//国外github
        public static String gitee_Http = "https://gitee.com/andtun/SteamSSFN-ssfn/raw/main/ssfn/";//国内码云
        public static String github_Notice = "https://raw.githubusercontent.com/AndtunO/SteamSSFN/main/Server/Notice.txt";//github通知
        public static String gitee_Notice = "https://gitee.com/andtun/SteamSSFN/raw/main/Server/Notice.txt";//gitee通知
        public static String github_Info = "https://raw.staticdn.net/AndtunO/SteamSSFN/main/Server/Info.xml";//github镜像info
        public static String gitee_Info = "https://gitee.com/andtun/SteamSSFN/raw/main/Server/Info.xml";//giteeinfo
        public static String steamPath; //steam地址
        public static String httpSource;//当前源地址
    }
}
