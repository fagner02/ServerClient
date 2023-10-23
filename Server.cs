using System.Net;
using System.Net.Sockets;

namespace SD
{

    class Server
    {
        public Server()
        {

        }

        public static async Task<bool> Setup()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new(ipAddress, 11_000);
            using Socket server = new Socket(AddressFamily.Ipx, SocketType.Stream, ProtocolType.IPv4);

            Console.WriteLine("Connecting");
            server.Bind(ipEndPoint);
            server.Listen(11000);
            Socket handler = await server.AcceptAsync();
            Console.WriteLine("Connected");
            while (true)
            {
                ArraySegment<byte> buffer = new();
                await handler.ReceiveAsync(buffer);
                Console.WriteLine(buffer);
                return true;
            }
            Console.WriteLine("Hello, World!");
            return false;
        }
    }

}