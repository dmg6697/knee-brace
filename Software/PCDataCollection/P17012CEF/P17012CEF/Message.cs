using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P17012CEF
{
    public class BTMessage
    {
        private byte[] _payload;

        /// <summary>
        /// Payload length in bytes allowed by each transmission/reception
        /// </summary>
        public const int MessageLength = 10;

        public BTMessage(byte[] payload)
        {

        }
    }
}
