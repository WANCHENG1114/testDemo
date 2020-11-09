using dragBoxTest;
using FrmDemo.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmDemo
{
   
    public partial class Form2 : SkinMain.FrmSkin
    {
        SqlHelp sqlHelp = new SqlHelp();
        public Form2()
        {
            InitializeComponent();
            InitButton();
        }

        private void InitButton()
        {
            foreach (Control Control in panel3.Controls)
            {
                treeView1.Nodes.Add(Control.Name);
                if (Control is Button)
                {
                    Button button = Control as Button;
                    button.BackColor = System.Drawing.SystemColors.ActiveCaption;
                    button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    button.ForeColor = System.Drawing.Color.White;
                }
            }

        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            string sqlstr = "select * from Table_2";
            var date=  sqlHelp.ExecuteQuery(sqlstr);

            dataGridView1.DataSource = date;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //创建一个节点对象，并初始化
                TreeNode tmp;
                tmp = new TreeNode("节点名称");
                //在TreeView组件中加入子节点
                treeView1.SelectedNode.Nodes.Add(tmp);
                treeView1.SelectedNode = tmp;
                treeView1.ExpandAll();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //创建一个节点对象，并初始化
                TreeNode tmp;
                tmp = new TreeNode("节点");
                //在TreeView组件中加入兄弟节点
                treeView1.SelectedNode.Parent.Nodes.Add(tmp);
                treeView1.ExpandAll();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //判断选定的节点是否存在下一级节点
            if (treeView1.SelectedNode.Nodes.Count == 0)
                //删除节点
                treeView1.SelectedNode.Remove();
            else
                MessageBox.Show("请先删除此节点中的子节点！", "提示信息" , MessageBoxButtons.OK ,MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (TreeNode item in treeView1.Nodes)
            {
                //定位根节点
                treeView1.SelectedNode = treeView1.Nodes[item.Index];
                //展开组件中的所有节点
                treeView1.SelectedNode.ExpandAll();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (TreeNode item in treeView1.Nodes)
            {
                //定位根节点
                treeView1.SelectedNode = treeView1.Nodes[item.Index];
                //折叠组件中所有节点
                treeView1.SelectedNode.Collapse();
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.Expand();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DragBox dragBox = new DragBox();
            foreach (Control control  in panel3.Controls)
            {
                if (control is DataGridView)
                {
                    dragBox.WireControl(control);
                }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        { string fileSource = flie;
            //获取用户选择路径  导出Execl
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.Filter = "文本文件(*.txt)|*.txt|Miscrosoft Office Excel 97-2003 工作表|*.xls|jpg files(*.jpg)|*.jpg|所有文件|*.*";
            SaveFile.FileName = "保存";//设置默认文件名
            SaveFile.DefaultExt = "txt";//设置默认格式（可以不设）
            SaveFile.AddExtension = true;//设置自动在文件名中添加扩展名
            SaveFile.RestoreDirectory = true;
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                string targetPath = SaveFile.FileName;
                if (File.Exists(targetPath)) { File.Delete(targetPath); }
                if (fileSource!="")
                {
                    File.Copy(fileSource, SaveFile.FileName);
                    DialogResult r = MessageBox.Show("导出数据成功！", "系统信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (r == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(SaveFile.FileName);
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(SaveFile.FileName))
                    {
                        string txtstr = richTextBox1.Text;
                        sw.WriteLineAsync(@txtstr);
                    }
                }
                richTextBox1.AppendText(Environment.NewLine);
                richTextBox1.AppendText($"以保存至{SaveFile.FileName}");
            }
            
        }
        string flie = "";
        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Photo";
            dlg.Filter = "jpg files(*.jpg)|*.jpg|All files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    richTextBox1.AppendText(dlg.FileName);
                    flie = dlg.FileName;

                    Clipboard.SetDataObject(Image.FromFile(dlg.FileName), false);
                    richTextBox1.Paste();
                  
                    //richTextBox1 = new Bitmap(dlg.OpenFile());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            dlg.Dispose();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                TreeviewHelper class1 = new TreeviewHelper();
                class1.SetTreeNoByFilePath(treeView1, dilog.SelectedPath, imageList1);
            }

         

        }
        private void button12_Click(object sender, EventArgs e)
        {
            //画直线
            Graphics g = CreateGraphics();
            Pen p = new Pen(Color.Gray);
            g.DrawLine(p, 0, this.Height / 2, this.Width, this.Height / 2);

            //画矩形
            g.DrawRectangle(p, 50, 50, 200, 100);

            //画圆形
            g.DrawEllipse(p, 100, 100, 100, 100);

            //画圆柱体
            int height = this.ClientSize.Height - 200;
            int width = this.ClientSize.Width - 50;
            int vHeight = 200;
            int vWidth = 100;
            g.Clear(Color.White);
            SolidBrush brush = new SolidBrush(Color.Gainsboro);
            for (int i = height / 2; i > 0; i--)
            {
                g.DrawEllipse(p, width / 2, i, vHeight, vWidth);
            }
            g.FillEllipse(brush, width / 2, 0, vHeight, vWidth);
            
            //释放资源
            p.Dispose();
            g.Dispose();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
        }

    }
    interface ITest
    {
        string name { get; set; }
        void Print();
    }
    abstract class Test : ITest
    {
        public abstract string name { get; set; }
        public abstract void Print();
    }
}
