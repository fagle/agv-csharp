using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGV
{
    public class define
    {
        public const byte W108_CAR_UPDATE_CARID = 0XE0;//配置小车ID

        public const byte W108_CAR_CONSOLE_SEND_COMMAND = 0XF1;//发送小车cmd
      
       // public const byte W108_CAR_UPDATE_CHANNEL=0XE4;//配置小车CHANNEL
        
        public const byte W108_CAR_UPDATE_TICKTIME = 0XE6;//配置小车的心跳时间     

        public const byte W108_CAR_SHOW_CAR_INFO = 0XEA;
        
        public const byte W108_CAR_REBOOT_CAR = 0XEB;

        public const byte W108_CAR_ERROR = 0XE8;

        public const byte W108_CAR_ACK = 0XE9;
    }
}
