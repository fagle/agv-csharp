using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Avg
{
    public partial class Form2 : Form
    {

        public Form1 form;
        public Form2()
        {
            InitializeComponent();
            groupBox1.Paint += new PaintEventHandler(groupBox1_Paint);
        }

        public void groupBox1_Paint(Object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black, 1);
            Point point1 = new Point(270, 60);
            Point point2 = new Point(300, 60);
            Rectangle rect=new Rectangle(10,20,690,270);
            e.Graphics.DrawRectangle(blackPen, rect);
         
        
            e.Graphics.DrawLine(blackPen, new Point(108, 20), new Point(108, 290));
            e.Graphics.DrawLine(blackPen, new Point(206, 20), new Point(206, 290));
            e.Graphics.DrawLine(blackPen, new Point(304, 20), new Point(304, 290));
            e.Graphics.DrawLine(blackPen, new Point(402, 20), new Point(402, 290));
            e.Graphics.DrawLine(blackPen, new Point(500, 20), new Point(500, 290));
            e.Graphics.DrawLine(blackPen, new Point(598, 20), new Point(598, 290));

            e.Graphics.DrawLine(blackPen, new Point(10, 62), new Point(700, 62));
            e.Graphics.DrawLine(blackPen, new Point(10, 100), new Point(700, 100));
            e.Graphics.DrawLine(blackPen, new Point(10, 138), new Point(700, 138));
            e.Graphics.DrawLine(blackPen, new Point(10, 176), new Point(700, 176));
            e.Graphics.DrawLine(blackPen, new Point(10, 214), new Point(700, 214));
            e.Graphics.DrawLine(blackPen, new Point(10, 252), new Point(700, 252));


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            
            this.form.Visible = true;
        }
    }
}
