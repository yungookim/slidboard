using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Threading;

namespace slidbaord
{

    public class SocketClient
    {
        private String ip;
        private int port;
        private TcpClient clientSocket;
        private NetworkStream serverStream;
        
        //Listening Thread
        private Thread listenThread;
        //Following variable will be accessed by multiple threads.
        private volatile Boolean keep_alive = false;

        public SocketClient(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.clientSocket = new TcpClient();
        }

        //Establish a connection to the server
        public void connect()
        {
            //Establish a connection, get the stream.
            this.clientSocket.Connect(this.ip, this.port);
            this.serverStream = clientSocket.GetStream();
           
            //Prepare initiating JSON message to send
            JSONMessageWrapper _msg = new JSONMessageWrapper("init", "");
            
            //Tell the server who I am
            this.write(_msg.getMessage());
            //this.listen();
        }

        public void close()
        {
            this.write("end");
            //this.listenThread.Join();
            this.clientSocket.Close();
        }

        public void write(String val)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(val);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public void listen()
        {
            byte[] inStream = new byte[clientSocket.Available];
            //Following function blocks. Wait til the server responds then go on.
            //BUG: The current worker thread does not shut down gracefully.
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Waiting for msg");
            String msg = "";

            serverStream.Read(inStream, 0, clientSocket.Available);
            
            msg = System.Text.Encoding.ASCII.GetString(inStream);

            Console.WriteLine("Server Says : {0}\r\n", msg);
            Console.WriteLine("------------------------------------------------");
        }

        public TcpClient getSocket()
        {
            return this.clientSocket;
        }
    }
}
