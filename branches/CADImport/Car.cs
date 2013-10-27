using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace AGV
{
    class Car
    {
        private string name;
        private int speed = 0;
        private int defaultSpeed = 1;
        private Point position=new Point(300,300); 
        private Track trackToGo;

        public int DefaultSpeed 
        {
            get { return defaultSpeed; }
            set
            {
                if (value > 1000)
                {
                    defaultSpeed = 1000;
                }else{
                    defaultSpeed = value;
                }
            }
        }

        public Car(string name) { 
            this.name=name;
        }

        public void stop() 
        {
            speed = 0;
        }
        private void run()
        {
            foreach (Point p in trackToGo.TrackPointList)
            {
                if (speed == 0)
                    break;
                else
                    Thread.Sleep(1000 - defaultSpeed);
            }
        }
        public void run(Track track) 
        {
            trackToGo = track;
        }

        public void run(Line line) 
        {
            if (line == null)
                return;
            trackToGo.AddLine(line);
            
        }
        public void run(Arc arc) {
            trackToGo.AddArc(arc);
        }
    }
}
