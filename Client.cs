using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    static class Client
    {
        public static void Connect(string message = "Hello coisa")
        {
            IPEndPoint ipEndPoint = new(IPAddress.Parse("172.25.238.106"), 1100);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndPoint);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            int bytes = client.Send(messageBytes);

            byte[] buffer = new byte[1024];
            string response = "";
            while (true)
            {
                int resBytes = client.Receive(buffer);
                if (bytes == 0) break;
                response += Encoding.UTF8.GetString(buffer, 0, resBytes);
            }
            Console.WriteLine(response);
            Console.WriteLine(bytes);
            client.Close();
            return;
        }
    }
}