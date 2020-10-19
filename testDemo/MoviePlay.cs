using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testDemo
{
    public partial class MoviePlay : Form
    {
        public MoviePlay()
        {
            InitializeComponent();
            //this.webBrowser1.Url = new System.Uri("http://baidu.com", System.UriKind.Absolute);
            string path = Application.StartupPath;
            path = path.Substring(0, path.IndexOf("\\bin\\Debug")) + @"\index.html";
            if (path != null)
            {
                webBrowser1.Navigate(path);
            }
            else
            {
                MessageBox.Show("err");
                return;
                string jsonStr = "";
                //string path = Application.StartupPath;
                File.WriteAllText(@"index.html", jsonStr);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete) return;
            Size szb = new Size(webBrowser1.Document.Body.OffsetRectangle.Width,
                webBrowser1.Document.Body.OffsetRectangle.Height);
            Size sz = webBrowser1.Size;

            int xbili = (int)((float)sz.Width / (float)szb.Width * 100);//水平方向缩小比例
            int ybili = (int)((float)sz.Height / (float)szb.Height * 100);//垂直方向缩小比例
            webBrowser1.Document.Body.Style = "zoom:" + xbili.ToString() + "%";
            webBrowser1.Invalidate();
        }
        #region 窗口可拖动

        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键
        private void MoviePlay_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void MoviePlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void MoviePlay_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        #endregion

        private void MoviePlay_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true; //禁用错误脚本提示
            webBrowser1.IsWebBrowserContextMenuEnabled = false; // 禁用右键菜单
            webBrowser1.WebBrowserShortcutsEnabled = false; //禁用快捷键
            webBrowser1.AllowWebBrowserDrop = false; // 禁止文件拖动
            webBrowser1.ScrollBarsEnabled = false;//禁用滚定条
        }
    }
}
