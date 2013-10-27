using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGV
{
    public class CommandFrame
    {
        private byte Ptefix = 0xfe;
        private byte FrameHead = 0x68;
        private byte CommandWord;//命令字//主版本
        private byte DataLength;//改为byte型
        private short RoadId;//2011.9.28新增,改消息的道路ID
        private byte[] Data = null; //数据体
        private byte SumCheck;
        public byte Ptefix1
        {
            get { return Ptefix; }
            set { Ptefix = value; }
        }
        public byte FrameHead1
        {
            get { return FrameHead; }
            set { FrameHead = value; }
        }
        public byte CommandWord1
        {
            get { return CommandWord; }
            set { CommandWord = value; }
        }
        /*public byte DataLength1
        {
            get { return DataLength; }
            set { DataLength = value; }
        }*/
        public short RoadId1
        {
            get { return RoadId; }
            set { RoadId = value; }
        }
        public byte[] Data1
        {
            get { return Data; }
            set { 
                Data = value;
                this.DataLength = (byte)Data.Length;
            }
        }
        public byte SumCheck1
        {
            get { return SumCheck; }
            set { SumCheck = value; }
        }
        public CommandFrame()
        {
            this.CommandWord = 0;
            this.DataLength = 0;
            this.RoadId = 0;//0x20;
            this.Data = null;
            this.SumCheck = 0;
        }
        public CommandFrame(byte command, short roadId, byte[] data)
        {
            this.CommandWord = command;
            this.DataLength = (byte)data.Length;
            this.RoadId = roadId;
            this.Data = data;
            this.SumCheck = 0;

        }
        public CommandFrame(byte command, short roadId, byte[] data, byte cnt_rev)
        {
            this.CommandWord = command;
            this.DataLength = (byte)data.Length;
            this.RoadId = roadId;
            this.Data = data;
            this.SumCheck = this.CalSumCheckFunction();
        }
        public byte[] GetFrameToBytes()
        {
            int len = this.DataLength + 7;//dataLength + extraSize;
            byte[] buffer = new byte[len];
            int i = 0;
            buffer[i++] = this.Ptefix;//1 0xfe
            buffer[i++] = this.FrameHead;//2 0x68
            buffer[i++] = this.CommandWord;//3 0xEE
            buffer[i++] = this.DataLength;//4 3
            buffer[i++] = (byte)0;//(this.RoadId & 0x00FF);//last 8bit
            buffer[i++] = (byte)0;//((this.RoadId & 0xFF00) >> 0x08);//front 8bit
            if (this.Data != null)
            {
                foreach (byte data in this.Data)
                {
                     buffer[i++] = data; //ASCII                    
                }
            }
            buffer[i] = this.CalSumCheckFunction(); 
            return buffer;
        }
        public byte CalSumCheckFunction()
        {
            byte sum = (byte)(this.CommandWord + this.DataLength + this.RoadId);

            for (int i = 0; i < this.DataLength; i++)
            {
                try
                {
                    sum += this.Data[i];
                }
                catch (Exception exp)
                {
                    System.Console.WriteLine(exp.Message);
                }
            }

            return sum;
        }
        public void CalSumCheck()
        {
            byte sum = (byte)(this.CommandWord + this.DataLength + this.RoadId);

            for (int i = 0; i < this.DataLength; i++)
            {
                sum += this.Data[i];
            }

            this.SumCheck = sum;
        }
    }
}
