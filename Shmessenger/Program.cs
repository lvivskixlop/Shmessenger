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
            Thread recieveThread = new Thread(recieveMessage);
            recieveThread.Start();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    Console.WriteLine("You pressed any key. It seems like you want something. What is it?\nSend Message(y)/Continiue waiting(n) ");
                    string answer = Console.ReadLine();
                    if (answer == "y")
                    {
                        sendMessage();
                    }
                    else if(answer == "n")
                    {
                        Console.WriteLine("ok");
                    }
                    else
                    {
                        Console.WriteLine("YOUR ANSWER IS WRONG! GTFO!");
                        break;
                    }
                }
            }
            recieveThread.Abort();
            
        }

        static void sendMessage()
        {
            TcpClient client = new TcpClient();
            Console.WriteLine("Input IP address you want to message to: ");
            string ipString = Console.ReadLine();
            IPAddress recieverIP = IPAddress.Parse(ipString);            

            client.Connect(recieverIP, 80);

            byte[] send = new byte[2048];
            Console.WriteLine("Type your message here: \n");
            string messageString = Console.ReadLine();
            send = Encoding.ASCII.GetBytes(messageString);
            NetworkStream stream = client.GetStream();
            stream.Write(send, 0, send.Length);
            stream.Close();
            client.Close();
            Console.WriteLine("Message sent.");
        }//Надіслати повідомлення

        static void recieveMessage()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 80);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            while (client.Connected)
            {
                Console.WriteLine("poop");
                byte[] recieve = new byte[2048];
                string recievedMessage;
                stream.Read(recieve, 0, recieve.Length);
                recievedMessage = Encoding.ASCII.GetString(recieve).Replace("\0", string.Empty);
                Console.WriteLine($"You recieved message from {client.Client.RemoteEndPoint.AddressFamily}:\n");
                Console.WriteLine(recievedMessage);
                
            }
        }//Отримання повідомлення

        

    }
}
