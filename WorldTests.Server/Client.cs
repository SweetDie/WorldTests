using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WorldTests.Server
{
    public class Client
    {
        public Guid Id { get; set; }
        public TcpClient TcpClient;
        public NetworkStream NetworkStream { get; set; }
        public byte[] Buffer { get; set; }

        public Client(TcpClient tcpClient, long bufferSize)
        {
            TcpClient = tcpClient;
            NetworkStream = tcpClient.GetStream();
            Id = Guid.NewGuid();
            Console.WriteLine($"Server -> client {Id} connected");
            Buffer = new byte[bufferSize];
        }

        public void Stop()
        {
            NetworkStream.Close();
            TcpClient.Close();
        }
    }
}
