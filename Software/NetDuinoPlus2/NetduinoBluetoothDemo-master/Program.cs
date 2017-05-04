using System.Collections;
using System.IO.Ports;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

// follow instructions to load dev env. under netduino 3, netduino 2, and go
// (this is a netduino plus 2).
// i updated the firmware to 4.3.2.1
// http://www.netduino.com/downloads/
// https://learn.sparkfun.com/tutorials/using-the-bluesmirf
// http://www.netduino.com/downloads/gettingstarted.pdf
// https://msdn.microsoft.com/en-us/subscriptions/downloads/

// Test with Hyperterminal - find the Com port you connect to, be sure to enable line feeds.

namespace NetduinoApplication1
{

    public class Program
    {
        static RovingNetworks rn42; 

        // Create a method for a delegate.
        public static void DelegateMethod(string message)
        {
            Debug.Print(message);
            rn42.write(message);
        }
        public static void Main()
        {
            // flash the led to say hi
            OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
            for (int i = 0; i < 5; i++)
            {
                led.Write(true);
                Thread.Sleep(250);
                led.Write(false);
                Thread.Sleep(250);
            }
            Debug.Print("flashed LEDs 5 times, now setting up BT");
            
            //rn42 = new RovingNetworks(Serial.COM1, BaudRate.Baudrate115200, Parity.None, 8, StopBits.None);
            //Debug.Print("exiting command mode");
            rn42 = new RovingNetworks(Serial.COM1, BaudRate.Baudrate115200, Parity.None, 8, StopBits.One); 
            rn42.ExitCommandModeJustInCase();

            int packetID = 0;
            bool _DEBUGMODE = false;
            AnalogInput PINA0 = new AnalogInput(AnalogChannels.ANALOG_PIN_A0);

            OutputPort PIND0 = new OutputPort(Cpu.Pin.GPIO_Pin0, false);

            BTSettings.mode = BTSettings.Mode.ACTIVE;

            for (;;)
            {
                string adc_val = PINA0.ReadRaw().ToString();

                if (_DEBUGMODE)
                {
                    rn42.write(adc_val);
                    rn42.write("\r\n");
                }
                else
                {
                    if (BTSettings.mode == BTSettings.Mode.ACTIVE)
                    {
                        byte[] adc = System.Text.Encoding.UTF8.GetBytes(adc_val);

                        BTMessage msg = new BTMessage(BTMessage.CmdType.ReadADC, adc, packetID++, false);
                        rn42.WriteBytes(msg.Packet);
                    }
                }
                System.Threading.Thread.Sleep(BTSettings.TX_MS); // only run every 10th of a second so that we don't completely overwhelm any components.
            }
            
           

            //Thread.Sleep(Timeout.Infinite);

            //rn42.CommandEvent = c.CommandEvent;
            //rn42.CommsCommands = c.CommsCommands;
            //// use a symmetric event wait for both input from Serial (bt) port, and from Brain
            //ArrayList newCommands = new ArrayList();
            //while (true)
            //{
            //    c.CommandEvent.WaitOne(); // wait forever, or should i timeout just in case i missed an event?
            //    lock (c.thelock)
            //    {
            //        //if (CommsCommands.Count == 0) getGo = true;
            //        while (c.CommsCommands.Count > 0)
            //        {
            //            // service the queue
            //           // newCommands.Add(CommsCommands[0]);
            //            string returnString = "You said:" + c.CommsCommands[0];
            //            Debug.Print(returnString);
            //            c.CommsCommands.RemoveAt(0);
            //            rn42.write(returnString);
            //        }
            //    }
            //    // dispatch commands
            //    //dispatchCommands(newCommands);
            //    //newCommands.Clear();
            //}
            //Debug.Print("entering command mode");
            //if (rn42.EnterCommandMode())
            //{
            //    Debug.Print("Success");
            //    ArrayList result = rn42.SendCommand("D\r\n", 500);
            //    foreach (string str in result)
            //    {
            //        Debug.Print(str);
            //    }
            //    //Thread.Sleep(Timeout.Infinite);
            //}
            //else
            //{
            //    Debug.Print("Oh dear...");
            //}
            //if (rn42.ExitCommandMode())
            //{
            //    Debug.Print("exited command mode");
            //}
            //else
            //{
            //    Debug.Print("Oh Dear 2...");
            //}
        }

    }
}
