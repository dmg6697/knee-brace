using System;
using System.Text;
using System.Timers;

namespace P17012CEF
{
    public class BTMessage
    {
        public enum PacketConstants : byte
        {
            SOH = 0x01, // Start of Header
            SOT = 0x02, // Start of Text
            ETX = 0x03, // End of Text
            EOT = 0x04, // End of Transmission
            ACK = 0x06, // Successful transmit acknowledge
            NAK = 0x15  // Unsuccessful transmit acknowledge
        }

        public enum CmdType : byte
        {
            ReadADC,
            WriteSetting,
            Acknowledge
        }

        private const int _packetlength = 11;

        private byte[] _packet;
        public byte[] Packet
        {
            get
            {
                return _packet;
            }
        }

        private CmdType _cmdType;
        private byte[] _data;
        private int _packetID;

        public BTMessage(CmdType cmd, byte[] payload, int packetID, bool RequiresAck)
        {

            _packet = new byte[_packetlength];
            _packet[0] = (byte)PacketConstants.SOH;
            _packet[1] = (byte)packetID;
            _packet[2] = (byte)cmd;
            _packet[3] = (byte)(RequiresAck ? 1 : 0);
            _packet[4] = (byte)PacketConstants.SOT;

            if (payload.Length >= 4)
            {
                _packet[5] = payload[0];
                _packet[6] = payload[1];
                _packet[7] = payload[2];
                _packet[8] = payload[3];
            }

            _packet[9] = (byte)PacketConstants.ETX;
            _packet[10] = (byte)PacketConstants.EOT;

            _isValidPacket = true;

            _data = new byte[2];
            Array.Copy(payload, _data, 2);
            _cmdType = cmd;
            _packetID = packetID;
        }

        public BTMessage(byte[] packet)
        {
            _packet = new byte[_packetlength];
            Array.Copy(packet, _packet, _packet.Length);

            if (_packet[0] == (byte)PacketConstants.SOH &&
                _packet[4] == (byte)PacketConstants.SOT &&
                _packet[9] == (byte)PacketConstants.ETX &&
                _packet[10] == (byte)PacketConstants.EOT)
            {
                _isValidPacket = true;
            }
            else
            {
                _isValidPacket = false;
            }

            _data = new byte[4];
            _data[0] = _packet[5];
            _data[1] = _packet[6];
            _data[2] = _packet[7];
            _data[3] = _packet[8];
            _cmdType = (CmdType)_packet[2];
            _packetID = (int)_packet[1];
        }

        private const int _requiresAck = 3;

        public bool RequiresAck
        {
            get
            {
                return _packet[_requiresAck] == 0x01;
            }
        }


        private bool _isValidPacket = false;
        public bool IsValidPacket
        {
            get
            {
                return _isValidPacket;
            }
        }

        public CmdType Type
        {
            get
            {
                return _cmdType;
            }
        }

        public byte[] DataPayload
        {
            get
            {
                return _data;
            }
            set
            {
                int length = value.Length;
                if (length > 2)
                {
                    length = 2;
                }
                Array.Copy(value, _data, length);
            }
        }

        public int PacketID
        {
            get
            {
                return _packetID;
            }
        }

        private Timer _timer;
        private AckExpiredEvent _ackExpiredHandler;
        public delegate void AckExpiredEvent(BTMessage msg);
        public event AckExpiredEvent AckExpired
        {
            add
            {
                _ackExpiredHandler += value;
                Console.WriteLine("Added handler to AckExpired.");
            }
            remove
            {
                _ackExpiredHandler -= value;
                Console.WriteLine("Removed handler from AckExpired.");
            }
        }
        

        public void ActivateAck(int timeout)
        {
            _timer = new Timer(timeout);
            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        public void ReceivedAck()
        {
            _timer.Stop();
            _timer.Elapsed -= _timer_Elapsed;
            _timer.Dispose();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_ackExpiredHandler != null)
            {
                _ackExpiredHandler(this);
            }
            _timer.Dispose();
        }

        private static ushort _nextPacketID = 0;
        internal static int NextPacketID()
        {
            return _nextPacketID++;
        }
    }
}