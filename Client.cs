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
            IPEndPoint ipEndPoint = new(IPAddress.Parse("192.168.100.11"), 1100);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndPoint);

            var message = "Hi friends 👋!<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            int bytes = client.Send(messageBytes);

            Console.WriteLine(bytes);
            client.Close();
            return;
        }
    }
}