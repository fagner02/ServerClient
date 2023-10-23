using System.Net;
using System.Net.Sockets;

namespace SD
{

    class Server
    {
        Server()
        {

        }

        async void Setup()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new(ipAddress, 11_000);
            using Socket server = new Socket(new AddressFamily(), SocketType.Stream, ProtocolType.IPv4);
            server.Accept();
            server.Bind(ipEndPoint);
            Console.WriteLine("Hello, World!");
        }
    }

}