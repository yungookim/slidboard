using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

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

        public void connect()
        {
            clientSocket.Connect(this.ip, this.port);
        }

        public void write(String msg)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(msg);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
    }
}
