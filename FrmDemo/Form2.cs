using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmDemo
{
    public partial class Form2 : SkinMain.FrmSkin
    {
        public Form2()
        {
            InitializeComponent();
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
