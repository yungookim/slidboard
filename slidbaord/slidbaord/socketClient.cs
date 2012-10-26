using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace slidbaord
{

    public class SocketClient
    {
        
        TcpClient clientSocket;
        private String ip;
        private int port;

        public SocketClient(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.clientSocket = new TcpClient();
        }

        //Establish a connection to the server
        public void connect()
        {
            clientSocket.Connect(this.ip, this.port);
        }

        public void close()
        {
            clientSocket.Close();
        }

        public void write(String key, String val)
        {
            //Prepare JSON message to send
            Message _msg = new Message(key, val);
            String message = _msg.getMessage();

            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public TcpClient getSocket()
        {
            return this.clientSocket;
        }
    }

    //Converts message to JSON format that can be easily parsed by the server
    public class Message
    {
        //Global Constant.
        private const String CONNECTION_TYPE = "SERVER";
        private const String CONNECTION_FROM = "PixelSense";

        //Let the server know where the connection is coming from
        public String type = CONNECTION_TYPE;
        public String from = CONNECTION_FROM;
        public String key = "";
        public String value = "";

        public Message(String key, String val) 
        {
            this.key = key;
            this.value = val;
        }

        //Serialize current object to JSON then return
        public String getMessage(){
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}
