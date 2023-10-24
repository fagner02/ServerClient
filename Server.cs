using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{

    static class Server
    {

        public static void Setup()
        {
            try
            {
                IPEndPoint localEndPoint = new(IPAddress.Parse("192.168.100.125"), 1100);
                Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine(localEndPoint.Address);
                server.Bind(localEndPoint);
                server.Listen(8090);
                Socket handler = server.Accept();
                Console.WriteLine("Connected");
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytes = handler.Receive(buffer);

                    if (bytes > 0)
                    {
                        var response = Encoding.UTF8.GetString(buffer, 0, bytes);
                        Console.WriteLine(response);
                        return;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine("erro"); Console.WriteLine(e); return; }
        }
    }

}