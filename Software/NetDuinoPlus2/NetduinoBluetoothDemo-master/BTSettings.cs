using System;
using Microsoft.SPOT;

namespace NetduinoApplication1
{
    public static class BTSettings
    {
        public enum Mode
        {
            STANDBY,
            ACTIVE
        }

        private static Mode _mode;
        public static Mode mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }

        private static int _tx_ms = 200;
        public static int TX_MS
        {
            get
            {
                return _tx_ms;
            }
            set
            {
                _mode = Mode.ACTIVE;
                if (value > 100)
                {
                    _tx_ms = value;
                }
                else if (value == 0)
                {
                    _mode = Mode.STANDBY;
                }
                else
                {
                    _tx_ms = 1000;
                }
            }
        }
    }
}
