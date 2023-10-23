using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    class Client
    {
        public Client()
        {

        }

        public void Connect()
        {
            IPEndPoint ipEndPoint = new(IPAddress.Parse("192.168.100.11"), 11_000);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine(ipEndPoint.Address);
            client.Connect(ipEndPoint);
            // client.Accept();

            var message = "Hi friends 👋!<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _ = client.Send(messageBytes);
            Console.WriteLine("Hello, World!");
            return;
        }
    }
}