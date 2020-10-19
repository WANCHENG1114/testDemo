using Echevil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testDemo
{
    public partial class Speed_indicator : Form
    {
        private NetworkAdapter[] adapters;//声明网卡适配器列表
        private NetworkMonitor monitor=new NetworkMonitor();//声明检测器

        NetworkAdapter adapter = null;
        public Speed_indicator(NetworkAdapter networkAdapter=null)
        {
            InitializeComponent();
            adapter = networkAdapter;
        }
        List<NetworkAdapter> networkAdaptersList = new List<NetworkAdapter>();
        private void FormMain_Load(object sender, System.EventArgs e)
        {
            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.BackColor = Color.Fuchsia;
            //this.TransparencyKey = this.BackColor;
            //this.Opacity = 0.5;
            contextMenuStrip1.Items.Add("退出");
            contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler((object sd, ToolStripItemClickedEventArgs ev) =>
             {
                 ItemMenu(ev.ClickedItem.Text);
             });
            this.ContextMenuStrip = contextMenuStrip1;
            listBox1.ContextMenuStrip = contextMenuStrip1;
            
            

            monitor.StopMonitoring();
            if (adapter == null)
            {
                foreach (var item in monitor.Adapters)
                {
                    if (item.Name.StartsWith("Intel[R]"))
                    {
                        monitor.StartMonitoring(item);
                        networkAdaptersList.Add(item);
                    }
                }
            }
            else
            {
                monitor.StartMonitoring(adapter);
                networkAdaptersList.Add(adapter);
            }

          
            labelDown.AutoSize = false;
            labelDown.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            labelDown.Dock = DockStyle.Top;
            labelDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.down_MouseDown);



            labelUp.AutoSize = false;
            labelUp.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            labelUp.Dock = DockStyle.Bottom;
            labelUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.up_MouseDown);
            this.Controls.Add(labelDown);
            this.Controls.Add(labelUp);

            this.TimerCounter.Start();
        }
        Label labelDown = new Label();
        Label labelUp = new Label();
        private string SpeedConvert(double str)
        {
            var mstr= str / 1024;
            if (mstr > 1)
            {
                return String.Format("{0:n} MB/s", mstr);
            }
            return String.Format("{0:n} KB/s", str);

            /*MB/s 的含义是兆字节每秒，Mb/s的含义是兆比特每秒，前者是指每秒传输的字节数量，
             * 后者是指每秒传输的比特位数。二者是完全不同的。Byte是字节 数，bit是位数，
             * 在计算机中每八位为一字节，也就是1Byte＝8bit，是1：8的对应关系。因此1MB/s等于8Mb/s。
             * 因此在在书写单位时一定 要注意B字母的大小写，此时B字母的大小真可以称为失之毫厘，谬以千里。
             * 在数据传输率上官方数据中(如电信部门)一般采用Mb/s或Kb/s为单位。
             * 而下载软件(如IE、迅雷、快车)一般采用MB/s或KB/s为单位。
             */
        }
        private void TimerCounter_Tick(object sender, System.EventArgs e)
        {
            listBox1.Items.Clear();
            double down=0;
            double up=0;
            foreach (var item in networkAdaptersList)
            {
                down += item.DownloadSpeedKbps;
                up += item.UploadSpeedKbps;
            }
            //listBox1.Items.Add( SpeedConvert(down));
            //listBox1.Items.Add( SpeedConvert(up));
            listBox1.Visible = false;
            
            labelDown.Text = "下载：" + SpeedConvert(down);
            labelUp.Text ="上传："+ SpeedConvert(up);
        }

        private void ItemMenu(string str)
        {
            switch (str)
            {
                case "退出":
                    Close();
                    break;
            }
        }

        #region 全局移动
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }
        private void down_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        } private void up_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }
        #endregion
    }
}
