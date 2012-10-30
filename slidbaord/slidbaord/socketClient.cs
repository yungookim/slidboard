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

            //Start listening on a dedicated thread
            this.listenThread = new Thread(new ThreadStart(listen));
            this.listenThread.Name = "TCP_LISENING_THREAD";
            this.keep_alive = true;
            this.listenThread.Start();
        }

        public void close()
        {
            this.keep_alive = false;
            this.write("END");
            //this.listenThread.Join();
            this.clientSocket.Close();
        }

        public void write(String val)
        {
            //Prepare JSON message to send
            Message _msg = new Message(val);
            String message = _msg.getMessage();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void listen()
        {
            
            byte[] inStream = new byte[10025];
            while (keep_alive)
            {
                Console.WriteLine("worker thread: working...");
                //Following function blocks. Wait til the server responds then go on.
                serverStream.Read(inStream, 0, 10025);
                string msg = System.Text.Encoding.ASCII.GetString(inStream);
                Console.WriteLine("Server Says :" + msg);
            }
            Console.WriteLine("worker thread: terminating gracefully.");
        }

        public TcpClient getSocket()
        {
            return this.clientSocket;
        }
    }

    //Converts message to JSON format that can be easily parsed by the server
    public class Message
    {
        private const String CONNECTION_FROM = "PixelSense";

        //Let the server know where the connection is coming from
        public String FROM = CONNECTION_FROM;
        public String msg = "";

        public Message(String data) 
        {
            this.msg = data;
        }

        //Serialize current object to JSON then return
        public String getMessage(){
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}
