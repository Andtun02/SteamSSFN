using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace SteamSSFN_v2
{
    class Net
    {
        //获取服务器通知
        //获取网页内容
        //来自 https://www.cnblogs.com/dennys/p/3400295.html
        public static string getNotice(int Style)
        {
            String ds = "";
            String b_ds;
            if (Style == 0)
            {
                b_ds = Config.Read("HttpSource");
            }
            else
            {
                b_ds = Variable.httpSource;
            }
            if (b_ds == "0")
            {
                //MessageBox.Show(b_ds+"\n 变量："+Variable.httpSource + "ds:" + ds);
                ds = Variable.gitee_Notice;
            }
            else if (b_ds == "1")
            {
                //MessageBox.Show(b_ds + "\n 变量：" + Variable.httpSource+"ds:"+ds);
                ds = Variable.github_Notice;
            }
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                Byte[] pageData = MyWebClient.DownloadData(ds); //从指定网站下载数据
                string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            
                //调试 MessageBox.Show(ds);                               //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
                //MessageBox.Show(b_ds,pageHtml);
                return pageHtml;
            }
            catch (Exception)
            {
                return "[异常] 无法连接至云端,请尝试更换下载源。";
            }
        }


        //通过http下载文件
        public static bool DownloadFile(string style, string filename)
        {
            try
            {
                WebClient dw = new WebClient();
                dw.DownloadFile(style + "/" + filename, Directory.GetCurrentDirectory() + "\\SteamSSFN\\" + filename);
                return true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "异常: 下载出错");
                return false;
            }
        }

    }
}
