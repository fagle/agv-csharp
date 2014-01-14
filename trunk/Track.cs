using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Collections;

namespace AGV
{
    public partial class Form1 : Form
    {
        enum eSerialSate { SerialOn, SerialOff };
        static int i = 0;
        //static int i1 = 0;
        static int callNum = 0;
        Thread readThread;
        private eSerialSate serialState = eSerialSate.SerialOff;
        private System.Timers.Timer aTimer1;//计时器
        private System.Timers.Timer aTimer2;//计时器
        Graphics dc, dc1, dc2;
        Point[] CurvePoint, CurvePoint1, CurvePoint2;//画图
        Track agvTrack = new Track();//重绘图形 //保存点到点之间路径 相邻点的路径
        Track path = new Track();
        private static int index1 = 0;//像素索引
        private static int index2 = 0;//像素索引
        AdjacencyList Adj = new AdjacencyList(11);
        #region
        Station T1;
        Station K1;
        Station F1;
        Station T2;
        Station K2;
        Station F2;
        Station Start;
        Station P1;
        Station P2;
        Station P3;
        Station P4;
        #endregion
        public Form1()
        {
            InitializeComponent();

            comboBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox.SelectedItem = comboBox.Items[0];
            InitPort();
            CurvePoint1 = new Point[0];
            #region
            Start = new Station("W0", 150, 400);
            T1 = new Station("T1", 400, 300);
            K1 = new Station("K1", 350, 350);
            F1 = new Station("F1", 450, 350);
            T2 = new Station("T2", 400, 500);
            K2 = new Station("K2", 350, 450);
            F2 = new Station("F2", 450, 450);
            P1 = new Station("P1", 200, 350);
            P2 = new Station("P2", 600, 350);
            P3 = new Station("P3", 200, 450);
            P4 = new Station("P4", 600, 450);
            Adj.AddVertex(Start);
            Adj.AddVertex(T1);
            Adj.AddVertex(K1);
            Adj.AddVertex(F1);
            Adj.AddVertex(T2);
            Adj.AddVertex(K2);
            Adj.AddVertex(F2);
            Adj.AddVertex(P1);
            Adj.AddVertex(P2);
            Adj.AddVertex(P3);
            Adj.AddVertex(P4);
            Adj.AddDirectedEdge(Adj.Find(Start), Adj.Find(P1));//1
            Adj.AddDirectedEdge(Adj.Find(P1), Adj.Find(K1));//2
            Adj.AddDirectedEdge(Adj.Find(K1), Adj.Find(F1));//3
            Adj.AddDirectedEdge(Adj.Find(K1), Adj.Find(T1));//4
            Adj.AddDirectedEdge(Adj.Find(T1), Adj.Find(F1));//5
            Adj.AddDirectedEdge(Adj.Find(F1), Adj.Find(P2));//6
            Adj.AddDirectedEdge(Adj.Find(P2), Adj.Find(P4));//7
            Adj.AddDirectedEdge(Adj.Find(P4), Adj.Find(F2));//8
            Adj.AddDirectedEdge(Adj.Find(F2), Adj.Find(K2));//9
            Adj.AddDirectedEdge(Adj.Find(F2), Adj.Find(T2));//10
            Adj.AddDirectedEdge(Adj.Find(T2), Adj.Find(K2));//11
            Adj.AddDirectedEdge(Adj.Find(K2), Adj.Find(P3));//12
            Adj.AddDirectedEdge(Adj.Find(P3), Adj.Find(Start));//13
            #endregion
        }

        public void ChangeData1()
        {
            //textBox1.Text = textBox13.Text;
            textBox1.Text = "W0";
            textBox7.Text = "等待";
            textBox13.Text = "";
            index1 = 0;
        }

        public void ChangeData2()
        {
            textBox2.Text = textBox14.Text;
            textBox8.Text = "等待";
            textBox14.Text = "";
            index2 = 0;
        }

        public void UpdateLable1(int index1)
        {
            Point p = new Point();
            if (index1 >= CurvePoint1.Length - 1)
                return;
            p = CurvePoint1[index1];
            p.X -= 6;
            p.Y -= 6;
            label_car1.Location = p;

        }
        
