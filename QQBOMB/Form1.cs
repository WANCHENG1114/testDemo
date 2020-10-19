using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QQBOMB
{
    public partial class BOMB : Form
    {
        public BOMB()
        {
            InitializeComponent();

            MessageBox.Show("食用方法：\n1、复制想要发送的内容\n2、鼠标点击输入框\n3、点击开始");

            txtName.Text = "";
            txtCisu.Text = "5";
            txtYanshi.Text = "100";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //wscript.exe 


            /*
             
               Dim WshShell

               Set WshShell= Wscript.CreateObject("WScript.Shell")
               
               WshShell.AppActivate "便签"
               
               for i=1 to 5
               WScript.Sleep 100
               
               WshShell.SendKeys "^v"
               
               WshShell.SendKeys i
               
               WshShell.SendKeys "%s"
               
               Next

             */

            string name = txtName.Text;
            string cisu = txtCisu.Text;
            string yanshi = txtYanshi.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("输入要发送人的备注名");
                return;
            }
            if (string.IsNullOrEmpty(cisu))
            {
                MessageBox.Show("输入次数");
                return;
            }
            if (string.IsNullOrEmpty(yanshi))
            {
                MessageBox.Show("输入延时时间/毫秒");
                return;
            }
            if (File.Exists(@"bomb.vbs"))
            {
                File.Delete(@"bomb.vbs");
            }
            string[] contents = null;
            string[] contents1 = new string[]
                            { "Dim WshShell",
                              "Set WshShell= Wscript.CreateObject(\"WScript.Shell\")",
                              $"WshShell.AppActivate \"{name}\"",
                              $"for i = 1 to {cisu}",
                              $"WScript.Sleep {yanshi}",
                              "WshShell.SendKeys \"^v\"",
                              "WshShell.SendKeys \"%s\"",
                              "Next"};
            string[] contents2 = new string[]
                            { "Dim WshShell",
                              "Set WshShell= Wscript.CreateObject(\"WScript.Shell\")",
                              $"WshShell.AppActivate \"{name}\"",
                              $"for i = 1 to {cisu}",
                              $"WScript.Sleep {yanshi}",
                              "WshShell.SendKeys \"^v\"",
                              "WshShell.SendKeys i",
                              "WshShell.SendKeys \"%s\"",
                              "Next"};
            //string path = Application.StartupPath;
            if (checkBox1.Checked == true)
            {
                contents = contents2;
            }
            else
            {
                contents = contents1;
            }
            File.AppendAllLines(@"bomb.vbs", contents,Encoding.Default);
            
            System.Diagnostics.Process.Start(@"bomb.vbs");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"stop.vbs"))
            {
                File.Delete(@"stop.vbs");
            }
            string[] contents = new string[]
                            { "Dim Wsh",
                            "Set Wsh = WScript.CreateObject(\"WScript.Shell\")",
                            "WScript.Sleep(100)",
                            "Wsh.Run \"taskkill /f /im wscript.exe\",0",
                            "Set Wsh=NoThing",
                            "WScript.quit"};
            //string path = Application.StartupPath;
            File.AppendAllLines(@"stop.vbs", contents, Encoding.Default);

            System.Diagnostics.Process.Start(@"stop.vbs");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(@"bomb.vbs"))
            {
                File.Delete(@"bomb.vbs");
            }

            if (File.Exists(@"stop.vbs"))
            {
                File.Delete(@"stop.vbs");
            }
        }
    }
}
