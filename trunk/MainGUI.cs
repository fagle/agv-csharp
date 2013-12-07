/* Author: Evren DAGLIOGLU
 * E-mail: evrenda@yahoo.com
 * This software is copyrighted to the author himself. It can be used freely for educational purposes.
 * For commercial usage written consent of the author must be taken and a reference to the author should be provided. 
 * No responsibility will be taken for any loss or damage that will occur as a result of the usage of this code. 
 * 
 * Please feel free to inform me about any bugs, problems, ideas etc.
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Collections.Generic;
using System.IO.Ports;
namespace AGV
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainGUI : System.Windows.Forms.Form
    {
        public bool keepCtrlFormWithinBorders = true;
        public bool IsPBoxVisible = true;

        private Canvas newCanvas;

        public ArrayList listMasterArray = new ArrayList();
        public string inputFileTxt;


        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private SplitContainer splitContainer1;
        private Button button1;
        private Button button2;
        private ComboBox comboBox;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private System.IO.Ports.SerialPort serialPort1;
        private TextBox textBox1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox textBox2;
        private Label label1;
        private GroupBox groupBox3;
        private IContainer components;
        private bool firstSerialEvent = true;
        private PictureBox pictureBox1;
        private Thread readThread = null;
        private Thread writeThread = null;
        enum eSerialSate { SerialOn, SerialOff };
        private eSerialSate serialState = eSerialSate.SerialOff;

        public MainGUI()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            comboBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox.SelectedItem = comboBox.Items[0];
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

        private delegate void SetTextCallBack(string text);
        private void writeLine(string text)
        {
            if (textBox1.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(writeLine);          
                Invoke(d, new object[] { text });
            }
            else
            {
                textBox1.Text += "\r\n" + text;
                textBox1.Select(textBox1.Text.Length, 0);
                textBox1.ScrollToCaret();
            }
        }

        public void ReadPort()
        {
            SerialHandler serialHander = new SerialHandler();
            serialHander.serialEvent += agvSerialRemoteCall;
            while (true)
            {
                if (serialPort1.IsOpen)
                {
                    if (serialPort1.BytesToRead == 0)
                    {
                        //Thread.Sleep(100);
                        continue;
                    }
                    try
                    {
                        byte b = (byte)serialPort1.ReadByte();
                        Console.WriteLine(b);
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
                else
                    break;
            }
            
        }

        public void writePort()
        {
            while (true)
            {
                if (serialPort1.IsOpen)
                {
                    if (serialPort1.BytesToWrite == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                }
            }
        }

        public delegate void PerformClick();
        public void agvSerialRemoteCall(object sender, SerialEventArgs e)
        {
            Station startStation = null;
            Station targetStation = null;
            Station endStation = null;
            switch(e.Call_type)
            {
                case 1:
                    startStation = this.newCanvas.Scheduler.GStartStation;
                    targetStation = this.newCanvas.StationDic["S" + e.Station_ID];
                    endStation = this.newCanvas.StationDic["F29"];
                    break;
                case 2:
                    startStation = this.newCanvas.Scheduler.RStartStation;
                    targetStation = this.newCanvas.StationDic["S" + e.Station_ID];
                    endStation = this.newCanvas.StationDic["S2"];
                    break;
                case 3:
                    startStation = this.newCanvas.Scheduler.PStartStation;
                    targetStation = this.newCanvas.StationDic["S" + e.Station_ID];
                    endStation = this.newCanvas.StationDic["S3"];
                    break;
                case 4:
                    startStation = this.newCanvas.Scheduler.GOStartStation;
                    targetStation = this.newCanvas.StationDic["S" + e.Station_ID];
                    endStation = this.newCanvas.StationDic["S10"];
                    break;
            }
            RoadTableFrameHandler serialHander = new RoadTableFrameHandler();
            //serialHander.serialEvent += serialHander.accessRoadTable;         
            //if (firstSerialEvent == true)
            //{
            //    firstSerialEvent = false;
            //}
            Control[] a;
            Button b;
            try
            {
                switch (e.Station_ID)
                {
                    case 1:
                        writeLine("站点1呼叫");
                        break;
                    case 2:
                        writeLine("站点2呼叫");
                        break;
                    case 3:
                                           
                        
                        break;
                    case 4:
                        writeLine("站点2呼叫");
                        break;
                    case 5:
                        writeLine("站点1呼叫");
                        break;
                    case 6:
                        writeLine("站点2呼叫");
                        break;
                    case 7:
                        writeLine("站点1呼叫");
                        break;
                    case 8:
                        writeLine("站点2呼叫");
                        break;
                    case 9:
                        writeLine("站点1呼叫");
                        break;
                    case 10:
                        writeLine("站点2呼叫");
                        break;
                    case 11:
                        serialHander.accessRoadTable(startStation, targetStation, endStation, this.newCanvas.AdjList, this.serialPort1, this.newCanvas);
                        this.newCanvas.Scheduler.CallStyle = (int)e.Call_type;
                        a = this.newCanvas.Controls.Find("S11",false);
                        b = (Button)a[0];
                        if (b != null)
                        {
                            Invoke(new PerformClick(b.PerformClick));      
                        }

                        writeLine("站点11呼叫");   
                        //this.newCanvas.Scheduler.CallStyle = 0;
                        break;
                    case 12:
                        serialHander.accessRoadTable(startStation, targetStation, endStation, this.newCanvas.AdjList, this.serialPort1, this.newCanvas);                    
                        this.newCanvas.Scheduler.CallStyle = (int)e.Call_type;
                        a = this.newCanvas.Controls.Find("S12", false);
                        b = (Button)a[0];
                        if(b!=null)
                        Invoke(new PerformClick(b.PerformClick));
                        writeLine("站点12呼叫"); 
                        //this.newCanvas.Scheduler.CallStyle = 0;
                        break;
                    case 13:
                        writeLine("站点1呼叫");
                        break;
                    case 14:
                        writeLine("站点2呼叫");
                        break;
                    case 15:
                        writeLine("站点1呼叫");
                        break;
                    case 16:
                        writeLine("站点2呼叫");
                        break;
                    case 17:
                        writeLine("站点1呼叫");
                        break;
                    case 18:
                        writeLine("站点2呼叫");
                        break;
                    case 19:
                        writeLine("站点1呼叫");
                        break;
                    case 20:
                        writeLine("站点2呼叫");
                        break;
                    case 21:
                        writeLine("站点1呼叫");
                        break;
                    case 22:
                        writeLine("站点2呼叫");
                        break;
                    case 23:
                        writeLine("站点1呼叫");
                        break;
                    case 24:
                        writeLine("站点2呼叫");
                        break;
                    //case "reF2":
                    //    writeLine("F2跑错");                        
                    //    break;
                    //case "reT1":
                    //    writeLine("T1跑错");
                    //    break;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
         
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            serialPort1.Close();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem5,
            this.menuItem6});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
            this.menuItem1.Text = "文件(F)";
            // 
            // menuItem2
            // 
            this.menuItem2.Enabled = false;
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Open";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "Exit";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.MdiList = true;
            this.menuItem5.Text = "窗口(W)";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7});
            this.menuItem6.Text = "帮助(H)";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.Text = "关于(A)";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "q";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1MinSize = 160;
            this.splitContainer1.Size = new System.Drawing.Size(1264, 729);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(882, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(382, 135);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "状态显示";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 20);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(358, 109);
            this.textBox1.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Location = new System.Drawing.Point(3, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 135);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作说明";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(24, 20);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(384, 109);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "    运行本软件前，请将调度中心设备用USB线连到本计算机，并安装好\r\n\r\n与操作系统版本一致的FT232串口驱动程序。可在设备管理器中查看插上\r\n\r\n设备后多" +
                "出来的COM号，打开本软件后选择对应的串口号打开，调度软件\r\n\r\n开始调度工作。";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.comboBox);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(452, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 135);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口设置";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AGV.Properties.Resources.ICON_OFF;
            this.pictureBox1.Location = new System.Drawing.Point(134, 56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "串口号选择：";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(165, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "打开串口";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox
            // 
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(21, 56);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(107, 20);
            this.comboBox.TabIndex = 2;
            this.comboBox.DropDown += new System.EventHandler(this.comboBox_DropDown);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(288, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 39);
            this.button2.TabIndex = 1;
            this.button2.Text = "点我有惊喜";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MainGUI
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1264, 729);
            this.Controls.Add(this.splitContainer1);
            this.Menu = this.mainMenu1;
            this.Name = "MainGUI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainGUI());
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            newCanvas = new Canvas();			//a new canvas is created...

            newCanvas.TopLevel = false;
            //newCanvas.MdiParent = this.panel1;			//...its mdiparent is set...           
            splitContainer1.Panel2.Controls.Add(newCanvas);
            newCanvas.Show();							//the canvas is displayed...
            newCanvas.Activate();
            newCanvas.Focus();
        }

        private void menuItem2_Click(object sender, System.EventArgs e)		//Opens openfile dialog to select a DXF file
        {
            inputFileTxt = "";

            openFileDialog1.InitialDirectory = "c:\\";		//sets the initial directory of the openfile dialog

            openFileDialog1.Filter = "dxf files (*.dxf)|*.dxf|All files (*.*)|*.*";	//filters the visible files...

            openFileDialog1.FilterIndex = 1;


            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)		//open file dialog is shown here...if "cancel" button is clicked then nothing will be done...
            {
                inputFileTxt = openFileDialog1.FileName;	//filename is taken (file path is also included to this name example: c:\windows\system\blabla.dxf

                int ino = inputFileTxt.LastIndexOf("\\");	//index no of the last "\" (that is before the filename) is found here


                newCanvas = new Canvas();			//a new canvas is created...

                newCanvas.MdiParent = this;			//...its mdiparent is set...

                newCanvas.Text = inputFileTxt.Substring(ino + 1, inputFileTxt.Length - ino - 1);  //...filename is extracted from the text...(blabla.dxf)...
                newCanvas.MinimumSize = new Size(500, 400);		//...canvas minimum size is set...


                if (inputFileTxt.Length > 0)
                {
                    newCanvas.ReadFromFile(inputFileTxt);		//the filename is sent to the method for data extraction and interpretation...
                }



                newCanvas.Show();							//the canvas is displayed...
                newCanvas.Activate();
                newCanvas.Focus();

            }

            openFileDialog1.Dispose();

        }

        private void menuItem4_Click(object sender, System.EventArgs e)		//exits program...
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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
                    writeThread = new Thread(writePort);
                    writeThread.Name = "write thread";
                    button1.Text = "关闭串口";
                    pictureBox1.Image = AGV.Properties.Resources.ICON_ON;
                    readThread.Start();
                    writeThread.Start();
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
                    button1.Text = "打开串口";
                    pictureBox1.Image = AGV.Properties.Resources.ICON_OFF;
                    if (serialPort1.IsOpen)
                        serialPort1.Close();
                    if(readThread != null)
                        readThread.Abort();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        public class RoadTableEventArgs
        {
            private Station startStation;
            private Station targetStation;
            private Station endStation;
            public RoadTableEventArgs(Station startStation, Station targetStation, Station endStation)
            {
                this.startStation = startStation;
                this.targetStation = targetStation;
                this.endStation = endStation;
            }
        }


        public class RoadTableFrameHandler
        {
            private List<byte> serialBuf = new List<byte>();
            private RoadTableEventArgs eArgs = null;
            public delegate void SeialEventHandler(Station startStation, Station targetStation, Station endStation);
            public event SeialEventHandler serialEvent;
            public void accessRoadTable(Station startStation, Station targetStation, Station endStation, AdjacencyList adj, SerialPort serialPort1,Canvas canvas)
            {
                List<Track> list1 = adj.FindWay(adj.Find(startStation), adj.Find(targetStation));
                List<Track> list2 = adj.FindWay(adj.Find(targetStation), adj.Find(endStation));
                List<byte> command = new List<byte>();
                for (int i = 0; i < list1.Count; ++i)
                {
                    if (list1[i].CarAction != null)
                    {
                        string station = list1[i].CarAction.Substring(0, list1[i].CarAction.IndexOf('G'));
                        command.Add((byte)canvas.StationDic[station].CardID);
                        switch (list1[i].CarAction.Substring(list1[i].CarAction.Length-1, 1))
                        {
                            case "L":
                                command.Add((byte)01);
                                break;
                            case "R":
                                command.Add((byte)02);
                                break;
                            case "S":
                                command.Add((byte)03);
                                break;
                        }
                    } 
                }
                command.Add((byte)canvas.StationDic[targetStation.Name].CardID);
                command.Add((byte)04);
                for (int i = 0; i < list2.Count; ++i)
                {
                    if (list2[i].CarAction != null)
                    {
                        string station = list2[i].CarAction.Substring(0, list2[i].CarAction.IndexOf('G'));
                        command.Add((byte)canvas.StationDic[station].CardID);
                        switch (list2[i].CarAction.Substring(list2[i].CarAction.Length - 1, 1))
                        {
                            case "L":
                                command.Add((byte)01);
                                break;
                            case "R":
                                command.Add((byte)02);
                                break;
                            case "S":
                                command.Add((byte)03);
                                break;
                        }
                    }
                }
                command.Add((byte)canvas.StationDic[startStation.Name].CardID);
                command.Add((byte)04);
                //for (int j = 0; j < command.Count; ++j)
                //{
                //    Console.Write(command[j] + " ");
                //}
                if (command != null && command.Count <= 28)
                {
                    byte CardAccount = (byte)(command.Count >> 1);
                    command.Insert(0, CardAccount);
                    command.Insert(0, CardAccount);
                    command.Insert(0, (byte)1);
                    command.Insert(0, (byte)canvas.StationDic[startStation.Name].OccupiedCar.CardID);
                    command.Insert(0, (byte)0);
                    command.Insert(0, (byte)0);
                    command.Insert(0, (byte)(CardAccount*2+4));
                    command.Insert(0, (byte)0xe6);
                    command.Add(checksum(command));
                    command.Insert(0, (byte)0x68);
                    byte[] roadTable = new byte[command.Count];
                    command.CopyTo(roadTable);
                    for (int j = 0; j < roadTable.Length; ++j)
                    {
                        Console.Write(roadTable[j] + " ");
                    }
                    serialPort1.Write(roadTable, 0, roadTable.Length);
                    
                }
            }
            public byte checksum(List<byte> command)
            {
                int sum = 0;
                for (int i = 0; i < command.Count; ++i)
                {
                    sum = sum + (int)command[i];
                }
                return (byte)(sum % 256);
            }
        }

    }

     

}
