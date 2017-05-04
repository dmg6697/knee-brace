using System;

namespace P17012CEF
{
    public class CallbackObjectForJs
    {
        public delegate void JSMessageRx(object message);

        private JSMessageRxHandler _jshandler;
        public event JSMessageRx MessageReceived
        {
            add
            {
                _jshandler += value;
                Console.WriteLine("Added handler to JSMsgRx");
            }
            remove
            {
                _jshandler -= value;
                Console.WriteLine("Removed handler to JSMsgRx");
            }
        }


        public void raiseEvent(object msg)
        {
            MessageReceived?.Invoke(msg);
        }
    }
}