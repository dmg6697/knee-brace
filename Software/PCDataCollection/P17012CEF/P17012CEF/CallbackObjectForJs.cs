using System;

namespace P17012CEF
{
    public class CallbackObjectForJs
    {
        // TODO: potentially split messages into types (error vs. data)
        /// <summary>
        /// Event handlers & delegates for Chromium instance so that we can centralize calls from JavaScript
        /// </summary>
        /// <param name="message"></param>
        public delegate void JSMessageRx(object message);

        private JSMessageRx _jshandler;
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

        /// <summary>
        /// Method registered with the Chromium instance in order to receive messages via the raiseEvent call in JavaScript.
        /// </summary>
        /// <param name="msg"></param>
        public void raiseEvent(object msg)
        {
            _jshandler?.Invoke(msg);
        }
    }
}