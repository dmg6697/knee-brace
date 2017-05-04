using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P17012CEF
{
    public static class Logger
    {
        private static bool _activated = false;
        private static string filename = "";
        private static string path = "";
        private static Thread _writeThread;
        private static List<string> _queue;

        public static void Activate()
        {
            if (!_activated)
            {
                _queue = new List<string>();
                _writeThread = new Thread(WriteMsg);

                filename = "P17012_" + DateTime.UtcNow.ToShortDateString().Replace("/", "") + ".log";
                path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
                _activated = true;

                _writeThread.Start();
            }
        }

        public static void Deactivate()
        {
            if (_activated)
            {
                _activated = false;
                
                if (_writeThread.IsAlive)
                {
                    try
                    {
                        _writeThread.Abort();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Killed _writeThread in Logger.cs");
                    }
                }
            }
        }

        private static void WriteMsg()
        {
            while (_activated)
            {
                string msg = "";
                lock (_queue)
                {
                    if (_queue.Count > 0)
                    {
                        msg = String.Join("\n", _queue.ToArray());
                        _queue = new List<string>();
                    }
                }

                if (!msg.Equals(""))
                {
                    try
                    {
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.AppendAllText(path, msg);
                        }
                        else
                        {
                            System.IO.File.WriteAllText(path, msg);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to write to '{0}'", filename);
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static void Log(string message)
        {
            if (_activated && !message.Equals(""))
            {
                lock (_queue)
                {
                    _queue.Add(message);
                }
            }
        }
    }
}
