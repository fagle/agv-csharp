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
        public LineEdit(int startX, int startY, int endX, int endY)
        {
            InitializeComponent();
            textBoxStartX.Text = startX.ToString();
            textBoxStartY.Text = startY.ToString();
            textBoxEndX.Text = endX.ToString();
            textBoxEndY.Text = endY.ToString();
            Canvas c = (Canvas)this.Owner;
            //this.KeyUp += c.CanvasRenewed_KeyUp;
            
        }
        public LineEdit(Point startPoint, Point endPoint):
            this(startPoint.X,startPoint.Y,endPoint.X,endPoint.Y)
        {            
        }        
    }
}
