using System;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Text;


namespace Shmessenger
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 80);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                while (client.Connected)
                {
                    byte[] recieve = new byte[2048];
                    string recievedMessage;
                    stream.Read(recieve, 0, recieve.Length);
                    recievedMessage = Encoding.ASCII.GetString(recieve).Replace("\0", string.Empty);
                    Console.WriteLine($"You recieved message from {client.Client.RemoteEndPoint.AddressFamily}:\n");
                    Console.WriteLine(recievedMessage);
                    client.Close();
                }//Отримання повідомлення


            }
            
        }
    }
}
