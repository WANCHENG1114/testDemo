using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace testDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Text"] = userText.Text;
            String jsonStr = JsonConvert.SerializeObject(dic);
            //string path = Application.StartupPath;
            File.WriteAllText(@"cache.data",jsonStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jsonStr= File.ReadAllText(@"cache.data");
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
            userText.Text = dic["Text"].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            userText.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", @"cache.data");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (MoviePlay moviePlay = new MoviePlay())
            {
                moviePlay.ShowDialog();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int totalFile = 0;
                //string dirPath = @"E:\chfuMetarnet\BSC6810 alarm\";
                if (this.textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("请输入HTML文件路径！");
                }
                else
                {
                    string dirPath = this.textBox1.Text.Trim();
                    if (!dirPath.Substring(dirPath.Length - 1).Contains("\\"))
                    {
                        dirPath = dirPath + "\\";
                    }
                    StreamWriter sw;
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                    FileInfo[] files = dirInfo.GetFiles();
                    string filename = dirPath + "告警经验库信息.txt";
                    if (File.Exists(filename))
                    {
                        sw = File.AppendText(filename);
                    }
                    else
                    {
                        sw = File.CreateText(filename);
                    }
                    foreach (FileInfo fileinfo in files)
                    {
                        if (fileinfo.Extension.Equals(".html"))//遍历所有htm文件
                        {
                            totalFile = totalFile + 1;
                            WebRequest myWebRequest = WebRequest.Create(dirPath + fileinfo.Name);
                            WebResponse myWebResponse = myWebRequest.GetResponse();
                            Stream myStream = myWebResponse.GetResponseStream();
                            Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
                            StreamReader myStreamReader = new StreamReader(myStream, encode);
                            string strhtml = myStreamReader.ReadToEnd();
                            myWebResponse.Close();
                            string stroutput = strhtml;
                            Regex regex = new Regex(@"<[^>]+>|</[^>]+>");//去掉HTML标记的正则表达式
                            string tmpStr = "<h4>([^<]*)</h4>";        //获取<h4>之间内容的表达式
                            Match TitleMatch = Regex.Match(strhtml, tmpStr, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            string causename = TitleMatch.Value.ToString();//包含<h4>和</h4>标记
                            causename = Regex.Replace(causename, "[\n|\r|\t]", " ");//去掉换行和TAB键符号
                            causename = causename.Trim();
                            string cause = causename.Substring(4, causename.Length - 9);//得到告警原因
                            string titleStr = "<title>([^<]*)</title>";
                            TitleMatch = Regex.Match(strhtml, titleStr, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            string titlename = TitleMatch.Value.ToString();
                            titlename = Regex.Replace(titlename, "[\n|\r|\t]", "");//去掉换行和TAB键符号
                            titlename = titlename.Trim();
                            string regexStr = "<ul><li>(?<key>.*?)</ul>";//获取<ul><li>后边的内容，直到</ul>结尾
                            Regex r = new Regex(regexStr, RegexOptions.None);
                            strhtml = Regex.Replace(strhtml, "[\n|\r|\t]", "");//去掉换行和TAB键符号
                            Match mc = r.Match(strhtml);
                            string dataStr = mc.Groups["key"].Value;
                            dataStr = "<ul><li>" + dataStr + "</ul>";//得到完整的<ul></ul>之间的源码
                            strhtml = strhtml.Replace(dataStr, "");//将去掉换行符和tab键的源码中去除<ul></ul>部分源码
                            strhtml = strhtml.Replace(titlename, "");//去掉<title></title>
                            strhtml = regex.Replace(strhtml, " ");//过滤掉HTML标记
                            strhtml = strhtml.Replace("&nbsp;", "");//去掉空格字符
                            string[] arr = cause.Split(' ');
                            string zhCause = arr[arr.Length - 1];//获取数组最后一个元素：告警原因
                            sw.WriteLine("第" + totalFile + "个文件：" + fileinfo.Name);
                            sw.WriteLine("-----告警原因------：");
                            //sw.WriteLine(cause);// ALM-1 网元启动
                            //zhCause = this.chinaString(zhCause);
                            sw.WriteLine(zhCause);//网元启动
                            sw.WriteLine("-----处理经验------：");
                            sw.WriteLine(strhtml);
                            sw.WriteLine();
                            sw.Flush();

                        }
                    }
                    sw.Close();
                    MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("操作失败：" + ee.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + @"\index.html";
            string jsonStr = textBox1.Text;
            //string path = Application.StartupPath;
            File.WriteAllText(path, jsonStr);
            MessageBox.Show(path);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件|*.jpg|所有文件|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Clipboard.SetDataObject(Image.FromFile(openFileDialog.FileName), false);
                userText.Paste();
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
           
        }

        private void button10_Click(object sender, EventArgs e)
        {
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
           {
               var Text = System.IO.Path.GetFullPath(openFileDialog1.FileName); //获取文件路径
                MessageBox.Show(Text);
            }


        }

        private void button12_Click(object sender, EventArgs e)
        {
            string pattern = @"^(\d|[1-5]\d|60)$";
            //string pattern = @"^60|^(\d|[1-5]\d)(\.\d+)*$";

            //string pattern = @"^[0-9]*$";
            if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                textBox2.Clear();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                return;
            }
          
            
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text)|| string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("不填数量，想屁吃呢!");
                textBox4.Focus();
                return;
            }
            int num = int.Parse(textBox4.Text);
            
            Task.Run(() => set1(num));
            Task.Run(() => set2(num));
        }
        private void set1(int num)
        {
            int X = 0;
            for (int i = 0; i < num; i++)
            {
                X++;
                X = X % 10;
                Thread.Sleep(int.Parse(textBox3.Text));
                string str = "G" + "12" + GetTimeStamp() + GenerateRandomCode(3)+X  ;
                listBox1.Invoke(new MethodInvoker(delegate
                {
                    if (listBox1.Items.Contains(str))
                    {
                        listBox1.Items.Add("重复元素"+ str);
                    }
                    else
                    {
                        listBox1.Items.Add(str);
                        listBox1.SelectedIndex = listBox1.Items.Count-1;
                    }
                    Application.DoEvents();
                }));
            }
        }
        private void set2(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Thread.Sleep(int.Parse(textBox3.Text));
                string str = "G" + "12" + GetTimeStamp() + i.ToString().PadLeft(2, '0');
                listBox2.Invoke(new MethodInvoker(delegate
                {
                    if (listBox2.Items.Contains(str))
                    {
                        listBox2.Items.Add("重复元素" + str);
                    }
                    else
                    {
                        listBox2.Items.Add(str);
                        listBox2.SelectedIndex = listBox2.Items.Count - 1;
                    }
                    Application.DoEvents();
                }));
            }
        }
        /// <summary>
        ///生成制定位数的随机码（数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            InitialDialog(openFileDialog1,"文件");
        }

        string FileName;
        //默认打开路径
        private string InitialDirectory = "D:\\";
        //统一对话框
        private bool InitialDialog(FileDialog fileDialog, string title)
        {
            fileDialog.InitialDirectory = InitialDirectory;//初始化路径
            fileDialog.Filter = "txt files (*.txt,*.*)|*.txt;*.*";//过滤选项设置，文本文件，所有文件。
            fileDialog.FilterIndex = 1;//当前使用第二个过滤字符串
            fileDialog.RestoreDirectory = true;//对话框关闭时恢复原目录
            fileDialog.Title = title;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 1; i <= fileDialog.FileName.Length; i++)
                {
                    if (fileDialog.FileName.Substring(fileDialog.FileName.Length - i, 1).Equals(@"\"))
                    {
                        //更改默认路径为最近打开路径
                        InitialDirectory = fileDialog.FileName.Substring(0, fileDialog.FileName.Length - i + 1);
                        return true;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        private void Open(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件对话框              
            if (InitialDialog(openFileDialog, "Open"))
            {
                using (Stream stream = openFileDialog.OpenFile())
                {
                    FileName = ((System.IO.FileStream)stream).Name;
                    // 执行相关文件操作
　　　　　　　　　　　　
                }
            }
        }
        private void Save(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();//打开文件对话框              
            if (InitialDialog(saveFileDialog, "Save"))
            {
                using (Stream stream = saveFileDialog.OpenFile())
                {
                    FileName = ((System.IO.FileStream)stream).Name;
                    //执行保存动作
                    
                    MessageBox.Show("保存成功。");
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        private void button17_Click(object sender, EventArgs e)
        {
            AllocConsole();
            GetNetworkInterface();
        }
        
        private void GetNetworkInterface()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();//获取本地计算机上网络接口的对象
            Console.WriteLine("适配器个数：" + adapters.Length);
            Console.WriteLine();
            foreach (NetworkInterface adapter in adapters)
            {
                Console.WriteLine("描述：" + adapter.Description);
                Console.WriteLine("标识符：" + adapter.Id);
                Console.WriteLine("名称：" + adapter.Name);
                Console.WriteLine("类型：" + adapter.NetworkInterfaceType);
                Console.WriteLine("速度：" + adapter.Speed * 0.001 * 0.001 + "M");
                Console.WriteLine("操作状态：" + adapter.OperationalStatus);
                Console.WriteLine("MAC 地址：" + adapter.GetPhysicalAddress());

                // 格式化显示MAC地址                
                PhysicalAddress pa = adapter.GetPhysicalAddress();//获取适配器的媒体访问（MAC）地址
                byte[] bytes = pa.GetAddressBytes();//返回当前实例的地址
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("X2"));//以十六进制格式化
                    if (i != bytes.Length - 1)
                    {
                        sb.Append("-");
                    }
                }
                Console.WriteLine("MAC 地址：" + sb);
                Console.WriteLine();
            }
            //Console.ReadKey();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
