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
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
                Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine(ipAddress);
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