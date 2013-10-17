using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avg
{

    public class AdjacencyList
    {
        List<Vertex> items; //图的顶点集合

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

        private Vertex Find(Station item) //查找指定项并返回 //4
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

        public override string ToString() //仅用于测试 //6
        {   //打印每个节点和它的邻接点
            string s = string.Empty;
            foreach (Vertex v in items)
            {
                s += v.data.ToString() + ":";
                if (v.firstEdge != null)
                {
                    Node tmp = v.firstEdge;
                    while (tmp != null)
                    {
                        s += tmp.adjvex.data.ToString();
                        tmp = tmp.next;
                    }
                }
                s += "\r\n";
            }
            return s;
        }

        public static Track getTrack(Station from, Station to) //7
        {
            Track track = new Track();
            string path = "";
            path = from.name + "->" + to.name;
            switch (path)
            {
                case "W1->D":
                    track.AddLine(270, 60, 300, 60);
                    break;
                case "D->E":
                    track.AddArc(285, 60, 30, 30, 270, 90);
                    track.AddArc(315, 60, 30, 30, 180, -90);
                    track.AddLine(330, 90, 405, 90);
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
                track = getTrack(fromVer.data, toVer.data);
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

        private void InitVisited() //初始化visited标志
        {
            foreach (Vertex v in items)
            {
                v.visited = false; //全部置为false
            }
        }

        public void BFSTraverse() //广度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            BFS(items[0]); //从第一个顶点开始遍历
        }

        private void BFS(Vertex v) //使用队列进行广度优先遍历
        {   //创建一个队列
            Queue<Vertex> queue = new Queue<Vertex>();
            Console.Write(v.data + " "); //访问
            v.visited = true; //设置访问标志
            queue.Enqueue(v); //进队
            while (queue.Count > 0) //只要队不为空就循环
            {
                Vertex w = queue.Dequeue();
                Node node = w.firstEdge;
                while (node != null) //访问此顶点的所有邻接点
                {   //如果邻接点未被访问，则递归访问它的边
                    if (!node.adjvex.visited)
                    {
                        Console.Write(node.adjvex.data + " "); //访问
                        node.adjvex.visited = true; //设置访问标志
                        queue.Enqueue(node.adjvex); //进队
                    }
                    node = node.next; //访问下一个邻接点
                }
            }
        }

        public List<Track> FindWay(Vertex fromVer, Vertex toVer) //8
        {
            Queue<Vertex> discoveryQueue = new Queue<Vertex>();//探索队列
            Queue<Track> trackQueue = new Queue<Track>();
            List<Track> curList = new List<Track>();//当前执行链表
            List<Track> ansList = new List<Track>();//解链表
            Vertex v = fromVer;
            int length = 0;
            int ansLength = int.MaxValue;
            discoveryQueue.Enqueue(v);
            while (discoveryQueue.Count > 0)
            {
                Vertex w = discoveryQueue.Dequeue();
                if (trackQueue.Count > 0)
                    curList.Add(trackQueue.Dequeue());
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
                            ansList = curList;
                        }
                    }
                    else
                    {
                        curList.RemoveAt(curList.Count - 1);
                        if (length < ansLength)
                        {
                            discoveryQueue.Enqueue(node.adjvex);
                            trackQueue.Enqueue(node.track);
                        }
                    }
                    node = node.next;//访问下一个邻接点
                }
            }

            /*List<Station> stationList = new List<Station>();
            stationList.Add(fromVer.data);
            foreach (Track t in ansList)
            {
                stationList.Add(t.endStation);
            }*/
            return ansList;
        }

    }
}
