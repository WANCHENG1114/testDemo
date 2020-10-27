using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);
        }
       
        #region 基础属性

        #region 标题栏移动
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                //这里一定要判断鼠标左键按下状态，否则会出现一个很奇葩的BUG，不信邪可以试一下~~
                ReleaseCapture();
                SendMessage(Handle, 0x00A1, 2, 0);
            }
        }
        //窗体移动API
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                else
                {
                    this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    this.WindowState = FormWindowState.Maximized;
                }
            }
        }

        #endregion

        #region 小大关
        private void pbox_XXX_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.White;
        }
        private void pbox_XXX_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = SystemColors.Control;
        }
        private void pbos_zuidahua_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void pbox_zuixiaohua_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void pbox_guanbi_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ADD属性
        private int _Border = 1;
        [Category("Style")]
        [Description("窗体边框粗细")]
        [DefaultValue(typeof(int), "1")]
        public int Border
        {
            get { return _Border; }
            set
            {
                if (_Border != value)
                {
                    _Border = value;
                    //SetBorder();
                }
            }
        }


        #endregion

        #region 窗体阴影

        private const int CS_DropSHADOW = 0x20000;
        private const int GCL_STYLE = (-26);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);


        #endregion

        #endregion
    }
}
