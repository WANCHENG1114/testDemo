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

namespace testDemo
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Id", Type.GetType("System.Int32"));

            //DataRow newRow = table.NewRow();
            //newRow["Name"] = "张三";
            //newRow["Id"] = 10;
            //table.Rows.Add(newRow);

            table.Rows.Add(new object[] { "张三", "10"});
            table.Rows.Add(new object[] { "张三", "11"});
            table.Rows.Add(new object[] { "张三", "12"});
            table.Rows.Add(new object[] { "张三", "13"});
            table.Rows.Add(new object[] { "张三", "14"});

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                string newName = "张三"+ i;
                //修改记录值
                row["Name"] = newName;
            }
            dataGridView1.DataSource = table;

           

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = "郑州市运输管理中心违章车辆信息.xls";

            exportExcel(fileName, this.dataGridView1);
        }
        public void exportExcel(string fn, DataGridView dv)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "将查询结果导出成EXCEL文件";
            sf.DefaultExt = "xls";
            sf.FileName = fn;
            sf.Filter = "Excel文件(*.xls)|*.xls";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string sFileName = sf.FileName;
                if (File.Exists(sFileName))
                {
                    File.Delete(sFileName);
                }
                FileStream objFileStream;
                StreamWriter objStreamWrite;
                string strLine = "";
                objFileStream = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write);
                objStreamWrite = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);
                for (int i = 0; i < dv.Columns.Count; i++)
                {
                    if (dv.Columns[i].Visible == true)
                    {
                        if (!dv.Columns[i].Name.Contains("未对应信息"))
                        {
                            strLine = strLine + dv.Columns[i].HeaderText.ToString() + Convert.ToChar(9);
                        }
                    }
                }
                objStreamWrite.WriteLine(strLine);
                strLine = "";

                for (int i = 0; i < dv.Rows.Count; i++)
                {
                    if (dv.Columns[0].Visible == true)
                    {
                        if (dv.Rows[i].Cells[0].Value == null)
                        {
                            strLine = strLine + " " + Convert.ToChar(9);
                        }
                        else
                        {
                            strLine = strLine + dv.Rows[i].Cells[0].Value.ToString() + Convert.ToChar(9);
                        }
                        for (int j = 1; j < dv.Columns.Count; j++)
                        {
                            if (dv.Columns[j].Visible == true)
                            {
                                if (!dv.Columns[j].Name.Contains("未对应信息"))
                                {
                                    if (dv.Rows[i].Cells[j].Value == null)
                                    {
                                        strLine = strLine + " " + Convert.ToChar(9);
                                    }
                                    else
                                    {
                                        string rowstr = "";
                                        rowstr = dv.Rows[i].Cells[j].Value.ToString();
                                        if (rowstr.IndexOf("\r\n") > 0)
                                        {
                                            rowstr = rowstr.Replace("\r\n", " ");
                                        }
                                        if (rowstr.IndexOf("\t") > 0)
                                        {
                                            rowstr = rowstr.Replace("\t", " ");
                                        }
                                        strLine = strLine + rowstr + Convert.ToChar(9);
                                    }
                                }
                            }
                        }
                        objStreamWrite.WriteLine(strLine);
                        strLine = "";
                    }
                }
                objStreamWrite.Close();
                objFileStream.Close();
                MessageBox.Show("导出Excel成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;//去掉最后一行空白行
            if (e.RowIndex >= dataGridView1.Rows.Count)
                return;
            DataGridViewRow dgr = dataGridView1.Rows[e.RowIndex];
            try
            {
                if (int.Parse(dgr.Cells["Id"].Value.ToString()) % 2 == 0)
                {
                    dgr.DefaultCellStyle.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e == null || e.Value == null || !(sender is DataGridView))
                return;
            DataGridView view = (DataGridView)sender;
            object originalValue = e.Value;
            if (view.Columns[e.ColumnIndex].DataPropertyName == "Id")
            {
                e.Value = e.Value .ToString()== "10"? "是":"否";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int status = (int)(TaskSubStatus)Enum.Parse(typeof(TaskSubStatus), "当前");
            MessageBox.Show(status.ToString());
        }
    }
    public enum TaskSubStatus
    {
        历史 = 0,
        当前 = 1,
        全部 = 2,
        待生效 = -1
    }
}
