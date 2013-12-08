using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CC
{
    public partial class LampHelp : Form
    {
        public LampHelp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在修改配置时，选择要修改的对象后，输入-后面所要的命令，注意空格不能省略！");
        }
    }
}
