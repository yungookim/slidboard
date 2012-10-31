using System;
using System.Net;
using System.Net.Sockets;

namespace slidbaord
{

    public class AsyncTCPClient
    {
        //TCP Client
        private TcpClient tcpClient = null;

        public AsyncTCPClient(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void ConnectToServer()
        {
            try
            {
                tcpClient = new TcpClient(AddressFamily.InterNetwork);
                IPAddress[] remoteHost = Dns.GetHostAddresses(this.ip);

                //Start the async connect operation
                tcpClient.BeginConnect(remoteHost, this.port, new AsyncCallback(ConnectCallback),
                                        tcpClient);
            }
            catch (Exception ex)
            {
                Console.Write("Error : ");
                Console.WriteLine(ex);
            }
        }


        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                //We are connected successfully.

                NetworkStream networkStream = tcpClient.GetStream();

                byte[] buffer = new byte[tcpClient.ReceiveBufferSize];

                //Now we are connected start asyn read operation.

                networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
            }
            catch (Exception ex)
            {
                Console.Write("Error : ");
                Console.WriteLine(ex);
            }
        }

        /// Callback for Read operation
        private void ReadCallback(IAsyncResult result)
        {

            NetworkStream networkStream;

            try
            {
                networkStream = tcpClient.GetStream();
            }
            catch
            {
                Console.Write("Error : ");
                Console.WriteLine(ex);
                return;
            }

            byte[] buffer = result.AsyncState as byte[];
            string data = ASCIIEncoding.ASCII.GetString(buffer, 0, buffer.Length);

            //Do something with the data object here.
            Console.WriteLine(data);

            //Then start reading from the network again.
            networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);

        }

    }
}