using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    static class Client
    {
        public static void Connect(int port, Action callback, string? message = null)
        {
            IPEndPoint ipEndPoint = new(IPAddress.Parse("172.25.251.25"), port);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndPoint);

            if (message != null)
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                int bytes = client.Send(messageBytes);
            }

            string response = "";
            while (true)
            {
                byte[] buffer = new byte[1024];
                int resBytes = client.Receive(buffer);
                if (resBytes == 0) break;
                response += Encoding.UTF8.GetString(buffer, 0, resBytes);
            }

            callback();
            Console.WriteLine(response);
            client.Close();
            return;
        }
    }
}