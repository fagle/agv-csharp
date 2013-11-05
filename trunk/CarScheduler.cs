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
        private Thread thread;
        private bool onLine = false;
        private Station targetStation = null, startStation = null;
        private Dictionary<string, Station> stationDic;
        private Dictionary<string, Track> trackDic;
        private AdjacencyList adjList;

        public Track TrackToGo 
        {
            set { trackTogo = value; }
            get { return trackTogo; }
        }

        public CarScheduler(Dictionary<string, Station> sDic,Dictionary<string, Track>tDic, AdjacencyList adj) 
        {
            stationDic = sDic;
            trackDic = tDic;
            adjList = adj;
            startStation = stationDic["S0"];
        }

        public void demo(Car car) 
        {
            carsRun.Add(car);
        }

        public void addCar(Car car)
        {
            carsStandby.Add(car);
        }

        public Station TargetStation 
        {
            set { targetStation = value; }
            get { return targetStation; }
        }

        public void addTargetTrackToCar() 
        { 
        }

        public void run() 
        {
            onLine = true;
            thread = new Thread(scheduleThread);
            thread.Start();
        }

        private void scheduleThread ()
        {
            while (onLine)
            {
                trackTogo.clear();
                if (targetStation == null || (startStation.Equals(targetStation)))
                {
                    Thread.Sleep(100);
                    continue;
                }
                List<Track> list = adjList.FindWay(adjList.Find(startStation), adjList.Find(targetStation));
                
                foreach (Track t in list)
                {
                    trackTogo.TrackPointList.AddRange(t.TrackPointList);
                }

                foreach(Car car in carsRun)
                {
                    car.run(trackTogo);
                }
                startStation = targetStation;
            }
        }
        public void stop()
        {
            onLine = false;
            thread.Abort();
        }
    }
}
