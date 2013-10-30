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
        public ArcEdit(int Ox, int Oy, int radius, int startAngle, int sweepAngle, int indexNo)
        {
            InitializeComponent();
            textBoxOx.Text = Ox.ToString();
            textBoxOy.Text = Oy.ToString();
            textBoxRadius.Text = radius.ToString();
            textBoxStartAngle.Text = startAngle.ToString();
            textBoxSweepAngle.Text = sweepAngle.ToString();
            textBoxEndAngle.Text = (startAngle + sweepAngle).ToString();
            textBoxNo.Text = indexNo.ToString();
        }

        public Point Center 
        {
            get {
                return new Point(Convert.ToInt32(textBoxOx.Text),-Convert.ToInt32(textBoxOy.Text));
            }
        }

        public int Radius 
        {
            get {
                return Convert.ToInt32(textBoxRadius.Text);
            }
        }

        public int StartAngle {
            get {
                return Convert.ToInt32(textBoxStartAngle.Text);    
            }
        }

        public int SweepAngle {
            get {
                return Convert.ToInt32(textBoxSweepAngle.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxEndAngle.Text = textBoxStartAngle.Text;
            int startAngle = Convert.ToInt32(textBoxStartAngle.Text) + Convert.ToInt32(textBoxSweepAngle.Text);
            if (startAngle > 360)
                startAngle -= 360;
            else if (startAngle < 0)
                startAngle += 360;
            textBoxStartAngle.Text = startAngle.ToString();
            textBoxSweepAngle.Text = (-Convert.ToInt32(textBoxSweepAngle.Text)).ToString();
        }       
    }
}
