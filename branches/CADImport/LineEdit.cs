using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AGV
{
    public partial class LineEdit : Form
    {        
        public LineEdit(int startX, int startY, int endX, int endY, int indexNo)
        {
            InitializeComponent();
            textBoxStartX.Text = startX.ToString();
            textBoxStartY.Text = startY.ToString();
            textBoxEndX.Text = endX.ToString();
            textBoxEndY.Text = endY.ToString();
            textBoxNo.Text = indexNo.ToString();
            Canvas c = (Canvas)this.Owner;                       
        }
        public LineEdit(Point startPoint, Point endPoint,int indexNo):
            this(startPoint.X,startPoint.Y,endPoint.X,endPoint.Y,indexNo)
        {            
        }

        public Point StartPoint
        {
            get { return new Point(Convert.ToInt32(textBoxStartX.Text), -Convert.ToInt32(textBoxStartY.Text)); }
        }

        public Point EndPoint
        {
            get { return new Point(Convert.ToInt32(textBoxEndX.Text), -Convert.ToInt32(textBoxEndY.Text)); }
        }        

        private void button1_Click(object sender, EventArgs e)
        {
           /* int startX = Convert.ToInt32(textBoxEndX), startY = Convert.ToInt32(textBoxEndY);
            int endX = Convert.ToInt32(textBoxStartX), endY = Convert.ToInt32(textBoxStartY);*/
            string startX = textBoxEndX.Text, startY = textBoxEndY.Text;
            textBoxEndX.Text = textBoxStartX.Text;
            textBoxEndY.Text = textBoxStartY.Text;
            textBoxStartX.Text = startX;
            textBoxStartY.Text = startY;
        }        
    }
}
