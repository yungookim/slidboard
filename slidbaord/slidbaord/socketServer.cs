using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace slidbaord
{
    class SocketServer
    {
        private int port;

        private TcpListener serverSocket;
        private IPAddress ipAddress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
        private IPEndPoint ipLocalEndPoint;
        private TcpClient clientSocket;

        public SocketServer(int port)
        {
            this.port = port;
            ipLocalEndPoint = new IPEndPoint(this.ipAddress, port);
            serverSocket = new TcpListener(ipLocalEndPoint);
            clientSocket = default(TcpClient);
        }

        public void open()
        {
            Console.WriteLine("Starting server socket.");
            serverSocket.Start();
            Console.WriteLine("Starting to accept client.");
            clientSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine("Listening on " + this.port);
            while (true)
            {
                try
                {
                    //Get client's stream
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[10025];

                    //Read in from the client
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);

                    Console.WriteLine(" >> Data from client - " + dataFromClient);
                    
                    /*
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                    */
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> exit");

        }

    }
}
