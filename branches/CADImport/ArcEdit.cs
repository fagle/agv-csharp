using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AGV
{
    public partial class ArcEdit : Form
    {
        public ArcEdit(int Ox, int Oy, int radius, int startAngle, int sweepAngle)
        {
            InitializeComponent();
            textBoxOx.Text = Ox.ToString();
            textBoxOy.Text = Oy.ToString();
            textBoxRadius.Text = radius.ToString();
            textBoxStartAngle.Text = startAngle.ToString();
            textBoxSweepAngle.Text = sweepAngle.ToString();
            textBoxEndAngle.Text = (startAngle + sweepAngle).ToString();
        }
    }
}
