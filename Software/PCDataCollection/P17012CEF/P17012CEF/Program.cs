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
            btc = BluetoothCommunicator.Instance("COM5", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One); // bt communicator instance
            jsc = new CallbackObjectForJs(); // javascript layer communications

            //Logger.Activate(); // optional activation of the logger so that we can dump out readings to a text file for analysis later.

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new P17012Form());

            //if (btc.IsConnected) // determined that this may be problematic since we'd have to wait on the connection.  Easier to register an event to handle the btc
            //{
            //    Application.Run(new P17012Form());
            //}
        }
    }
}
