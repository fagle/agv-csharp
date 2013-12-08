using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.Data.SQLite;

namespace CC
{

    public partial class serial : Form
    {
        enum eSerialState { SerialOn, SerialOff };
        Thread readThread;
        private eSerialState serialState = eSerialState.SerialOff;
        public serial()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox1.SelectedItem = comboBox1.Items[0];
            InitPort();            
        }

        public int InitPort()
        {
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
            serialPort1.BaudRate = 115200;
            serialPort1.DataBits = 8;
            return 0;
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        public void ReadPort()
        {
            while (true)
            {
                if (serialPort1.IsOpen)
                {
                    if (serialPort1.BytesToRead == 0)
                        Thread.Sleep(100);
                    try
                    {
                        byte b = (byte)serialPort1.ReadByte();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialState == eSerialState.SerialOff)
            {
                try
                {
                    InitPort();
                    serialPort1.Open();
                    serialState = eSerialState.SerialOn;
                    readThread = new Thread(ReadPort);
                    readThread.Name = "read thread";
                    button1.Text = "关闭串口";                   
                    comboBox1.Enabled = false;
                    readThread.Start();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);                    ;
                    return;
                }
            }
            else
            {
                try
                {
                    serialState = eSerialState.SerialOff;
                    button1.Text = "打开串口";
                    comboBox1.Enabled = true;
                    if (serialPort1.IsOpen)
                        serialPort1.Close();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);                    
                    return;
                }
            }
        }

        bool SerialPortIsReceiving = false;        
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPortIsReceiving = true;

            try
            {
                AppendTextBox(serialPort1.ReadExisting());
            }
            catch { }

            SerialPortIsReceiving = false;
        }

        delegate void SetTextCallback(string text);

        private void AppendTextBox(string text)
        {
            try
            {
                if (textBox1.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(AppendTextBox);
                    this.Invoke(d, text);
                }
                else
                {
                    textBox1.SuspendLayout();
                    if (text.Length == 1 && text[0] == (char)0x08)
                    {
                        if (textBox1.Text.Length > 0)
                        {
                            textBox1.SelectionStart = textBox1.Text.Length - 1;
                            textBox1.SelectionLength = 1;
                            textBox1.SelectedText = "";
                        }
                    }
                    else
                    {
                        textBox1.AppendText(text);
                    }
                    if (textBox1.Text.Length > 100000)
                    {
                        textBox1.Text = textBox1.Text.Substring(50000, textBox1.Text.Length - 50000);
                    }
                    textBox1.ResumeLayout(false);
                }
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("请先打开串口！");
                return;
            }
            if (comboBox2.Text == "")
            {
                MessageBox.Show("请先选择操作对象！");
                return;
            }
            string s = textBox2.Text;
            string l = comboBox2.Text;
            if (s.Length < 3)
            {
                MessageBox.Show("格式不正确，请查点击lamphelp或者roadhelp！", "提示！");
            }
            else
            {
                switch (l + " -" + s.Substring(0, 1))
                {
                    case "road -o":
                        serialPort1.Write("road -" + s + "\n\r");
                        break;
                    case "road -c":
                        serialPort1.Write("road -" + s + "\n\r");
                        break;
                    case "road -m":
                        serialPort1.Write("road -" + s + "\n\r");
                        break;
                    case "road -x":
                        serialPort1.Write("road -" + s + "\n\r");
                        break;
                    case "road -r":                        
                        serialPort1.Write("road -" + s + "\n\r");
                        break;
                    case "road -n":
                        serialPort1.Write("road -" + s + "\n\r");
                        break;
                    case "road -p":
                        serialPort1.Write("road -" + s + "\n\r");
                        break;
                    case "lamp -d":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -o":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -c":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -t":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -n":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -w":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -i":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -l":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -s":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    case "lamp -g":
                        serialPort1.Write("lamp -" + s + "\n\r");
                        break;
                    default:
                        MessageBox.Show("格式不正确，请查点击lamphelp或者roadhelp！", "提示！");
                        break;
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("请先打开串口！");
                return;
            }
            serialPort1.Write("lamp\n\r");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            RoadHelp r = new RoadHelp();
            //r.ShowDialog();
            r.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            LampHelp l = new LampHelp();
            l.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("请先打开串口！");
                return;
            }
            serialPort1.Write("road\n\r");
        }

    
    }
}