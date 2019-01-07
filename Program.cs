using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

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
            TcpClient connectedClient;

            tcpListener.Start();
            do
            {
                connectedClient = tcpListener.AcceptTcpClient();
                Console.WriteLine("waiting...");
            } while (connectedClient == null);

            Console.WriteLine("ClientConnected");
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            //string sentFromClient = (string)binaryFormatter.Deserialize(connectedClient.GetStream());
            //Console.WriteLine(sentFromClient);
            AClass aclass = (AClass)binaryFormatter.Deserialize(connectedClient.GetStream());
            Console.WriteLine(aclass.eineZahl + " / " + aclass.einString);
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
            NetworkStream netStream =  myClient.GetStream();
            //string einText = "blubba";
            AClass aclass = new AClass();

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(netStream, aclass);
        }
    }
       
    [Serializable]
    public class AClass
    {
        public int eineZahl = 5;
        public string einString = "dakjföadjk";
    }
}
