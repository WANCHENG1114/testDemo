using Echevil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testDemo
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private NetworkAdapter[] adapters;
        private NetworkMonitor monitor;
        private void Form3_Load(object sender, EventArgs e)
        {
            monitor = new NetworkMonitor();
            this.adapters = monitor.Adapters;

            if (adapters.Length == 0)
            {
                this.ListAdapters.Enabled = false;
                MessageBox.Show("No network adapters found on this computer.");
                return;
            }

            this.ListAdapters.Items.AddRange(this.adapters);
        }
        private void ListAdapters_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            NetworkAdapter networkAdapter = adapters[this.ListAdapters.SelectedIndex];
            Speed_indicator form2 = new Speed_indicator(networkAdapter);
            form2.Show();
            this.Close();
        }
    }
}
