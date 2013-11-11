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
        private Thread thread, thread2;
        private bool onLine = false;
        private Station targetStation = null, nextTargetStation=null, startStation = null;
        private Dictionary<string, Station> stationDic;
        private Dictionary<string, Track> trackDic;
        private AdjacencyList adjList;
        private List<Station> stationList1;

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
            startStation = stationDic["S1"];
            stationList1 = new List<Station>(8);
            stationList1.Add(stationDic["S1"]);
            stationList1.Add(stationDic["F28"]);
            stationList1.Add(stationDic["F29"]);
            stationList1.Add(stationDic["F30"]);
            stationList1.Add(stationDic["F31"]);            
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
            set { nextTargetStation = value; }
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
            thread2 = new Thread(stationThread);
            thread2.Start();
        }

        private void stationThread()
        {
            List<Track> list;
            while (true)
            {
                for (int i = 0; i < stationList1.Count;i++ )
                {
                    Station s = stationList1[i];
                    if ((s.OccupiedCar != null) && (s.Next != ""))
                    {
                        if (stationDic[s.Next].OccupiedCar == null)
                        {
                            list = adjList.FindWay(adjList.Find(s), adjList.Find(stationDic[s.Next]));
                            Track track = new Track();
                            foreach (Track t in list)
                                track.TrackPointList.AddRange(t.TrackPointList);
                            s.OccupiedCar.run(track);
                            stationDic[s.Next].OccupiedCar = s.OccupiedCar;
                            s.OccupiedCar = null;
                        }
                    }
                }
                Thread.Sleep(500);
            }

        }

        private void runCarTask(object o)
        {            
            Station targetStation = (Station)o;
            Track trackTogo = new Track();
            if (carsStandby.Count == 0)
                return;
            if (targetStation == null || (startStation.Equals(targetStation)))
            {
                return;
            }
            if (targetStation.name == "S1")
                targetStation = stationDic["F31"];
            List<Track> list1 = adjList.FindWay(adjList.Find(startStation), adjList.Find(targetStation));
            List<Track> list2 = adjList.FindWay(adjList.Find(targetStation), adjList.Find(stationDic["F31"]));
            foreach (Track t in list1)
            {
                trackTogo.TrackPointList.AddRange(t.TrackPointList);
            }

            startStation.OccupiedCar = null;
            
            Car car = carsStandby.First();
            carsStandby.Remove(car);
            car.run(trackTogo);
            trackTogo.clear();
            Thread.Sleep(3000);
            foreach (Track t in list2)
            {
                trackTogo.TrackPointList.AddRange(t.TrackPointList);
            }
            car.run(trackTogo);
            while (stationDic["F31"].OccupiedCar != null)
            {
                Thread.Sleep(100);
            }
            stationDic["F31"].OccupiedCar = car;
            carsStandby.Add(car);
        }
        
        private void scheduleThread ()
        {

            while (onLine)
            {
                if (nextTargetStation == null)
                    Thread.Sleep(200);
                else if(stationList1.First().OccupiedCar!=null)
                {
                    targetStation = nextTargetStation;
                    nextTargetStation = null;
                    Thread t = new Thread(runCarTask);
                    t.Start(targetStation);
                }
            }
        }
        public void stop()
        {
            onLine = false;
            thread.Abort();
            thread2.Abort();
        }
    }
}
