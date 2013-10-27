using System;
using System.Drawing;
using System.Collections.Generic;
namespace Avg
{

    public partial class AdjacencyList
    {    
        public List<Vertex> items; //图的顶点集合

        public AdjacencyList() : this(10) { } //构造方法

        public AdjacencyList(int capacity) //指定容量的构造方法
        {
            items = new List<Vertex>(capacity);
        }

        public void AddVertex(Station item) //添加一个顶点 //1
        {   //不允许插入重复值
            if (Contains(item))
            {
                throw new ArgumentException("插入了重复顶点！");
            }
            items.Add(new Vertex(item));
        }

        public void AddEdge(Station from, Station to) //添加无向边 //2
        {
            Vertex fromVer = Find(from); //找到起始顶点
            if (fromVer == null)
            {
                throw new ArgumentException("头顶点并不存在！");
            }
            Vertex toVer = Find(to); //找到结束顶点
            if (toVer == null)
            {
                throw new ArgumentException("尾顶点并不存在！");
            }
            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(fromVer, toVer);
            AddDirectedEdge(toVer, fromVer);
        }

        public bool Contains(Station item) //查找图中是否包含某项 //3
        {
            foreach (Vertex v in items)
            {
                if (v.data.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public Vertex Find(Station item) //查找指定项并返回 //4
        {
            foreach (Vertex v in items)
            {
                if (v.data.Equals(item))
                {
                    return v;
                }
            }
            return null;
        }

        //添加有向边
        public void AddDirectedEdge(Vertex fromVer, Vertex toVer) //5
        {
            if (fromVer.firstEdge == null) //无邻接点时
            {
                fromVer.firstEdge = new Node(fromVer, toVer);
            }
            else
            {
                Node tmp, node = fromVer.firstEdge;
                do
                {   //检查是否添加了重复边
                    if (node.adjvex.data.Equals(toVer.data))
                    {
                        throw new ArgumentException("添加了重复的边！");
                    }
                    tmp = node;
                    node = node.next;
                } while (node != null);
                tmp.next = new Node(fromVer, toVer); //添加到链表未尾
            }
        }

        public static Track getTrack(Station from, Station to) 
        {
            Track track = new Track();
            string path = "";     
            path = from.name + "->" + to.name;
            switch (path)
            {
                case "S1->S3"://1
                    track.Name = null;
                    track.startStation = "S1";
                    track.endStation = "S3";
                    track.AddArc(150, 350, 100, 100, 180, 45);
                    break;
                case "S3->P1"://2
                    track.Name = null;
                    track.startStation = "S3";
                    track.endStation = "P1";
                    track.AddArc(150, 350, 100, 100, 225, 45);
                    break;
                case "P3->S4"://3
                    track.Name = null;
                    track.startStation = "P3";
                    track.endStation = "S4";
                    track.AddArc(150, 350, 100, 100, 90, 45);
                    break;
                case "S4->S1"://4
                    track.Name = null;
                    track.startStation = "S4";
                    track.endStation = "S1";
                    track.AddArc(150, 350, 100, 100, 135, 45);
                    break;
                case "S2->S3"://5
                    track.Name = null;
                    track.startStation = "S2";
                    track.endStation = "S3";
                    track.AddArc(130, 375, 50, 50, 180, 90);
                    break;
                case "S4->S2"://6
                    track.Name = null;
                    track.startStation = "S4";
                    track.endStation = "S2";
                    track.AddArc(130, 375, 50, 50, 90, 90);
                    break;
                case "P1->Z1"://7
                    track.Name = null;
                    track.startStation = "P1";
                    track.endStation = "Z1";
                    track.AddLine(200, 350, 250, 350);
                    break;
                case "Z1->Z3"://8
                    track.Name = null;
                    track.startStation = "Z1";
                    track.endStation = "Z3";
                    track.AddLine(250, 350, 300, 350);
                    break;
                case "Z3->K1"://9
                    track.Name = null;
                    track.startStation = "Z1";
                    track.endStation = "Z3";
                    track.AddLine(300, 350, 350, 350);
                    break;
                case "Z1->Z2"://10
                    track.Name = null;
                    track.startStation = "Z1";
                    track.endStation = "Z2";
                    track.AddArc(250, 325, 50, 50, 180, 90);
                    break;
                case "Z2->Z3"://11
                    track.Name = null;
                    track.startStation = "Z2";
                    track.endStation = "Z3";
                    track.AddArc(250, 325, 50, 50, 270, 90);
                    break;
                case "K1->F1"://12
                    track.Name = "K1S";
                    track.startStation = "K1";
                    track.endStation = "F1";
                    track.AddLine(350, 350, 450, 350);
                    break;
                case "K1->T1"://13
                    track.Name = "K1L";
                    track.startStation = "K1";
                    track.endStation = "T1";
                    track.AddArc(350, 300, 100, 100, 180, 90);
                    break;
                case "T1->F1"://14
                    track.Name = "T1";
                    track.startStation = "T1";
                    track.endStation = "F1";
                    track.AddArc(350, 300, 100, 100, 270, 90);
                    break;
                case "F1->P2"://15
                    track.Name = null;
                    track.startStation = "F1";
                    track.endStation = "P2";
                    track.AddLine(450, 350, 600, 350);
                    break;
                case "P2->P4"://16
                    track.Name = null;
                    track.startStation = "P2";
                    track.endStation = "P4";
                    track.AddArc(550, 350, 100, 100, 270, 180);
                    break;
                case "P4->Z6"://17
                    track.Name = null;
                    track.startStation = "P4";
                    track.endStation = "Z6";
                    track.AddLine(600, 450, 550, 450);
                    break;
                case "Z6->Z4"://18
                    track.Name = null;
                    track.startStation = "Z6";
                    track.endStation = "Z4";
                    track.AddLine(550, 450, 500, 450);
                    break;
                case "Z4->F2"://19
                    track.Name = null;
                    track.startStation = "Z4";
                    track.endStation = "F2";
                    track.AddLine(500, 450, 450, 450);
                    break;
                case "Z6->Z5"://20
                    track.Name = null;
                    track.startStation = "Z6";
                    track.endStation = "Z5";
                    track.AddArc(500, 425, 50, 50, 0, 90);
                    break;
                case "Z5->Z4"://21
                    track.Name = null;
                    track.startStation = "Z5";
                    track.endStation = "Z4";
                    track.AddArc(500, 425, 50, 50, 90, 90);
                    break;
                case "F2->K2"://22
                    track.Name = "F2S";
                    track.startStation = "F2";
                    track.endStation = "K2";
                    track.AddLine(450, 450, 350, 450);
                    break;
                case "F2->T2"://23
                    track.Name ="F2L";
                    track.startStation = "F2";
                    track.endStation = "T2";
                    track.AddArc(350, 400, 100, 100, 0, 90);
                    break;                                   
                case "T2->K2"://24
                    track.Name = "T2";
                    track.startStation = "T2";
                    track.endStation = "K2";
                    track.AddArc(350, 400, 100, 100, 90, 90);
                    break;       
                case "K2->P3"://25
                    track.Name = null;
                    track.startStation = "K2";
                    track.endStation = "P3";
                    track.AddLine(350, 450, 200, 450);
                    break;
                default:
                    break;

            }
            return track;
        }

        //嵌套类，表示链表中的表结点
        public class Node
        {
            public Vertex adjvex; //邻接点域
            public Track track;//边信息
            public Node next; //下一个邻接点指针域
            public Node(Vertex fromVer, Vertex toVer)
            {
                adjvex = toVer;
                try
                {
                    track = getTrack(fromVer.data, toVer.data);
                }
                catch (Exception w)
                {
                    System.Console.WriteLine(w.Message);
                }
            }
        }

        //嵌套类，表示存放于数组中的表头结点
        public class Vertex
        {
            public Station data; //数据
            public Node firstEdge; //邻接点链表头指针
            public Boolean visited; //访问标志,遍历时使用
            public Vertex(Station value) //构造方法
            {
                data = value;
            }
        }
        public List<Track> FindWay(Vertex fromVer, Vertex toVer)
        {
            Queue<Vertex> discoveryQueue = new Queue<Vertex>();//探索队列
            Queue<List<Track>> trackQueue = new Queue<List<Track>>();
            List<Track> curList = new List<Track>();//当前执行链表
            List<Track> ansList = new List<Track>();//解链表
            Vertex v = fromVer;
            int length = 0;
            int ansLength = int.MaxValue;
            discoveryQueue.Enqueue(v);
            while (discoveryQueue.Count > 0)
            {
                curList.Clear();
                Vertex w = discoveryQueue.Dequeue();
                if (trackQueue.Count > 0)
                    curList.AddRange(trackQueue.Dequeue());
                if (curList.Count > 200)
                    break;
                Node node = w.firstEdge;
                while (node != null)
                {
                    curList.Add(node.track);
                    length = 0;
                    foreach (Track t in curList)
                    {
                        length += t.Length;
                    }
                    if (node.adjvex.Equals(toVer))
                    {
                        if (length < ansLength)
                        {
                            ansLength = length;
                            ansList = new List<Track>(curList);
                            curList.RemoveAt(curList.Count - 1);
                        }
                    }
                    else
                    {
                        if (length < ansLength && node.adjvex.firstEdge != null && !node.adjvex.Equals(fromVer))
                        {
                            discoveryQueue.Enqueue(node.adjvex);
                            trackQueue.Enqueue(new List<Track>(curList));
                        }
                        curList.RemoveAt(curList.Count - 1);
                    }
                    node = node.next;//访问下一个邻接点
                }
            }
            return ansList;
        }

    }

    

    
}

