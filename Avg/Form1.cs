

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

namespace Avg
{
    public partial class Form1 : Form
    {
        enum eSerialSate { SerialOn, SerialOff };
        //static int i = 0;
        //static int i1 = 0;
        static bool initFlag = false;
        //static int callNum = 0;
        //static int threadNum = 0;
        Thread readThread;
        private eSerialSate serialState = eSerialSate.SerialOff;
        private System.Timers.Timer aTimer1;//计时器

        //private System.Timers.Timer aTimer2;//计时器
        Graphics dc, dc1, dc2;
        Point[] CurvePoint, CurvePoint1, CurvePoint2, CurvePoint3, CurvePoint4;//画图
        Track agvTrack = new Track();//重绘图形 //保存点到点之间路径 相邻点的路径
        Track path = new Track();
        private static int index1 = 0;//像素索引
        private static int index2 = 0;//像素索引
        private static int index3 = 0;//像素索引
        private static int index4 = 0;//像素索引
        AdjacencyList Adj = new AdjacencyList(20);
        #region
        static Station T1=new Station("T1", 400, 300);
        Station K1;
        Station F1;
        static Station T2 = new Station("T2", 400, 500);
        Station K2;
        static Station F2 = new Station("F2", 450, 450);     
        Station P1;
        Station P2;
        Station P3;
        Station P4;
        Station Z1;
        Station Z2;
        Station Z3;
        Station Z4;
        Station Z5;
        Station Z6;
        Station S1;
        Station S2;
        Station S3;
        Station S4;
        #endregion
        public Form1()
        {
            InitializeComponent();
            aTimer1 = new System.Timers.Timer(1000000);
            aTimer1.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
            //aTimer1.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
            aTimer1.Interval = 60;
            aTimer1.AutoReset = true;
            aTimer1.Enabled = false;//只有Timer类 可以触发自身事件
            
            comboBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox.SelectedItem = comboBox.Items[0];
            InitPort();
            CurvePoint1 = new Point[0];
            #region
            S1 = new Station("S1", 150, 400);
            S2 = new Station("S2",130,400);
            S3 = new Station("S3",175,375);
            S4 = new Station("S4", 175, 425);
            //T1 = new Station("T1", 400, 300);
            K1 = new Station("K1", 350, 350);
            F1 = new Station("F1", 450, 350);
            //T2 = new Station("T2", 400, 500);
            K2 = new Station("K2", 350, 450);
            //F2 = new Station("F2", 450, 450);
            P1 = new Station("P1", 200, 350);
            P2 = new Station("P2", 600, 350);
            P3 = new Station("P3", 200, 450);
            P4 = new Station("P4", 600, 450);
            Z1 = new Station("Z1",250,350);
            Z2 = new Station("Z2", 275, 325,true);
            Z3 = new Station("Z3",300,350);
            Z4 = new Station("Z4", 600, 450);
            Z5 = new Station("Z5", 575, 475,true);
            Z6 = new Station("Z6", 550, 450);
            Adj.AddVertex(S1);
            Adj.AddVertex(S2);
            Adj.AddVertex(S3);
            Adj.AddVertex(S4);
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
            Adj.AddVertex(Z1);
            Adj.AddVertex(Z2);
            Adj.AddVertex(Z3);
            Adj.AddVertex(Z4);
            Adj.AddVertex(Z5);
            Adj.AddVertex(Z6);
            Adj.AddDirectedEdge(Adj.Find(S1), Adj.Find(S3));//1
            Adj.AddDirectedEdge(Adj.Find(S2), Adj.Find(S3));//2
            Adj.AddDirectedEdge(Adj.Find(S4), Adj.Find(S1));//3
            Adj.AddDirectedEdge(Adj.Find(S4), Adj.Find(S2));//4
            Adj.AddDirectedEdge(Adj.Find(S3), Adj.Find(P1));//5
            Adj.AddDirectedEdge(Adj.Find(P1), Adj.Find(Z1));//6
            Adj.AddDirectedEdge(Adj.Find(Z1), Adj.Find(Z2));//7
            Adj.AddDirectedEdge(Adj.Find(Z2), Adj.Find(Z3));//8
            Adj.AddDirectedEdge(Adj.Find(Z1), Adj.Find(Z3));//9
            Adj.AddDirectedEdge(Adj.Find(Z3), Adj.Find(K1));//10
            Adj.AddDirectedEdge(Adj.Find(K1), Adj.Find(F1));//11
            Adj.AddDirectedEdge(Adj.Find(K1), Adj.Find(T1));//12
            Adj.AddDirectedEdge(Adj.Find(T1), Adj.Find(F1));//13
            Adj.AddDirectedEdge(Adj.Find(F1), Adj.Find(P2));//14
            Adj.AddDirectedEdge(Adj.Find(P2), Adj.Find(P4));//15
            Adj.AddDirectedEdge(Adj.Find(P4), Adj.Find(Z6));//16
            Adj.AddDirectedEdge(Adj.Find(Z6), Adj.Find(Z5));//17
            Adj.AddDirectedEdge(Adj.Find(Z5), Adj.Find(Z4));//18
            Adj.AddDirectedEdge(Adj.Find(Z6), Adj.Find(Z4));//19
            Adj.AddDirectedEdge(Adj.Find(Z4), Adj.Find(F2));//20
            Adj.AddDirectedEdge(Adj.Find(F2), Adj.Find(K2));//21
            Adj.AddDirectedEdge(Adj.Find(F2), Adj.Find(T2));//22
            Adj.AddDirectedEdge(Adj.Find(T2), Adj.Find(K2));//23
            Adj.AddDirectedEdge(Adj.Find(K2), Adj.Find(P3));//24
            Adj.AddDirectedEdge(Adj.Find(P3), Adj.Find(S4));//25
            
            #endregion
        }

