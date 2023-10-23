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

        public async void Connect()
        {
            IPEndPoint ipEndPoint = new(IPAddress.Parse("192.168.100.11"), 11_000);
            using Socket client = new(new AddressFamily(), SocketType.Stream, ProtocolType.IPv4);
            client.Accept();
            client.Connect(ipEndPoint);
            var message = "Hi friends 👋!<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _ = await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine("Hello, World!");
            return;
        }
    }
}