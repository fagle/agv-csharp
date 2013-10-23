using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avg
{
    class Monitor
    {
        private int _monitorSecond = 20000;
        public void set(int value)
        {
            this._monitorSecond = value;
        }
        public int getValue()
        {
            return this._monitorSecond;
        }

        


    }
}
