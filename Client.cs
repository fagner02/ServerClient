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
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new(ipAddress, 1100);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine(localEndPoint.Address);
            client.Connect(localEndPoint);
            // client.Accept();

            var message = "Hi friends 👋!<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            int bytes = client.Send(messageBytes);

            Console.WriteLine(bytes);
            // client.
            // client.Shutdown(SocketShutdown.Both);
            client.Close();
            return;
        }
    }
}