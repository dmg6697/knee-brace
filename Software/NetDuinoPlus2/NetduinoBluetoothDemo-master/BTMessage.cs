using System;
using Microsoft.SPOT;
using System.Threading;

namespace NetduinoApplication1
{
    class BTMessage
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

            if (payload.Length == 4)
            {
                _packet[5] = payload[0];
                _packet[6] = payload[1];
                _packet[7] = payload[2];
                _packet[8] = payload[3];
            }

            _packet[9] = (byte)PacketConstants.ETX;
            _packet[10] = (byte)PacketConstants.EOT;

            _isValidPacket = true;

            _data = new byte[payload.Length];
            Array.Copy(payload, _data, payload.Length);
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

        public BTMessage(object[] p)
        {
            byte[] packet = new byte[p.Length];
            
            for (int i = 0; i < packet.Length; i++)
            {
                p[i] = (byte)packet[i];
            }

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

        public byte[] DataPayload()
        {
            return _data;
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
                Microsoft.SPOT.Debug.Print("Added handler to AckExpired.");
            }
            remove
            {
                _ackExpiredHandler -= value;
                Microsoft.SPOT.Debug.Print("Removed handler from AckExpired.");
            }
        }


        public void ActivateAck(int timeout)
        {
            _timer = new Timer(_timer_Elapsed, new object(), 30000, Timeout.Infinite);
        }

        private void _timer_Elapsed(object state)
        {
            throw new NotImplementedException();
        }

        public void ReceivedAck()
        {
            _timer.Dispose();
        }
    }
}
