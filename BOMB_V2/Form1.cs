using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOMB_V2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MessageBox.Show("食用方法：\n1、复制想要发送的内容\n2、输入QQ账号\n3、点击开始");

            txtAccont.Text = "";
            txtCisu.Text = "5";
            txtYanshi.Text = "100";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string accont = txtAccont.Text;
            string cisu = txtCisu.Text;
            string yanshi = txtYanshi.Text;
            if (string.IsNullOrEmpty(accont))
            {
                MessageBox.Show("输入QQ号码");
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
                            { "On Error Resume Next",
                            "Set WshShell=WScript.CreateObject(\"WScript.Shell\")",
                            $"WshShell.run \"tencent://message/?Menu=yes&uin={accont}&Site=&Service=200&sigT=2a39fb276d15586e1114e71f7af38e195148b0369a16a40fdad564ce185f72e8de86db22c67ec3c1\",0,true",
                            "WScript.Sleep 3000",
                            $"for i=1 to {cisu}",
                            $"WScript.Sleep {yanshi}",
                            "WshShell.SendKeys \"^v\"",
                            "WshShell.SendKeys \"%s\"",
                            "Next"
                            };
            string[] contents2 = new string[]
                            { "On Error Resume Next",
                            "Set WshShell=WScript.CreateObject(\"WScript.Shell\")",
                            $"WshShell.run \"tencent://message/?Menu=yes&uin={accont}&Site=&Service=200&sigT=2a39fb276d15586e1114e71f7af38e195148b0369a16a40fdad564ce185f72e8de86db22c67ec3c1\",0,true",
                            "WScript.Sleep 3000",
                            $"for i=1 to {cisu}",
                            $"WScript.Sleep {yanshi}",
                            "WshShell.SendKeys \"^v\"",
                            "WshShell.SendKeys i",
                            "WshShell.SendKeys \"%s\"",
                            "Next"
                            };
            //string path = Application.StartupPath;
            if (checkBox1.Checked == true)
            {
                contents = contents2;
            }
            else
            {
                contents = contents1;
            }
            File.AppendAllLines(@"bomb.vbs", contents, Encoding.Default);
            
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
