using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{

    class Server
    {
        public Server()
        {

        }

        public static void Setup()
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.100.11"), 11000);
                Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine(localEndPoint.Address);
                server.Bind(localEndPoint);
                server.Listen(11000);
                Socket handler = server.Accept();
                Console.WriteLine("Connected");
                while (true)
                {
                    ArraySegment<byte> buffer = new();
                    int bytes = handler.Receive(buffer);
                    if (bytes > 0)
                    {
                        var response = Encoding.UTF8.GetString(buffer.Array!, 0, bytes);
                        Console.WriteLine(buffer);
                        return;
                    }
                }
                Console.WriteLine("Hello, World!");

            }
            catch (Exception e) { Console.WriteLine("erro"); Console.WriteLine(e); return; }
        }
    }

}