        public static Station getT1
        {
            get { return T1; }
        }

        public static Station getT2
        {
            get { return T2; }
        }

        public static Station getF2
        {
            get { return F2; }
        }
        public void ChangeData1()
        {
            textBox1.Text = textBox13.Text;       
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

        public void ChangeData3()
        {
            textBox3.Text = textBox15.Text;
            textBox9.Text = "等待";
            textBox15.Text = "";
            index3 = 0;
        }

        public void ChangeData4()
        {
            textBox4.Text = textBox16.Text;
            textBox10.Text = "等待";
            textBox16.Text = "";
            index4 = 0;
        }

        public void UpdateLable1(int index1)
        {
            Point p = new Point();
            p = CurvePoint1[index1];
            p.X -= 6;
            p.Y -= 6;
            label_car1.Location = p;

        }
        
        public void UpdateLable2(int index2)
        {
            Point p = new Point();
            p = CurvePoint2[index2];
            p.X -= 6;
            p.Y -= 6;
            label_car2.Location = p;

        }

        public void UpdateLable3(int index3)
        {
            Point p = new Point();
            p = CurvePoint3[index3];
            p.X -= 6;
            p.Y -= 6;
            label_car3.Location = p;

        }

        public void UpdateLable4(int index4)
        {
            Point p = new Point();
            p = CurvePoint4[index4];
            p.X -= 6;
            p.Y -= 6;
            label_car4.Location = p;

        }
        
        private void OnTimedEvent1(object source, ElapsedEventArgs e)
        {
            if (textBox7.Text == "工作")
            {
                try
                {
                    if (CurvePoint1 == null || CurvePoint1.Length == 0)
                    {
                        return;
                    }
                    index1 += 1;
                    if (index1 == CurvePoint1.Length)
                    {
                        Invoke(new ChangeDataDelegate(ChangeData1));
                        index1 = 0;
                        CurvePoint1 = null;
                        //return;
                    }
                    Invoke(new UpdateLableDelegate(UpdateLable1), new object[] { index1 });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            if (textBox8.Text == "工作")
            {
                try
                {
                    if (CurvePoint2 == null || CurvePoint2.Length == 0)
                    {
                        return;
                    }
                    index2 += 1;
                    if (index2 == CurvePoint2.Length)
                    {
                        Invoke(new ChangeDataDelegate(ChangeData2));
                        CurvePoint2 = null;
                        index2 = 0;
                        //return;
                    }
                    Invoke(new UpdateLableDelegate(UpdateLable2), new object[] { index2 });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (textBox9.Text == "工作")
            {
                try
                {
                    if (CurvePoint3 == null || CurvePoint3.Length == 0)
                    {
                        return;
                    }
                    index3 += 1;
                    if (index3 == CurvePoint3.Length)
                    {
                        Invoke(new ChangeDataDelegate(ChangeData3));
                        CurvePoint3 = null;
                        index3 = 0;
                        //return;
                    }
                    Invoke(new UpdateLableDelegate(UpdateLable3), new object[] { index3 });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (textBox10.Text == "工作")
            {
                try
                {
                    if (CurvePoint4 == null || CurvePoint4.Length == 0)
                    {
                        return;
                    }
                    index4 += 1;
                    if (index4 == CurvePoint4.Length)
                    {
                        Invoke(new ChangeDataDelegate(ChangeData4));
                        CurvePoint4 = null;
                        index4 = 0;
                        //return;
                    }
                    Invoke(new UpdateLableDelegate(UpdateLable4), new object[] { index4 });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
       
        protected delegate void UpdateLableDelegate(int index);
        protected delegate void ChangeDataDelegate();

        protected override void OnPaint(PaintEventArgs e)//重绘
        {
            base.OnPaint(e);
            dc = e.Graphics;
            Pen blkPen = new Pen(Color.Black, 1);

            agvTrack.AddArc(250, 325, 50, 50, 180, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(250, 325, 50, 50, 270, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(500, 425, 50, 50, 0, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(500, 425, 50, 50, 90, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(130, 375, 50, 50, 90, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(130, 375, 50, 50, 180, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();


            agvTrack.AddLine(200, 350, 600, 350);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddLine(200, 450, 600, 450);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(150, 350, 100, 100, 180, 45);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(150, 350, 100, 100, 225, 45);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            //agvTrack.AddArc(150, 350, 100, 100, 180, 90);
            //CurvePoint = agvTrack.TrackPoints;
            //dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            //agvTrack.trackPoints.Clear();

            agvTrack.AddArc(150, 350, 100, 100, 90, 45);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(150, 350, 100, 100, 135, 45);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            //agvTrack.AddArc(150, 350, 100, 100, 90, 90);
            //CurvePoint = agvTrack.TrackPoints;
            //dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            //agvTrack.trackPoints.Clear();

            agvTrack.AddArc(550, 350, 100, 100, 90, -180);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(350, 300, 100, 100, 180, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(350, 300, 100, 100, 0, -90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(350, 400, 100, 100, 90, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();

            agvTrack.AddArc(350, 400, 100, 100, 0, 90);
            CurvePoint = agvTrack.TrackPoints;
            dc.DrawCurve(blkPen, CurvePoint, 0.1f);
            agvTrack.trackPoints.Clear();
        }

        private void button8_Click(object sender, EventArgs e)//初始化
        {
            Init.ForeColor = Color.Gray;
            textBox1.Text = "S1";
            textBox2.Text = "S2";
            textBox3.Text = "Z2";
            textBox4.Text = "Z5";
            textBox7.Text = "等待";
            textBox8.Text = "等待";
            textBox9.Text = "等待";
            textBox10.Text = "等待";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)//启动
        {
            aTimer1.Start();
            string str1;
            string str2;
            if (button7.Text == "AGV1")
            {
                str1 = textBox1.Text;
                str2 = textBox13.Text;
                CurvePoint1 = ShortPath(str1, str2);
            }

            if (button7.Text == "AGV2")
            {
                str1 = textBox2.Text;
                str2 = textBox14.Text;
                CurvePoint2 = ShortPath(str1, str2);
            }

            if (button7.Text == "AGV3")
            {
                str1 = textBox3.Text;
                str2 = textBox15.Text;
                CurvePoint3 = ShortPath(str1, str2);
            }

            if (button7.Text == "AGV4")
            {
                str1 = textBox4.Text;
                str2 = textBox16.Text;
                CurvePoint4 = ShortPath(str1, str2); 
            }
        }

        private void button1_Click(object sender, EventArgs e)//AGV1
        {
            textBox7.Text = "工作";
            button7.Text = "AGV1";
        }

        private void button2_Click(object sender, EventArgs e)//AGV2
        {
            textBox8.Text = "工作";
            button7.Text = "AGV2";
        }

        private void button3_Click(object sender, EventArgs e)//AGV3
        {
            textBox9.Text = "工作";
            button7.Text = "AGV3";
        }

        private void button4_Click(object sender, EventArgs e)//AGV4
        {
            textBox10.Text = "工作";
            button7.Text = "AGV4";
        }

        private void button10_Click(object sender, EventArgs e)//停止
        {
            if (button7.Text == "AGV1")
            {
                //aTimer1.Elapsed -= new ElapsedEventHandler(OnTimedEvent1);
                //aTimer1.Enabled = false;
            }
            if (button7.Text == "AGV2")
            {
                //aTimer2.Elapsed -= new ElapsedEventHandler(OnTimedEvent2);
                //aTimer2.Enabled = false;
            }
        }

        private void DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
             
        }    

        private void button40_Click(object sender, EventArgs e)//Send
        {
            CommandFrame cmd = new CommandFrame();
            cmd.CommandWord1 = define.W108_CAR_CONSOLE_SEND_COMMAND;//0XEE
            List<byte> temArray = new List<byte>();
            if (textBox19.Text != "")
            {
                byte[] command = Encoding.Default.GetBytes(textBox19.Text);
                foreach (byte c in command)
                {
                    temArray.Add(c);
                }
            }
            //temArray.Add((byte)((id & 0xff00) >> 0x08));
            cmd.DataLength1 = (byte)temArray.Count;
            cmd.Data1 = temArray.ToArray();
            byte[] t = cmd.GetFrameToBytes();
            try
            {
                //SendMsgTorSerialPort(cmd);
                serialPort1.Write(t, 0, t.Length);
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show(ex.Message);

            }
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
              //  List<Track> shortPath1 = ShortPath(str1, str2);
              //  List<Track> shortPath2 = ShortPath(str2, "P3");
               // arrangeWays(shortPath1, shortPath2);
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
              //  List<Track> shortPath1 = ShortPath(str1, str2);
             //   List<Track> shortPath2 = ShortPath(str2, "P3");
             //   arrangeWays(shortPath1,shortPath2);
            }
        }

        private void StartStation_Click(object sender, EventArgs e)
        {
            if (button7.Text == "AGV1")
            {
                //textBox13.Text = "W0";
              //  string str1;
              //  string str2 = "W0";
              //  str1 = textBox1.Text;
          //      List<Track> shortPath1 = ShortPath(str1, str2);
          //      List<Track> shortPath2 = ShortPath(str2, "W0"/*str1*/);
          //      arrangeWays(shortPath1, shortPath2);
            }
        }

        protected delegate void niming();

        protected delegate void rePlan(SerialEventArgs e);

        void SetLocation(SerialEventArgs e)
        {
           
        }

        void SetStartStation(SerialEventArgs e)
        {
            
        }

        string compare(string callStation)
        {
            int S1toCS = shortLength("S1", callStation);
            int T1toCS = shortLength("T1", callStation);
            int T2toCS = shortLength("T2", callStation);
            int temp = S1toCS < T1toCS ? S1toCS : T1toCS;
            int terminal = temp < T2toCS ? temp : T2toCS;
            if (terminal == S1toCS)
                return "S1";
            if (terminal == T1toCS)
                return "T1";
            return "T2";
        }

        string findWaitCar()
        {
            if (textBox1.Text == "S1")
                return "AGV1";
            if (textBox2.Text == "S1")
                return "AGV2";
            return null;
        }

        void dispatch(object obj)
        {
            string s = (string)obj;
            Invoke(new niming(Station1.PerformClick));
            Invoke(new niming(button9.PerformClick));

        }

        void planning(object obj)//规划处理函数
        {
            SerialEventArgs e = (SerialEventArgs)obj;
            switch (e.CallMessage)
            {
                case "toT1":
                    
                    new Thread(dispatch).Start(compare(e.CallMessage));
                    break;
                case "toT2":
                    break;
            }
        }

        public void agvSerialRemoteEvent(object sender, SerialEventArgs e)//事件处理函数
        {
            new Thread(planning).Start(e);//规划线程
            //new Thread(replanning).Start();//车子跑错了重新规划线程
       
        }

        public void arrangeWays(List<Track> shortPath1, List<Track> shortPath2)
        {
            List<byte> byteList1 = new List<byte>();
            byteList1.Add(1);//carID
            byteList1.Add(0xff);//startTheWay
            List<byte> byteList2 = new List<byte>();
            int n1 = Check(shortPath1, byteList1);
            int n2 = Check(shortPath2, byteList2);
            if (n1 + n2 <= 30)
            {
                byteList1.AddRange(byteList2);
                sendData(byteList1);
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

        public int InitPort()
        {
            serialPort1.PortName = comboBox.SelectedItem.ToString();
            serialPort1.BaudRate = 115200;
            serialPort1.DataBits = 8;
            // serialPort1.WriteTimeout = 500;
            //serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);
            return 0;
        }

        public void ReadPort()//处理消息的线程
        {
            SerialHandler serialHander = new SerialHandler();
            //serialHander.serialEvent += agvSerialRemoteEvent;
            serialHander.serialEvent += new SerialHandler.SeialEventHandler(agvSerialRemoteEvent);//agvSerialRemoteEvent 事件订阅者是 Form                                                                                            //SerialHandler 是 事件的发行者
            while (true)
            {
                if (serialPort1.IsOpen && serialPort1.BytesToRead > 0)
                {
                        byte b = (byte)serialPort1.ReadByte();
                        serialHander.handleOneByte(b);
                }
            }
        }

        public Point[] ShortPath(string str1, string str2)
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
            List<Point> temp = new List<Point>();
            List<Point> TempAll = new List<Point>();
            foreach (Track m in shortPath)
            {
                temp = m.trackPoints;
                TempAll.AddRange(temp);
            }
            Point[] CurvePoint = TempAll.ToArray();
            return CurvePoint;
        }

        public int shortLength(string str1, string str2)
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
            List<Track> shortPath = Adj.FindWay(Adj.Find(temp1), Adj.Find(temp2));
            List<Point> temp = new List<Point>();
            List<Point> TempAll = new List<Point>();
            foreach (Track m in shortPath)
            {
                temp = m.trackPoints;
                TempAll.AddRange(temp);
            }
            Point[] CurvePoint = TempAll.ToArray();
            return CurvePoint.Length;
        }

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

        public void sendData(List<byte> byteList)  {
            CommandFrame cmd = new CommandFrame();
            cmd.CommandWord1 = define.W108_CAR_CONSOLE_SEND_COMMAND;
            cmd.Data1 = byteList.ToArray();
            cmd.DataLength1 = (byte)byteList.Count;
            cmd.CalSumCheck();
            if (serialPort1.IsOpen)
            {
                byte[] t = cmd.GetFrameToBytes();
                serialPort1.Write(t, 0, t.Length);
            }
        }

        public class SerialEventArgs//串口事件
        {
            private string callMessage;
            private string carInfo;
            //private string reStation;
            public SerialEventArgs(string callMessage, string carInfo)
            {
                this.callMessage = callMessage;
                this.carInfo = carInfo;
            }
           
            public string CallMessage
            {
                get { return callMessage; }
            }
            public string CarInfo
            {
                get { return carInfo; }
            }
           
        }

        public class SerialHandler//事件发行者
        {
            private List<byte> serialBuf = new List<byte>(12);
            private SerialEventArgs eArgs = null;
            public delegate void SeialEventHandler(object sender, SerialEventArgs e);//声明代理
            public event SeialEventHandler serialEvent;//声明代理的事件
            public void handleOneByte(byte b)
            {
                serialBuf.Add(b);
                if (serialBuf[0] != 0xfe)
                    serialBuf.RemoveAt(0);
                if (serialBuf.Count < 11)
                    return;

                if (serialBuf[2] == 0xF2)//车子错位，重新规划回起点
                {
                    if (serialBuf[7] == 0x04)
                    {
                        if (serialBuf[8] == 0x0D && serialBuf[9] == 0xB1 && serialBuf[10] == 0xDB)
                        {
                            //eArgs = new SerialEventArgs(null, "AGV1", "F2");
                        }
                    }
                }

               if (serialBuf[2] == 0xF6)//呼叫器呼叫
                {
                    if (serialBuf[7] == 0x0c && serialBuf[8] == 0x38 && serialBuf[9] == 0xca)//T1呼叫
                    {
                        eArgs = new SerialEventArgs("toT1", "tail");
                    }
                    else if (serialBuf[7] == 0x0e && serialBuf[8] == 0x2a && serialBuf[9] == 0xe9)//T2呼叫
                    {
                        eArgs = new SerialEventArgs("toT2", "notail");
                    }
                }

                try
                {
                    if ((null != serialEvent) && (eArgs != null))
                        serialEvent(this, eArgs);//事件非null，触发事件
                }
                catch (Exception x)
                {
                    Console.WriteLine(x.Message);
                }
                serialBuf.Clear();
            }
        }
       
    }

    public class Track
    {
        //public Station startStation;
        //public Station endStation;
        public String startStation;
        public String endStation;
        public List<Point> trackPoints;
        private string name;
        public Track()//构造函数
        {
            trackPoints = new List<Point>();
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
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
        public string name;
        public int X;
        public int Y;
        public bool bPark = false;
 
        public Station()
        {
        }
        public Station(string name, int X, int Y)
        {
            this.name = name;
            this.X = X;
            this.Y = Y;
        }

        public Station(string name, int X, int Y,bool bPark)
        {
            this.name = name;
            this.X = X;
            this.Y = Y;
            this.bPark = bPark;
        }

        //public bool canStop;

    }

    

    
}
