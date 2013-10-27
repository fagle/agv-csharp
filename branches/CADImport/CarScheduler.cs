using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AGV
{
    class CarScheduler
    {
        private List<Car> carsRun = new List<Car>(20);
        private Track trackTogo = new Track();
        private List<Car> carsStandby = new List<Car>(20);

        public Track TrackToGo 
        {
            set { trackTogo = value; }
            get { return trackTogo; }
        }

        public CarScheduler() 
        {
            
        }

        public void demo(Car car) 
        {
            carsRun.Add(car);
        }

        public void addCar(Car car)
        {
            carsStandby.Add(car);
        }

        public void addTargetTrackToCar() 
        { 
        }

        public void run() 
        {
            Thread thread = new Thread(scheduleThread);
        }

        private void scheduleThread ()
        {
            while (true)
            {
                foreach(Car car in carsRun)
                {
                    car.run(trackTogo);
                }
            }
        }
    }
}
