using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace slidbaord
{
    class SocketServer
    {
        static string HOSTNAME = "127.0.0.1";
        static IPAddress ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
        private int port;
        private TcpListener listener;

        // Sample high score table data
        static Dictionary<string, int> highScoreTable = new Dictionary<string, int>() { 
            { "john", 1001 }, 
            { "ann", 1350 }, 
            { "bob", 1200 }, 
            { "roxy", 1199 } 
    };

        public SocketServer(int port)
        {
            this.port = port;
            this.listener = new TcpListener(ipAddress, this.port);
        }

        public void open()
        {
            this.listener.Start();
            Console.WriteLine("Server running, listening to port " + port + " at " + ipAddress);
            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                Task task = new Task(Service, TaskCreationOptions.LongRunning);
                task.Start();
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            listener.Stop();
        }

        private void Service()
        {
            while (true)
            {
                Socket socket = listener.AcceptSocket();

                Console.WriteLine("Connected: {0}", socket.RemoteEndPoint);
                try
                {
                    // Open the stream
                    Stream stream = new NetworkStream(socket);
                    StreamReader sr = new StreamReader(stream);
                    StreamWriter sw = new StreamWriter(stream);
                    sw.AutoFlush = true;

                    sw.WriteLine("{0} stats available", highScoreTable.Count);
                    while (true)
                    {
                        // Read name from client
                        string name = sr.ReadLine();
                        if (name == "" || name == null) break;

                        // Write score to client
                        if (highScoreTable.ContainsKey(name))
                            sw.WriteLine(highScoreTable[name]);
                        else
                            sw.WriteLine("Player '" + name + "' was not found.");

                    }
                    stream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("Disconnected: {0}", socket.RemoteEndPoint);
                socket.Close();
            }
        }
 

    }
}
