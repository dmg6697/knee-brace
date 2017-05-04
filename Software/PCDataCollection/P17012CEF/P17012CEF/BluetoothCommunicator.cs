using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace P17012CEF
{
    public class BluetoothCommunicator
    {
        private static BluetoothCommunicator _instance;
        private List<BTMessage> _txMsgQueue;
        private List<BTMessage> _rxMsgQueue;
        private List<byte> _rxByteQueue;
        private Dictionary<int, BTMessage> _noAck;
        private SerialPort _port;
        private int _txInterval = 100; // 0.1 seconds
        private int _ackExpiration = 30000; // 30 seconds

        public delegate void BTCMessageRx(BTMessage msg);

        private BTCMessageRx _MsgRxHandler;
        public event BTCMessageRx MessageReceived
        {
            add
            {
                _MsgRxHandler += value;
                Console.WriteLine("Added handler to MsgRx");
            }
            remove
            {
                _MsgRxHandler -= value;
                Console.WriteLine("Removed handler to MsgRx");
            }
        }

        private Thread _sendThread;

        private bool _connected = false;
        public bool IsConnected
        {
            get
            {
                return _connected;
            }
        }

        public static BluetoothCommunicator Instance(string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            if (_instance == null)
            {
                _instance = new BluetoothCommunicator(comPort, baudRate, parity, dataBits, stopBits);
            }
            return _instance;
        }

        // constructor
        private BluetoothCommunicator(string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _port = new SerialPort(comPort, baudRate, parity, dataBits, stopBits);
            _connected = false;
            _port.DataReceived += _port_DataReceived;
            _txMsgQueue = new List<BTMessage>();
            _rxMsgQueue = new List<BTMessage>();
            _rxByteQueue = new List<byte>();
            _noAck = new Dictionary<int, BTMessage>();
        }

        // destructor
        ~BluetoothCommunicator()
        {
            if (_sendThread != null)
            {
                _sendThread.Abort(); // this will raise an exception on the thread
                _sendThread = null;
            }
            if (_port != null)
            {
                if (_port.IsOpen)
                {
                    _port.Close();
                }
                _port.Dispose();
                _port = null;
            }
        }

        public void ConnectAsync(string COMPort = "")
        {
            Task.Factory.StartNew(delegate {
                bool connected = Connect(COMPort);
                string[] ports = SerialPort.GetPortNames();
                if (connected)
                {
                    ports = new string[0];
                }
                _ConnectedEventHandler?.Invoke(connected, ports);
            });
        }
        public delegate void ConnectedEvent(bool IsConnected, string[] portNames);

        private ConnectedEvent _ConnectedEventHandler;
        public event ConnectedEvent Connected
        {
            add
            {
                _ConnectedEventHandler += value;
                Console.WriteLine("Added handler to ConnectAsync");
            }
            remove
            {
                _ConnectedEventHandler -= value;
                Console.WriteLine("Removed handler to ConnectAsync");
            }
        }


        public bool Connect(string COMPort = "")
        {
            if (COMPort != "")
            {
                if (_port.IsOpen)
                {
                    _port.Close();
                }
                _port.PortName = COMPort;
            }
            try
            {  
                if (!_port.IsOpen)
                {
                    _port.Open();
                }

                if (_port.IsOpen)
                {
                    _sendThread = new Thread(_port_SendMessage);
                    _sendThread.Start();
                }

                return _port.IsOpen;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Disconnect()
        {
            if (_sendThread != null)
            {
                _sendThread.Abort(); // this will raise an exception
                _sendThread = null;
            }
            if (_port != null)
            {
                if (_port.IsOpen)
                {
                    _port.Close();
                }
            }
        }

        public void EnqueueMsg(BTMessage msg)
        {
            lock (_txMsgQueue) // make sure a dequeue operation doesn't mess this addition up
            {
                _txMsgQueue.Add(msg);
            }
        }

        private void DequeueMsg()
        {
            BTMessage msg = null;

            if (_port.IsOpen)
            {
                lock (_txMsgQueue) // make sure an enqueue operation doesn't mess this dequeue up
                {
                    if (_txMsgQueue.Count > 0)
                    {
                        msg = _txMsgQueue[0]; // get the next message off the queue

                        _port.Write(msg.Packet, 0, msg.Packet.Length); // write the dq'd message to the bt module   

                        _txMsgQueue.RemoveAt(0);
                    }
                }

                if (msg != null && msg.RequiresAck) // if we dq'd a message successfully msg is not null
                {
                    lock (_noAck) // make sure we do not violate an ack dq
                    {
                        msg.ActivateAck(_ackExpiration); // start the ack expiration count down
                        msg.AckExpired += EnqueueMsg; // register event to resend the message if we don't ack on time
                        _noAck.Add(msg.PacketID, msg); // enqueue expectation for ack for this message
                    }
                }
            }
            else
            {
                throw new Exception("Unable to enqueue message due to the port not being open.");
            }
        }

        private bool _firstBurst = true;
        /// <summary>
        /// Raises a MessageReceived Event when it receives a complete packet.  If a complete packet is not present, but there are enough bytes it assumes misalignment and will byte shift out until the packet is fully formed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)(sender);
            
            lock (sp)
            {
                sp.Encoding = Encoding.UTF8;
                while (sp.BytesToRead > 0 && sp.IsOpen) // read out all bytes
                {
                    byte rxByte = (byte)sp.ReadByte();

                    _rxByteQueue.Add(rxByte);
                }
            }

            if (_rxByteQueue.Count >= 11) // could have a complete packet
            {
                // special case, start of message will always be 45 x3 bytes
                if (_firstBurst)
                {
                    _firstBurst = false;

                    while (_rxByteQueue[0] == 45)
                    {
                        _rxByteQueue.RemoveAt(0);
                    }
                }
                BTMessage msg = new BTMessage(_rxByteQueue.ToArray());

                if (msg.IsValidPacket)
                {
                    switch (msg.Type)
                    {
                        case BTMessage.CmdType.Acknowledge:
                            lock (_noAck)
                            {
                                if (_noAck.ContainsKey(msg.PacketID))
                                {
                                    _noAck[msg.PacketID].ReceivedAck();
                                    _noAck.Remove(msg.PacketID); // message has been ack'd, so dq
                                }
                            }
                            
                            break;
                        case BTMessage.CmdType.ReadADC:
                            _MsgRxHandler?.Invoke(msg); // invoke the event if there are subscribers
                            break;
                        case BTMessage.CmdType.WriteSetting:
                            // todo: implement
                            break;
                    }
                    
                    _rxByteQueue.RemoveRange(0, msg.Packet.Length); // remove the number of elements we need to pop
                }
                else // assume byte shifting
                {
                    _rxByteQueue.RemoveAt(0); // pop first element and try looking at the rest, until we realign
                }
            }
        }
        
        private void _port_SendMessage() // contained in a thread checking if there are enq'd mesages to send
        {
            try
            {
                for (;;)
                {
                    DequeueMsg();

                    System.Threading.Thread.Sleep(_txInterval);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().ToString() + ": " + ex.Message);
            }
        }
    }
}
