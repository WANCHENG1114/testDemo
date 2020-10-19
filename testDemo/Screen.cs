using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace testDemo
{
    public partial class Screen : Form
    {
        
        public Screen()
        {
            InitializeComponent();
        }
        public enum PlayerTypes
        {
            TENNIS,
            FOOTBALL,
            BASKETBALL,
            PINGPANG,
            UNKNOWN
        }
        
    }
}
