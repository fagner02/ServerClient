using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace SD
{
    class Server
    {
        public static List<int> j;
        class ClientRequest
        {
            public Socket handler;
            public ref Dictionary<string, VariantType> data;
        }
        public static void ProcessClientRequest(object handler)
        {
            Console.WriteLine("Connected");

            byte[] buffer = new byte[1024];
            string response = "";
            while (true)
            {
                int bytes = handler.Receive(buffer);
                if (bytes == 0) break;
                response += Encoding.UTF8.GetString(buffer, 0, bytes);
            }
            // pessoas = JsonSerializer.Deserialize<Pessoa[]>(response, new JsonSerializerOptions()
            // {
            //     IncludeFields = true,
            //     WriteIndented = true
            // }) ?? pessoas;
            handler.Send(Encoding.UTF8.GetBytes("Message sent"));
            handler.Close();
        }
        public static void ThreadTask()
        {

        }
        public static void Setup(ref Dictionary<string, VariantType> data)
        {
            try
            {
                Pessoa[] pessoas = Array.Empty<Pessoa>();
                IPEndPoint localEndPoint = new(IPAddress.Parse("172.25.238.106"), 1100);
                Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine(localEndPoint.Address);
                server.Bind(localEndPoint);
                server.Listen();
                j.Add(0);
                while (true)
                {
                    Socket handler = server.Accept();

                    Thread thread = new(new ParameterizedThreadStart(ProcessClientRequest));
                    thread.Start(new ClientRequest() as object);
                }
            }
            catch (Exception e) { Console.WriteLine("erro"); Console.WriteLine(e); return; }
        }

    }
}