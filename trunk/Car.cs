﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace AGV
{
    public class CarEventArgs
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

    public enum  CarState{
        CarStop,CarRun
    }

    public class CarInit
    {
        public string carName;
        public string carNumber;
        public string carStation;
        public string carID;
        public string carColor;

        public CarInit(string s1, string s2, string s3, string s4, string s5)
        {
            carName = s1;
            carNumber = s2;
            carStation = s3;
            carID = s4;
            carColor = s5;
        }
    }

    public class Car
    {
        private Station targetStation;
        private string name;
        private int speed = 0;
        private int defaultSpeed = 90;
        private Point position=new Point(300,300); 
        private Track trackToGo=new Track();
        private Label bindingLabel; 
        private byte carID;
        private CarState realState = CarState.CarStop;
        public Station StartStation;
        public delegate void CarPosEventHandler(object sender, CarEventArgs e);
        public event CarPosEventHandler carPosEvent;
        public Station lastStation = null;
        private bool workState = true;
        public byte posCard;
        public void permitPass()
        {
            if (realState == CarState.CarStop)
            {
                realState = CarState.CarRun;
            }
        }
        public void forbidPass()
        {
            if (realState == CarState.CarRun)
            {
                realState = CarState.CarStop;
            }
        }
        public Station TargetStation
        {
            set { targetStation = value; }
            get { return targetStation; }
        }
        public bool WorkState
        {
           get {return workState;}
            set { workState = value; }
        }
        public CarState RealState
        {
            set { realState = value; }
        }
        public CarState getRealState()
        {
            return realState;
        }
        public int DefaultSpeed 
        {
            get { return defaultSpeed; }
            set
            {
                if (value > 100)
                {
                    defaultSpeed = 99;
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
                    speed = 99;
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

        public Car(string name, Label label,byte carID) 
        {
            this.name = name;
            this.bindingLabel = label;  
            this.carID = carID;
        }

        public byte CarID
        {
            get { return carID; }
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
            position = new Point(p.X-8, -p.Y+8);
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
                else ;
                    //Thread.Sleep(1000 - 900 - this.speed);
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
