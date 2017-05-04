using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P17012CEF
{
    static class Program
    {
        public static BluetoothCommunicator btc;
        public static CallbackObjectForJs jsc;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // communications thread
            btc = BluetoothCommunicator.Instance("COM5", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            jsc = new CallbackObjectForJs();

            //Logger.Activate();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new P17012Form());

            //if (btc.IsConnected)
            //{
            //    Application.Run(new P17012Form());
            //}
        }
    }
}
