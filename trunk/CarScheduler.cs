using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace AGV
{    
    public class CarScheduler
    {
        private Dictionary<byte, byte> mappingRoute = new Dictionary<byte, byte>(100);
        private Dictionary<byte,byte> carIDMappingTasklen = new Dictionary<byte,byte>(100);
        private SerialPort sp = new SerialPort();
        private CarTask carTask;
        private Queue<CarTask> [] carTaskQueues = new Queue<CarTask>[9];
        private byte callStyle = 0;
        private List<Car> carsRun = new List<Car>(20);
        private Track trackTogo = new Track();
        private Queue<Car> greenCarQueue = new Queue<Car>(20);
        private Queue<Car> redCarQueue = new Queue<Car>(20);
        private Queue<Car> pinkCarQueue = new Queue<Car>(20);
        private Queue<Car> goldCarQueue = new Queue<Car>(20);
        private Queue<Car>[] carQueues = new Queue<Car>[9];       
        private Thread thread ;
        private bool onLine = false;
        private Station targetStation = null, gStartStation = null, rStartStation = null, pStartStation = null, goStartStation = null;
        private Dictionary<string, Station> stationDic;
        private Dictionary<string, Track> trackDic;
        private AdjacencyList adjList;
        private List<Station> stationList1, stationList2, stationList3, stationList4;//Green Red Pink Gold
        private Mutex mutexStationTarget = new Mutex(false);
        
        public Track TrackToGo 
        {
            set { trackTogo = value; }
            get { return trackTogo; }
        }

        public SerialPort SP {
            get { return sp; }
        }

        public Dictionary<byte, byte> MappingRoute
        {
            set { mappingRoute = value; }
            get { return mappingRoute; }
        }

        public CarScheduler(Dictionary<string, Station> sDic,Dictionary<string, Track>tDic, AdjacencyList adj) 
        {
            stationDic = sDic;
            trackDic = tDic;
            adjList = adj;
            gStartStation = stationDic["F28"];
            stationList1 = new List<Station>(8);
            stationList1.Add(stationDic["F28"]);

            rStartStation = stationDic["F29"];
            stationList2 = new List<Station>(8);
            stationList2.Add(stationDic["F29"]);

            pStartStation = stationDic["F30"];
            stationList3 = new List<Station>(8);
            stationList3.Add(stationDic["F30"]);

            goStartStation = stationDic["F31"];
            stationList4 = new List<Station>(8);
            stationList4.Add(stationDic["F31"]);
            carQueues[5] = greenCarQueue;
            carQueues[6] = redCarQueue;
            carQueues[7] = pinkCarQueue;
            carQueues[8] = goldCarQueue;
            carTaskQueues[5] = new Queue<CarTask>(10);
            carTaskQueues[6] = new Queue<CarTask>(10);
            carTaskQueues[7] = new Queue<CarTask>(10);
            carTaskQueues[8] = new Queue<CarTask>(10);            
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

        public void demo(Car car) 
        {
            carsRun.Add(car);
        }

        public void addGreenCar(Car car)
        {
            greenCarQueue.Enqueue(car);
        }

        public void addRedCar(Car car)
        {
            redCarQueue.Enqueue(car);
        }

        public void addGoldCar(Car car)
        {
            goldCarQueue.Enqueue(car);
        }

        public void addPinkCar(Car car)
        {
            pinkCarQueue.Enqueue(car);
        }

        public Station TargetStation {
            get { return targetStation;}
            set { targetStation = value;}
        }

        public byte CallStyle {
            get { return callStyle;}
            set { callStyle = value;}
        }

        public void addTarget(Station targetStation,byte carType)
        {            
            switch (carType)
            {
                case 5:
                    carTask = new CarTask(stationDic["F28"], targetStation, stationDic["F28"], carType);                    
                    break;
                case 6:
                    carTask = new CarTask(stationDic["F29"], targetStation, stationDic["F29"], carType);
                    break;
                case 7:
                    carTask = new CarTask(stationDic["F30"], targetStation, stationDic["F30"], carType);
                    break;
                case 8:
                    carTask = new CarTask(stationDic["F31"], targetStation, stationDic["F31"], carType);
                    break;
            }
            if (carTask != null )
                if (carTaskQueues[carType].Count == 0)
                {
                    carTaskQueues[carType].Enqueue(carTask);
                    if (carQueues[carTaskQueues[carType].First().CarType].Count > 0)
                    {
                        MainGUI.RoadTableFrameHandler serialHander = new MainGUI.RoadTableFrameHandler();
                        if (carQueues[carTaskQueues[carType].First().CarType].First() != null)
                        {
                            byte taskLen = serialHander.planRoadTable(mappingRoute,carQueues[carTaskQueues[carType].First().CarType].First().CarID, carTask.StartStation, carTask.TargetStation, carTask.EndStation, this.adjList, this.sp, stationDic);
                            //byte carID = carQueues[carTaskQueues[carType].First().CarType].First().CarID;
                            //this.mappingRoute.Add(carQueues[carTaskQueues[carType].First().CarType].First().CarID,taskLen);
                            //byte carID = carQueues[carTaskQueues[carType].First().CarType].Peek().CarID;
                        }
                    }
                }
                else if (!carTaskQueues[carType].Contains(carTask))
                {
                    carTaskQueues[carType].Enqueue(carTask);
                }
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
                for (int i = 5; i <= 8; i++)
                {
                    if (carTaskQueues[i].Count > 0)
                    {
                        if (carQueues[carTaskQueues[i].First().CarType].Count > 0)
                        {
                            Thread t = new Thread(runCarThread);
                            t.Start(carTaskQueues[i].Dequeue());
                        }                        
                    }                    
                    Thread.Sleep(200);
                }
            }
        }
        
        private void runCarThread(object o)
        {
            CarTask carTask = (CarTask)o;
            Track trackTogo = new Track();
            int i;
            Queue<Car> carQueue = carQueues[carTask.CarType];
            
            if (carQueue.Count == 0)
                return;
            if (carTask.TargetStation == null || (carTask.StartStation.Equals(carTask.TargetStation)))
            {
                return;
            }
            
            List<Track> list1 = adjList.FindWay(adjList.Find(carTask.StartStation), adjList.Find(carTask.TargetStation));
            List<Track> list2 = adjList.FindWay(adjList.Find(carTask.TargetStation), adjList.Find(stationDic["F32"]));
            List<Track> list3 = adjList.FindWay(adjList.Find(stationDic["F32"]), adjList.Find(carTask.EndStation));
            Car car = null;
            if (carQueue.Count > 0)
            {
                car = carQueue.Dequeue();
            }
            else
                return;

            car.TargetStation = carTask.TargetStation;
            carTask.StartStation.OccupiedCar = null;

            while (car.WorkState)
            {
                Thread.Sleep(200);
            }

            for (i=0;i<list1.Count;i++)
            {
                Track t = list1[i];
                while (true)
                {
                    //Thread.Sleep(2000);
                    mutexStationTarget.WaitOne();
                    
                    if (stationDic[t.StartStation].CardID == car.posCard||0==i)
                    {
                        if (stationDic[t.EndStation].targeted == false)
                        {
                            stationDic[t.EndStation].targeted = true;
                            car.permitPass(sp);
                            if (car.lastStation != null)
                                car.lastStation.targeted = false;
                            car.lastStation = stationDic[t.EndStation];
                            mutexStationTarget.ReleaseMutex();
                            break;
                        }
                        else
                        {
                            car.forbidPass(sp);
                        }
                    }                    
                    mutexStationTarget.ReleaseMutex();
                    Thread.Sleep(200);
                }
                trackTogo.clear();
                trackTogo.TrackPointList.AddRange(t.TrackPointList);
                car.run(trackTogo);
                
            }
            car.RealState = CarState.CarStop;
            Thread.Sleep(200);
            car.WorkState = true;
            while (car.WorkState)
            {
                Thread.Sleep(200);
            }
                  
            for (i = 0; i < list2.Count; i++)
            {
                Track t = list2[i];
                while (true)
                {
                    mutexStationTarget.WaitOne();
                    if (stationDic[t.StartStation].CardID == car.posCard)
                    {
                        if (stationDic[t.EndStation].targeted == false)
                        {
                            stationDic[t.EndStation].targeted = true;
                            car.permitPass(sp);
                            if (car.lastStation != null)
                                car.lastStation.targeted = false;
                            car.lastStation = stationDic[t.EndStation];
                            mutexStationTarget.ReleaseMutex();
                            break;
                        }
                        else
                        {
                            car.forbidPass(sp);
                        }
                    }
                    mutexStationTarget.ReleaseMutex();
                    Thread.Sleep(200);
                }
                trackTogo.clear();
                trackTogo.TrackPointList.AddRange(t.TrackPointList);
                car.run(trackTogo);                
            }

            car.RealState = CarState.CarStop;
            Thread.Sleep(200);
            car.WorkState = true;
            while (car.WorkState)
            {
                Thread.Sleep(200);
            }

            for (i = 0; i < list3.Count; i++)
            {
                Track t = list3[i];
                while (true)
                {
                    mutexStationTarget.WaitOne();
                    if (stationDic[t.StartStation].CardID == car.posCard)
                    {
                        if (stationDic[t.EndStation].targeted == false)
                        {
                            stationDic[t.EndStation].targeted = true;
                            car.permitPass();
                            if (car.lastStation != null)
                                car.lastStation.targeted = false;
                            car.lastStation = stationDic[t.EndStation];
                            mutexStationTarget.ReleaseMutex();
                            break;
                        }
                        else
                        {
                            car.forbidPass();
                        }
                    }
                    mutexStationTarget.ReleaseMutex();
                    Thread.Sleep(200);
                }
                trackTogo.clear();
                trackTogo.TrackPointList.AddRange(t.TrackPointList);
                car.run(trackTogo);
            }
//            mappingRoute.Remove(car.CarID);
            car.WorkState = true;
            car.RealState = CarState.CarStop;
            car.TargetStation = null;
            carTask.EndStation.OccupiedCar = car;
            carQueue.Enqueue(car);
           
        }  
        
        public void stop()
        {
            onLine = false;
            thread.Abort();
            //thread2.Abort();
        }
    }

    public class CarTask
    {
        private Station startStation;
        private Station targetStation;
        private Station endStation;
        private byte carType;

        public CarTask(Station startStation, Station targetStation, Station endStation, byte carType)
        {
            this.startStation = startStation;
            this.targetStation = targetStation;
            this.endStation = endStation;
            this.carType = carType;            
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

        public byte CarType
        {
            get { return carType; }
        }

        public override bool Equals(object obj)
        {
            CarTask task = (CarTask)obj;
            if (task.carType == this.carType)
                if (task.endStation == this.endStation)
                    if (task.startStation == this.startStation)
                        if (task.targetStation == this.targetStation)
                            return true;
            return false;            
        }
    }
}