        public void UpdateLable2(int index2)
        {
            Point p = new Point();
            if (index2 >= CurvePoint2.Length - 1)
                return;
            p = CurvePoint2[index2];
            p.X -= 6;
            p.Y -= 6;
            label_car2.Location = p;

        }
        
        private void OnTimedEvent1(object source, ElapsedEventArgs e)
        {
            try
            {
                if (CurvePoint1 == null || CurvePoint1.Length == 0 )
                {
                    return;
                }
                aTimer1.Enabled = false;

                index1 += 1;

                if (index1 == CurvePoint1.Length)
                {
                    Invoke(new ChangeDataDelegate(ChangeData1));
                    index1 = 0;
                    CurvePoint1 = null;                    
                    return;
                }               
                Invoke(new UpdateLableDelegate1(UpdateLable1), new object[] { index1 });

                //this.Invalidate();
                aTimer1.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            try
            {
                if (CurvePoint2 == null || CurvePoint2.Length == 0 )
                {
                    return;
                }
                aTimer2.Enabled = false;

                index2 += 1;

                if (index2 == CurvePoint2.Length)
                {
                    Invoke(new ChangeDataDelegate(ChangeData2));
                    CurvePoint2 = null;
                    index2 = 0;
                    return;
                }


                Invoke(new UpdateLableDelegate2(UpdateLable2), new object[] { index2 });

                //this.Invalidate();
                aTimer2.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        protected delegate void UpdateLableDelegate1(int index1);
        protected delegate void UpdateLableDelegate2(int index1);
        protected delegate void ChangeDataDelegate();
        //protected delegate void ChangeDataDelegate2();

        protected override void OnPaint(PaintEventArgs e)//重绘
        {
            base.OnPaint(e);
            dc = e.Graphics;
            Pen blkPen = new Pen(Color.Black, 1);

            agvTrack.AddLine(200, 350, 600, 350);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddLine(200, 450, 600, 450);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddArc(150, 350, 100, 100, 180, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddArc(150, 350, 100, 100, 90, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddArc(550, 350, 100, 100, 90, -180);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddArc(350, 300, 100, 100, 180, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddArc(350, 300, 100, 100, 0, -90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddArc(350, 400, 100, 100, 90, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();

            agvTrack.AddArc(350, 400, 100, 100, 0, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.TrackPointList.Clear();
        }

        private void button8_Click(object sender, EventArgs e)//初始化
        {
            button8.ForeColor = Color.Gray;
            textBox1.Text = "W0";
            textBox2.Text = "W1";
            textBox7.Text = "等待";
            textBox8.Text = "等待";
            textBox9.Text = "";
            textBox9.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)//启动
        {


            Station temp1 = new Station();
            Station temp2 = new Station();
            string str1;
            string str2;
            string str3;
            string str4;
            if (button7.Text == "AGV1")
            {
                aTimer1 = new System.Timers.Timer(1000000);
                //aTimer1.BeginInit();
                aTimer1.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
                aTimer1.Interval = 10;
                aTimer1.AutoReset = true;
                aTimer1.Enabled = true;//只有Timer类 可以触发自身事件
                if (textBox7.Text == "等待")
                {
                    CurvePoint1 = null;
                    index1 = 0;
                    textBox7.Text = "工作";
                    str1 = textBox1.Text;
                    str2 = textBox13.Text;
                    foreach (AdjacencyList.Vertex ver in Adj.items)
                    {
                        if (str1 == ver.data.name)
                        {
                            temp1 = ver.data;
                            break;
                        }
                    }
                    foreach (AdjacencyList.Vertex ver in Adj.items)
                    {
                        if (str2 == ver.data.name)
                        {
                            temp2 = ver.data;
                            break;
                        }
                    }
                    List<Track> shortPath = Adj.FindWay(Adj.Find(temp1), Adj.Find(temp2));
                    if (temp2 != Start)
                    {
                        shortPath.AddRange(Adj.FindWay(Adj.Find(temp2), Adj.Find(Start)));
                    }
                    List<Point> temp = new List<Point>();
                    List<Point> TempAll = new List<Point>();
                    foreach (Track m in shortPath)
                    {
                        temp = m.TrackPointList;
                        TempAll.AddRange(temp);
                    }
                    CurvePoint1 = TempAll.ToArray();
                    index1 = 0;
                    aTimer1.Start();
                    Pen blkPen = new Pen(Color.Black, 1);
                    dc1 = this.CreateGraphics();
                    try
                    {
                        dc1.DrawCurve(blkPen, CurvePoint1, 0.1f);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }
            }

            if (button7.Text == "AGV2")
            {
                aTimer2 = new System.Timers.Timer(10000);
                aTimer2.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
                aTimer2.Interval = 10;
                aTimer2.AutoReset = true;
                aTimer2.Enabled = true;//只有Timer类 可以触发自身事件
                if (textBox8.Text == "等待")
                {
                    textBox8.Text = "工作";
                    str3 = textBox2.Text;
                    str4 = textBox14.Text;
                    foreach (AdjacencyList.Vertex ver in Adj.items)
                    {
                        if (str3 == ver.data.name)
                        {
                            temp1 = ver.data;
                        }
                    }
                    foreach (AdjacencyList.Vertex ver in Adj.items)
                    {
                        if (str4 == ver.data.name)
                        {
                            temp2 = ver.data;
                        }
                    }
                    List<Track> shortPath = Adj.FindWay(Adj.Find(temp1), Adj.Find(temp2));
                    List<Point> temp = new List<Point>();
                    List<Point> TempAll = new List<Point>();
                    foreach (Track m in shortPath)
                    {
                        temp = m.TrackPointList;
                        TempAll.AddRange(temp);
                    }
                    CurvePoint2 = TempAll.ToArray();
                    index2 = 0;
                    Pen blkPen = new Pen(Color.Black, 1);
                    dc2 = this.CreateGraphics();
                    try
                    {
                        dc2.DrawCurve(blkPen, CurvePoint1, 0.1f);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)//AGV1
        {
            //textBox7.Text = "工作";
            button7.Text = "AGV1";
        }

        private void button10_Click(object sender, EventArgs e)//停止
        {
            if (button7.Text == "AGV1")
            {
                aTimer1.Elapsed -= new ElapsedEventHandler(OnTimedEvent1);
                aTimer1.Enabled = false;
            }
            if (button7.Text == "AGV2")
            {
                aTimer2.Elapsed -= new ElapsedEventHandler(OnTimedEvent2);
                aTimer2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)//AGV2
        {
            //textBox8.Text = "工作";
            button7.Text = "AGV2";
        }

        public int InitPort()
        {
            serialPort1.PortName = comboBox.SelectedItem.ToString();
            serialPort1.BaudRate = 115200;
            serialPort1.DataBits = 8;
            // serialPort1.WriteTimeout = 500;
            //serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);
            return 0;
        }
        private string startPositon;
        /*
        public void agvSerialRemoteCall(object sender, SerialEventArgs e)
        {
            ++callNum;
            if (callNum == 1)
            {
                Invoke(new niming(button8.PerformClick));//初始化
            }
            Invoke(new niming(button1.PerformClick));//AGV1
            try
            {
                switch (e.Message)
                {
                    case "toT1":
                        Invoke(new niming(Station1.PerformClick));
                        Invoke(new niming(button9.PerformClick));
                        break;
                    case "toT2":
                        Invoke(new niming(Station2.PerformClick));
                        Invoke(new niming(button9.PerformClick));
                        break;
                    case "reF2":
                        //aTimer1.Elapsed -= new ElapsedEventHandler(OnTimedEvent1);
                        startPositon = "F2";
                        Invoke(new niming(SetLocation1));
                        Invoke(new niming(SetStartStation1));
                        Invoke(new niming(StartStation.PerformClick));
                        Invoke(new niming(button9.PerformClick));
                        break;
                    case "reT1":
                        startPositon = "T1";
                        Invoke(new niming(SetLocation1));
                        Invoke(new niming(SetStartStation1));
                        Invoke(new niming(StartStation.PerformClick));
                        Invoke(new niming(button9.PerformClick));
                        break;

                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }
        */
        public void ReadPort()
        {
            SerialHandler serialHander = new SerialHandler();
            //serialHander.serialEvent += agvSerialRemoteCall;
            while (true)
            {
                if (serialPort1.IsOpen)
                {
                    if (serialPort1.BytesToRead == 0)
                        Thread.Sleep(100);
                    try
                    {

                        byte b = (byte)serialPort1.ReadByte();
                        serialHander.handleOneByte(b);
                        //String SerialIn = System.Text.Encoding.ASCII.GetString(readBuffer, 0, count);
                        //if (count != 0) 
                        //{ 
                        //MessageBox.Show(SerialIn); 
                        //}
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                /*else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    Thread.Sleep(waitTime);
                }*/
            }
        }

        delegate void niming();

        void SetLocation1()
        {
            label_car1.Location = new Point(447, 450);
        }

        void SetStartStation1()
        {
            textBox1.Text = startPositon;
            textBox7.Text = "等待";
        }

        private void DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            ++callNum;
            int n = serialPort1.BytesToRead;
            byte[] buf = null;
            buf = new byte[n];
            serialPort1.Read(buf, 0, n);//读取缓冲数据 
            if (buf[2] == 0xF2)//车子错位，重新规划回起点
            {
                if (buf[7] == 0x04)
                {
                    if (buf[8] == 0x0D && buf[9] == 0xB1 && buf[10] == 0xDB)
                    {
                        aTimer1.Elapsed -= new ElapsedEventHandler(OnTimedEvent1);
                        Invoke(new niming(SetLocation1));
                        Invoke(new niming(SetStartStation1));
                        Invoke(new niming(StartStation.PerformClick));
                        Invoke(new niming(button9.PerformClick));
                    }
                }
            }
            if (buf[2] == 0xF6)//呼叫器呼叫
            {
                if (callNum == 1)
                {
                    Invoke(new niming(button8.PerformClick));//初始化
                }
                Invoke(new niming(button1.PerformClick));//AGV1
                if (buf[7] == 0x0c && buf[8] == 0x38 && buf[9] == 0xca)//T1呼叫
                {
                    Invoke(new niming(Station1.PerformClick));
                }
                if (buf[7] == 0x0e && buf[8] == 0x2a && buf[9] == 0xe9)//T2呼叫
                {
                    Invoke(new niming(Station2.PerformClick));
                }
                try
                {
                    Invoke(new niming(button9.PerformClick));
                }
                catch (Exception x) {
                    Console.WriteLine(x.Message);
                }
            }
        }

        private void button39_Click(object sender, EventArgs e)//打开串口
        {
            if (serialState == eSerialSate.SerialOff)
            {
                try
                {
                    //serialPort1.WriteTimeout = 500;
                    //serialPort1.ReadTimeout = 500;
                    InitPort();
                    serialPort1.Open();
                    serialState = eSerialSate.SerialOn;
                    readThread = new Thread(ReadPort);
                    readThread.Name = "read thread";
                    button39.Text = "关闭串口";
                    readThread.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            else
            {
                try
                {
                    serialState = eSerialSate.SerialOff;
                    //readThread.Join();//等待线程结束会出现线程死掉的情况，不知道为什么
                    button39.Text = "打开串口";
                    if (serialPort1.IsOpen)
                        serialPort1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)//Send
        {
           
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            if (textBox20.TextLength >= textBox20.MaxLength - 1)
            {

                textBox20.Text = "";
            }
        }

        private void Station1_Click(object sender, EventArgs e)
        {
            if (button7.Text == "AGV1")
            {
                textBox13.Text = "T1";
                string str1;
                string str2 = "T1";
                str1 = textBox1.Text;
                List<Track> shortPath1 = ShortPath(str1, str2);
                List<Track> shortPath2 = ShortPath(str2, "W0");
                arrangeWays(shortPath1, shortPath2);
            }
        }

        private void Station2_Click(object sender, EventArgs e)
        {
            if (button7.Text == "AGV1")
            {
                textBox13.Text = "T2";
                string str1;
                string str2 = "T2";
                str1 = textBox1.Text;
                List<Track> shortPath1 = ShortPath(str1, str2);
                List<Track> shortPath2 = ShortPath(str2, "W0");
                arrangeWays(shortPath1,shortPath2);
            }
        }
        #region
        //private void Station3_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T3";
        //        string str1;
        //        string str2 = "T3";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 3);
        //    }
        //}

        //private void Station4_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T4";
        //        string str1;
        //        string str2 = "T4";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 4);
        //    }
        //}

        //private void Station5_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T5";
        //        string str1;
        //        string str2 = "T5";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 5);
        //    }
        //}

        //private void Station6_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T6";
        //        string str1;
        //        string str2 = "T6";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 6);
        //    }
        //}

        //private void Station7_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T7";
        //        string str1;
        //        string str2 = "T7";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 7);
        //    }
        //}

        //private void Station8_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T8";
        //        string str1;
        //        string str2 = "T8";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 8);
        //    }
        //}

        //private void Station9_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T9";
        //        string str1;
        //        string str2 = "T9";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 9);
        //    }
        //}

        //private void Station10_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T10";
        //        string str1;
        //        string str2 = "T10";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 10);
        //    }
        //}

        //private void Station11_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T11";
        //        string str1;
        //        string str2 = "T11";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 11);
        //    }
        //}

        //private void Station12_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T12";
        //        string str1;
        //        string str2 = "T12";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 12);
        //    }
        //}

        //private void Station13_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T13";
        //        string str1;
        //        string str2 = "T13";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 13);
        //    }
        //}

        //private void Station14_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T14";
        //        string str1;
        //        string str2 = "T14";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 14);
        //    }
        //}

        //private void Station15_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T15";
        //        string str1;
        //        string str2 = "T15";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1,str2);
        //        SendData(shortPath,1,15);
        //    }
        //}

        //private void Station16_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T16";
        //        string str1;
        //        string str2 = "T16";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 16);
        //    }
        //}

        //private void Station17_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T17";
        //        string str1;
        //        string str2 = "T17";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 17);
        //    }
        //}

        //private void Station18_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T18";
        //        string str1;
        //        string str2 = "T18";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 18);
        //    }
        //}

        //private void Station19_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T19";
        //        string str1;
        //        string str2 = "T19";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 19);
        //    }
        //}

        //private void Staton20_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T20";
        //        string str1;
        //        string str2 = "T20";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 20);
        //    }
        //}

        //private void Station21_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T21";
        //        string str1;
        //        string str2 = "T21";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 21);
        //    }
        //}

        //private void Station22_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T22";
        //        string str1;
        //        string str2 = "T22";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 22);
        //    }
        //}

        //private void Station23_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T23";
        //        string str1;
        //        string str2 = "T23";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1, str2);
        //        SendData(shortPath, 1, 23);
        //    }
        //}

        //private void Station24_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text == "AGV1")
        //    {
        //        textBox13.Text = "T24";
        //        string str1;
        //        string str2 = "T24";
        //        str1 = textBox1.Text;
        //        List<Track> shortPath = ShortPath(str1,str2);
        //        SendData(shortPath,1,201);
        //        shortPath = ShortPath(str2, "W0");
        //        SendData(shortPath, 1, 100);
        //    }
        //}
        #endregion
        public int Check(List<Track> shortPath, List<byte> strSP)
        {
            int i = 0;
            if (shortPath == null)
                return 0;
            foreach (Track sP in shortPath)
            {
                if (sP.Name == "K1S")//1
                {
                    strSP.Add(1);
                    ++i;
                    strSP.Add(3);
                    ++i;
                }
                if (sP.Name == "K1L")//2
                {
                    strSP.Add(1);
                    ++i;
                    strSP.Add(1);
                    ++i;
                }
                if (sP.Name == "F2S")//3
                {
                    strSP.Add(2);
                    ++i;
                    strSP.Add(3);
                    ++i;
                }
                if (sP.Name == "F2L")//4
                {
                    strSP.Add(2);
                    ++i;
                    strSP.Add(1);
                    ++i;
                }
                if (sP.Name == "T1")//5
                {
                    strSP.Add(4);
                    ++i;
                    strSP.Add(4);
                    ++i;
                }
                if (sP.Name == "T2")//6
                {
                    strSP.Add(5);
                    ++i;
                    strSP.Add(4);
                    ++i;
                }
                if (sP.Name == "W0")//6
                {
                    strSP.Add(3);
                    ++i;
                    strSP.Add(4);
                    ++i;
                }
            }
            
            //switch(shortPath[shortPath.Count - 1].endStation){
            //    case "T1":
            //        strSP.Add(4);
            //        ++i;                    
            //        break;
            //    case "T2":
            //        strSP.Add(5);
            //        ++i;                    
            //        break;
            //    case "Start":
            //        strSP.Add(3);
            //        ++i;
            //        break;
            //}
            //strSP.Add(4);
            //++i;
             
            return i;
        }

        public List<Track> ShortPath(string str1, string str2)
        {
            Station temp1 = new Station();
            Station temp2 = new Station();
            foreach (AdjacencyList.Vertex ver in Adj.items)
            {
                if (str1 == ver.data.name)
                {
                    temp1 = ver.data;
                    break;
                }
            }
            foreach (AdjacencyList.Vertex ver in Adj.items)
            {
                if (str2 == ver.data.name)
                {
                    temp2 = ver.data;
                    break;
                }
            }
            if (temp1 == temp2)
                return null;
            List<Track> shortPath = Adj.FindWay(Adj.Find(temp1), Adj.Find(temp2));
            return shortPath;
        }

        

        public void arrangeWays(List<Track> shortPath1, List<Track> shortPath2) {
            List<byte> byteList1 = new List<byte>();
            byteList1.Add(1);//carID
            byteList1.Add(0xff);//startTheWay
            List<byte> byteList2 = new List<byte>();
            int n1 = Check(shortPath1, byteList1);
            int n2 = Check(shortPath2, byteList2);
            if (n1 + n2 <= 30)
            {
                byteList1.AddRange(byteList2);
                //sendData(byteList1);
            }                
        }

        

        private void StartStation_Click(object sender, EventArgs e)
        {
            if (button7.Text == "AGV1")
            {
                textBox13.Text = "W0";
                string str1;
                string str2 = "W0";
                str1 = textBox1.Text;
                List<Track> shortPath1 = ShortPath(str1, str2);
                List<Track> shortPath2 = ShortPath(str2, "W0"/*str1*/);
                arrangeWays(shortPath1,shortPath2);
                //SendData(shortPath, 1, 100);
            }
        }

    }

    public class Track
    {
        //public Station startStation;
        //public Station endStation;
        private String startStation;
        private String endStation;
        private List<Point> trackPoints;
        private string name;
        private string pathStr;
        private string carAction;
        private ArrayList drawingList;

        public Track()//构造函数
        {
            trackPoints = new List<Point>();
        }
        public Track(Track t) 
        {
            this.trackPoints = new List<Point>( t.trackPoints);
            this.startStation = t.startStation;
            this.endStation = t.endStation;
        }
        public Track(ArrayList drawingList)
        {
            trackPoints = new List<Point>();
            this.drawingList = drawingList;           
        }

        public bool occupied = false;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string PathString 
        {
            get { return pathStr ; }
            set
            {
                pathStr = value;
                string str = "";
                int i;
                int sn;
                string type = "", lastType = "";
                try
                {
                    for (i = 0; i < pathStr.Length; i++)
                    {
                        char c = pathStr[i];
                        
                        switch (c)
                        {
                            case 'L':
                                lastType = type;
                                type = "line";                                
                                break;
                            case 'A':
                                lastType = type;
                                type = "arc";                                
                                break;
                            default:
                                {
                                    if (char.IsDigit(c))
                                        str += c;
                                    break;
                                }
                        }
                        if (i == pathStr.Length - 1)
                            lastType = type;
                        if (lastType == "line")
                        {
                            sn = Convert.ToInt32(str);
                            Line line = (Line)drawingList[sn];
                            AddLine(line);
                            lastType = "";
                            str = "";
                        }
                        else if (lastType == "arc")
                        {
                            sn = Convert.ToInt32(str);
                            Arc arc = (Arc)drawingList[sn];
                            AddArc(arc);
                            lastType = "";
                            str = "";
                        }
                    }
                }
                catch(Exception x)
                {
                    Console.WriteLine(x.Message);
                }
                // == 'L') || (str[0] == 'A')

            }
        }

        public string CarAction
        {
            get { return carAction; }
            set { carAction = value; }
        }

        public List<Point> TrackPointList
        {
            set { trackPoints = value; }
            get { return trackPoints; }
        }
        public Point[] TrackPoints
        {
            get { return trackPoints.ToArray(); }
        }
        //public Point StartPoint
        //{
        //    set { trackPoints[0] = value; }
        //    get { return trackPoints[0]; }
        //}
        //public Point EndPoint
        //{
        //    set { trackPoints[trackPoints.Count - 1] = value; }
        //    get { return trackPoints[trackPoints.Count - 1]; }
        //}
        public String StartStation
        {
            set { startStation = value; }
            get { return startStation; }
        }
        public String EndStation
        {
            set { endStation = value; }
            get { return endStation; }
        }
        public int Length
        {
            get
            {
                return trackPoints.Count;
            }//轨迹点的个数
        }
        public void clear()
        {
            trackPoints.Clear();
        }
        public bool AddLine(int x1, int y1, int x2, int y2)
        {
            Point curPoint = new Point(0, 400);
            int s = (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            float cosA = (float)(x2 - x1) / s;
            float sinA = (float)(y2 - y1) / s;

            for (int i = 0; i < s; i++)
            {
                Point p = new Point();
                p.X = x1 + (int)(i * cosA);
                p.Y = y1 + (int)(i * sinA);
                trackPoints.Add(p);
            }
            return true;
        }
        public void AddLine(Line line){
            AddLine(line.GetStartPoint.X, line.GetStartPoint.Y, line.GetEndPoint.X, line.GetEndPoint.Y);
        }

        public bool AddArc(int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            int s = Math.Abs(width * sweepAngle / 90);
            int r = width / 2;
            Point origin = new Point();
            origin.X = x + width / 2;
            origin.Y = y + width / 2;
            if (width != height)
            {
                Exception ex = new Exception("agv track AddArc: only Circle Arc is supported.");
                throw ex;
            }
            for (int i = 0; i < s; i++)
            {
                Point p = new Point();
                p.X = origin.X +
                    (int)(r * Math.Cos(
                            startAngle * Math.PI / 180 + (float)i / s * sweepAngle * Math.PI / 180
                              )
                         );
                p.Y = origin.Y +
                    (int)(r * Math.Sin(
                            startAngle * Math.PI / 180 + (float)i / s * sweepAngle * Math.PI / 180
                              )
                    );
                trackPoints.Add(p);
            }
            return true;
        }
        public void AddArc(Arc arc)
        {
            int radius = (int)arc.Radius;
            Point center = arc.CenterPoint;
            int left = center.X - radius;
            int top = center.Y - radius;
            AddArc(left,top,2*radius,2*radius,-(int)arc.StartAngle,-(int)arc.SweepAngle);
        }
        /*public bool AddLine(Point p1, Point p2)
       {
           int x1, y1, x2, y2;
           x1 = p1.X;
           y1 = p1.Y;
           x2 = p2.X;
           y2 = p2.Y;
           AddLine(x1, y1, x2, y2);
           return true;
       }*/
        /*public bool AddArc(Rectangle rect, int startAngle, int sweepAngle)
        {
            int x = rect.X, y = rect.Y, width = rect.Width, height = rect.Height;
            AddArc(x, y, width, height, startAngle, sweepAngle);
            return true;
        }*/
    }

    public class Station
    {
        public int stationID;
        public string name;
        public int X;
        public int Y;
        public int BtnXoffset;
        public int BtnYoffset;
        public int cardID;
        public bool targeted = false;
        public Point Location 
        {
            get { return new Point(X,Y); }
        }
        public Station()
        {            
        }
        public Station(string name, int X, int Y, int btnX, int btnY,int cardID,int stationID)
        {
            this.name = name;
            this.X = X;
            this.Y = Y;
            this.BtnXoffset = btnX;
            this.BtnYoffset = btnY;
            this.cardID = cardID;
            this.stationID = stationID;
            Next = "";
        }
        public Station(string name, int X, int Y)
        {
            this.name = name;
            this.X = X;
            this.Y = Y;
        }
        public string Next;
        public Car OccupiedCar = null;
        //public bool canStop;
        public int CardID
        {
            get { return cardID; }
        }
        public string Name
        {
            get { return name; }
        }
        
    }

    enum SerialEventType{
        RemoteCall,
        ErrorState,
        StateReport,
        NoReport
    }

    public class CarStateReportEventArgs 
    {
        private byte error;
        private byte type;
        private byte carid;
        private byte cardid;
        private byte targetCardId;
        private byte endCardId;
        private bool obstacle;
        private byte movement;
        private byte step;
        private byte taskLen;
        public CarStateReportEventArgs(byte carid, byte cardid, byte type, byte movement, byte obstacle,
            byte step,byte taskLen, byte error, byte targetCardId, byte endCardId) 
        {
            this.carid = carid;
            this.cardid = cardid;
            this.type = type;
            this.movement = movement;
            if (obstacle > 0)
                this.obstacle = true;
            else
                this.obstacle = false;
            this.step = step;
            this.taskLen = taskLen;
            this.error = error;
            this.targetCardId = targetCardId;
            this.endCardId = endCardId;     
        }
        public byte Type {
            get { return type; }        
        }
        public byte CarId{
            get { return carid;}
        }
        public byte CardId{
            get{ return cardid; }
        }
        public byte TargetCardId{
            get { return targetCardId; }
        }
        public byte EndCardId{
            get{ return endCardId; }
        }
        public bool Obstacle{
            get{ return obstacle;}
        }
        public byte Movement{
            get { return movement;}
        }
        public byte Step { 
            get { return step;} 
        }
        public byte TaskLen{
            get { return taskLen; }
        }
    }

    public class RemoteCallEventArgs
    {
        private byte station_ID;
        private byte call_type;      
        public RemoteCallEventArgs(byte station_ID, byte call_type)
        {            
            this.station_ID = station_ID;
            this.call_type = call_type;
        }
        public byte Station_ID
        {
            get { return station_ID; }
        }
        public byte Call_type
        {
            get { return call_type; }
        }
    }

    public class SerialHandler
    {
        private List<byte> serialBuf = new List<byte>(100);
        private RemoteCallEventArgs callEventArgs = null;
        private CarStateReportEventArgs stateReportEventArgs = null;
        public delegate void CarSateReportEventHandler(object sender, CarStateReportEventArgs e);
        public delegate void RemoteCallEventHandler(object sender, RemoteCallEventArgs e);
        public event RemoteCallEventHandler remoteCallEvent;
        public event CarSateReportEventHandler carStateReportEvent;
        public void handleOneByte(byte b)
        {
            serialBuf.Add(b);

            if (serialBuf[0] != 0x68)
                serialBuf.RemoveAt(0);
            if (serialBuf.Count < 8)
                return;
            callEventArgs = null;
            stateReportEventArgs = null;
            if (serialBuf[1] == 0xE5)//呼叫器呼叫
            {
                if (serialBuf != null)
                {
                    callEventArgs = new RemoteCallEventArgs(serialBuf[5], serialBuf[6]);
                }
            }
            else if (serialBuf[1] == 0xE3)//状态上报
            {
                if (serialBuf.Count < 20)
                    return;
                else if (serialBuf.Count == 20)
                {
                    stateReportEventArgs = new CarStateReportEventArgs(serialBuf[5], serialBuf[6], serialBuf[7],
                        serialBuf[8], serialBuf[9], serialBuf[10], serialBuf[11], serialBuf[12],serialBuf[13], serialBuf[14]);
                }
            }

            try
            {
                if ((null != remoteCallEvent) && (callEventArgs != null))
                {
                    remoteCallEvent(this, callEventArgs);
                    serialBuf.Clear();//有效帧
                }
                else if ((null != stateReportEventArgs) && (null != carStateReportEvent))
                {
                    carStateReportEvent(this, stateReportEventArgs);
                    serialBuf.Clear();
                }
                else
                    serialBuf.RemoveAt(0);

            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }

         }
    }

   
}
