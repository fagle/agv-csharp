using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AGV
{
    public class CarTask
    {
        Car car;
    }

    public class CarScheduler
    {
        private controlMessage ctlMessage;
        private int callStyle = 0;
        private List<Car> carsRun = new List<Car>(20);
        private Track trackTogo = new Track();
        private List<Car> greenCarsStandby = new List<Car>(20);
        private List<Car> redCarsStandby = new List<Car>(20);
        private List<Car> pinkCarsStandby = new List<Car>(20);
        private List<Car> goldCarsStandby = new List<Car>(20);
        private Thread thread, thread2;
        private bool onLine = false;
        private Station targetStation = null, nextTargetStation = null, gStartStation = null, rStartStation = null, pStartStation = null, goStartStation = null;
        private Dictionary<string, Station> stationDic;
        private Dictionary<string, Track> trackDic;
        private AdjacencyList adjList;
        private List<Station> stationList1, stationList2, stationList3, stationList4;//Green Red Pink Gold

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
            gStartStation = stationDic["F29"];
            stationList1 = new List<Station>(8);
            stationList1.Add(stationDic["F29"]);

            rStartStation = stationDic["S2"];
            stationList2 = new List<Station>(8);
            stationList2.Add(stationDic["S2"]);

            pStartStation = stationDic["S3"];
            stationList3 = new List<Station>(8);
            stationList3.Add(stationDic["S3"]);

            goStartStation = stationDic["S10"];
            stationList4 = new List<Station>(8);
            stationList4.Add(stationDic["S10"]);
            //stationList1.Add(stationDic["S1"]);
            //stationList1.Add(stationDic["F28"]);
            
            //stationList1.Add(stationDic["F30"]);
            //stationList1.Add(stationDic["F31"]);            
        }

        public Station GStartStation
        {
            get { return gStartStation; }
        }

        public Station RStartStation
        {
            get { return rStartStation; }
        }

        public Station PStartStation
        {
            get { return pStartStation; }
        }

        public Station GOStartStation
        {
            get { return goStartStation; }
        }

        public int CallStyle
        {  
            set 
            {
                lock (this)
                {
                    callStyle = value;
                }
            }
            get { return callStyle; }
        }

        public void demo(Car car) 
        {
            carsRun.Add(car);
        }

        public void addGreenCar(Car car)
        {
            greenCarsStandby.Add(car);
        }

        public void addRedCar(Car car)
        {
            redCarsStandby.Add(car);
        }

        public void addGoldCar(Car car)
        {
            goldCarsStandby.Add(car);
        }

        public void addPinkCar(Car car)
        {
            pinkCarsStandby.Add(car);
        }

        public Station TargetStation 
        {
            set 
            {
                lock (this)
                {
                    nextTargetStation = value;
                }
            }
            get { return nextTargetStation; }//targetStation; }
        }

        public void addTargetTrackToCar() 
        { 
        }

        public void run() 
        {
            onLine = true;
            thread = new Thread(scheduleThread);
            thread.Start();
            //thread2 = new Thread(stationThread);
            //thread2.Start();
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

        private void scheduleThread()
        {
            //RoadTableFrameHandler serialHander = new RoadTableFrameHandler();
            //serialHander.serialEvent += serialHander.accessRoadTable;
            while (onLine)
            {
                #region
                if (nextTargetStation == null)
                    Thread.Sleep(200);
                else if (callStyle!=0)
                {
                    switch (callStyle)
                    {
                        case 1:
                            if (stationList1.First().OccupiedCar == null)
                            {
                                nextTargetStation = null;
                                continue;
                            }
                            ctlMessage = new controlMessage(stationDic["F29"], nextTargetStation, stationDic["F29"], greenCarsStandby);
                            //serialHander.accessRoadTable(stationDic["F29"], nextTargetStation, stationDic["F29"]);
                            nextTargetStation = null;
                            break;
                        case 2:
                            if (stationList2.First().OccupiedCar == null)
                            {
                                nextTargetStation = null;
                                continue;
                            }
                            ctlMessage = new controlMessage(stationDic["S2"], nextTargetStation, stationDic["S2"], redCarsStandby);
                            //serialHander.accessRoadTable(stationDic["S2"], nextTargetStation, stationDic["S2"]);
                            nextTargetStation = null;
                            break;
                        case 3:
                            if (stationList3.First().OccupiedCar == null)
                            {
                                nextTargetStation = null;
                                continue;
                            }
                            ctlMessage = new controlMessage(stationDic["S3"], nextTargetStation, stationDic["S3"], pinkCarsStandby);
                            //serialHander.accessRoadTable(stationDic["S3"], nextTargetStation, stationDic["S3"]);
                            nextTargetStation = null;
                            break;
                        case 4:
                            if (stationList4.First().OccupiedCar == null)
                            {
                                nextTargetStation = null;
                                continue;
                            }
                            ctlMessage = new controlMessage(stationDic["S10"], nextTargetStation, stationDic["S10"], goldCarsStandby);
                            //serialHander.accessRoadTable(stationDic["S10"], nextTargetStation, stationDic["S10"]);
                            nextTargetStation = null;
                            break;
                    }
                    //targetStation = nextTargetStation;
                    //nextTargetStation = null;
                    Thread t = new Thread(runCarTask);
                    t.Start(ctlMessage);
                }
                #endregion
            }
               
        }

        public List<CarTask> carTaskList;

        private void runCarTask(object o)
        {
            controlMessage ctlMessage = (controlMessage)o;
            Track trackTogo = new Track();
            if (ctlMessage.RelevantStandby.Count == 0)
                return;
            if (ctlMessage.TargetStation == null || (ctlMessage.StartStation.Equals(ctlMessage.TargetStation)))
            {
                return;
            }
            //if (targetStation.name == "S1")
                //targetStation = stationDic["F31"];
            List<Track> list1 = adjList.FindWay(adjList.Find(ctlMessage.StartStation), adjList.Find(ctlMessage.TargetStation));
            List<Track> list2 = adjList.FindWay(adjList.Find(ctlMessage.TargetStation), adjList.Find(ctlMessage.EndStation));
            for (int i = 0; i < list1.Count; ++i)
            {
                if (list1[i].CarAction != null)
                {
                    string station = list1[i].CarAction.Substring(0, 1);

                }
            }
            

            foreach (Track t in list1)
            {
                trackTogo.TrackPointList.AddRange(t.TrackPointList);
            }
            Car car = null;
            ctlMessage.StartStation.OccupiedCar = null;
            if (ctlMessage.RelevantStandby.Count!=0)
                car = ctlMessage.RelevantStandby.First();
            else
                return;
            if (car != null)
            {
                ctlMessage.RelevantStandby.Remove(car);
                car.run(trackTogo);
            }
            else
            {
                return;
            }
            trackTogo.clear();
            Thread.Sleep(3000);
            foreach (Track t in list2)
            {
                trackTogo.TrackPointList.AddRange(t.TrackPointList);
            }
            car.run(trackTogo);
            //while (stationDic["F31"].OccupiedCar != null)
            //{
            //    Thread.Sleep(100);
            //}
            ctlMessage.EndStation.OccupiedCar = car;
            ctlMessage.RelevantStandby.Add(car);
        }  
        
        public void stop()
        {
            onLine = false;
            thread.Abort();
            //thread2.Abort();
        }
    }

    public class controlMessage
    {
        private Station startStation;
        private Station targetStation;
        private Station endStation;
        private List<Car> relevantStandby;
        public controlMessage(Station startStation, Station targetStation, Station endStation, List<Car> relevantStandby)
        {
            this.startStation = startStation;
            this.targetStation = targetStation;
            this.endStation = endStation;
            this.relevantStandby = relevantStandby;

        }
        public Station StartStation
        {
            get{return startStation;}
        }
        public Station TargetStation
        {
            get { return targetStation; }
        }
        public Station EndStation
        {
            get { return endStation; }
        }
        public List<Car> RelevantStandby
        {
            get { return relevantStandby; }
        }
    }
}
