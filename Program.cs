using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace ThreadingAndWaitingForClients
{
    class Program
    {
        static IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),5555);
        static TcpListener tcpListener;
        static bool waitEnded = false;

        static void Main(string[] args)
        {

            tcpListener = new TcpListener(iPEndPoint);
            Parallel.Invoke(WaitForClient, WriteNonsense,ConnectToServer);
            Console.ReadKey();
        }


        private static void WaitForClient()
        {
            tcpListener.Start();
            do
            {
                Console.WriteLine("waiting...");
            } while (tcpListener.AcceptTcpClient() == null);

            Console.WriteLine("ClientConnected");
            tcpListener.Stop();
            waitEnded = true;
        }

        private static void WriteNonsense()
        {
            do
            {
                Console.Write("bljblb");
            }while(!waitEnded);
        }

        private static void ConnectToServer()
        {
            TcpClient myClient = new TcpClient();
            Thread.Sleep(3000);
            myClient.Connect(iPEndPoint);
        }
    }
       
    
}
