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
                IPEndPoint localEndPoint = new(ipAddress, 1100);
                Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine(localEndPoint.Address);
                server.Bind(localEndPoint);
                server.Listen(8080);
                Socket handler = server.Accept();
                // Console.WriteLine("Connected"); handler.Blocking = false;
                handler.ReceiveTimeout = 10;
                while (true)
                {
                    ArraySegment<byte> buffer = new();
                    int bytes = handler.Receive(buffer, SocketFlags.None);

                    Console.WriteLine(handler.Blocking);
                    if (buffer.Count > 0)
                    {
                        var response = Encoding.UTF8.GetString(buffer.Array!, 0, bytes);
                        Console.WriteLine(buffer);
                        return;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine("erro"); Console.WriteLine(e); return; }
        }
    }

}