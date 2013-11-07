using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace AGV
{
    class CarEventArgs
    {
        private string carName;
        private Point position;
        private Label bindingLabel;
        public string CarName
        {
            get { return carName; }
        }
        public Point Position
        {
            get { return position; }
        }

        public Label BingdingLabel 
        {
            get { return bindingLabel; }
            set { bindingLabel = value; }
        }

        public CarEventArgs(string carName, Point pos,Label label)
        {
            this.carName = carName;
            this.position = pos;
            this.bindingLabel = label;
        }
    }

    class Car
    {
        private string name;
        private int speed = 0;
        private int defaultSpeed = 99;
        private Point position=new Point(300,300); 
        private Track trackToGo=new Track();
        private Label bindingLabel;
        public delegate void CarPosEventHandler(object sender, CarEventArgs e);
        public event CarPosEventHandler carPosEvent;

        public int DefaultSpeed 
        {
            get { return defaultSpeed; }
            set
            {
                if (value > 100)
                {
                    defaultSpeed = 100;
                }else{
                    defaultSpeed = value;
                }
            }
        }
        public int Speed
        {
            get { return speed; }
            set
            {
                if (value > 100)
                {
                    speed = 100;
                }
                else if (value < 0)
                {
                    speed = 0;
                }
                else
                    speed = value;
            }
        }

       /* public Car(string name) { 
            this.name=name;           
        }*/

        public Car(string name, Label label) 
        {
            this.name = name;
            this.bindingLabel = label;
        }

        public void stop() 
        {
            speed = 0;
        }

        public Point Position 
        {
            get { return position ;}
            set { position = value; }
        }

        public void setPosition(Point p)
        {
            position = new Point(p.X, -p.Y);
            if (carPosEvent != null)
                carPosEvent(this, new CarEventArgs(name, position, bindingLabel));   
        }

        private void run(int speed)
        {
            this.speed = speed;
            if (trackToGo.TrackPointList.Count == 0)
                return;
            for (int i=0;i<trackToGo.TrackPointList.Count;i++)
            {
                Point p = trackToGo.TrackPointList[i];
                setPosition(p);
                if (this.speed == 0)
                    break;
                else
                    Thread.Sleep(1000 - 900 - this.speed);
                if (trackToGo.TrackPointList.Count == 0)
                    break;
            }
        }
        
        private void run()
        {
            run(defaultSpeed);
        }

        public void run(Track track) 
        {
            trackToGo = track;
            run();
        }

        public void run(Line line) 
        {
            if (line == null)
                return;
            trackToGo.AddLine(line);
            run();
        }
        public void run(Arc arc) {
            trackToGo.AddArc(arc);
            run();
        }
    }
}
