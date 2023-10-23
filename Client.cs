using System.Net;
using System.Net.Sockets;


namespace SD
{
    class Client
    {
        Client()
        {

        }

        async void Connect()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new(IPAddress.Parse(""), 11_000);
            using Socket client = new Socket(new AddressFamily(), SocketType.Stream, ProtocolType.IPv4);
            client.Accept();
            client.Connect(ipEndPoint);
            Console.WriteLine("Hello, World!");
        }
    }
}