using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace SD
{
    class Server
    {
        class ClientRequest
        {
            public required Socket handler;
        }
        public static void ProcessClientRequest(object? param)
        {
            ClientRequest clientRequest = (ClientRequest)param!;
            Socket handler = clientRequest.handler;
            Console.WriteLine("Connected");


            string response = "";
            while (true)
            {
                byte[] buffer = new byte[1024];
                Console.WriteLine("in");
                if (handler.Available == 0) break;
                int bytes = handler.Receive(buffer);
                Console.WriteLine(bytes);
                response = Encoding.UTF8.GetString(buffer, 0, bytes);
                Console.WriteLine(response);
            }
            // pessoas = JsonSerializer.Deserialize<Pessoa[]>(response, new JsonSerializerOptions()
            // {
            //     IncludeFields = true,
            //     WriteIndented = true
            // }) ?? pessoas;
            handler.Send(Encoding.UTF8.GetBytes("Message sent"));
            handler.Close();
            Console.WriteLine("Sent");
        }
        public static void ThreadTask()
        {

        }
        public static void Setup()
        {
            try
            {
                Pessoa[] pessoas = Array.Empty<Pessoa>();
                IPEndPoint localEndPoint = new(IPAddress.Parse("192.168.100.125"), 1100);
                Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine(localEndPoint.Address);
                server.Bind(localEndPoint);
                server.Listen();
                while (true)
                {
                    Socket handler = server.Accept();
                    Thread thread = new(new ParameterizedThreadStart(ProcessClientRequest));
                    thread.Start(new ClientRequest() { handler = handler });
                }
            }
            catch (Exception e) { Console.WriteLine("erro"); Console.WriteLine(e); return; }
        }

    }
}