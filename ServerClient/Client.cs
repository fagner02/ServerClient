using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    public class Client
    {
        /// <summary>
        /// Tipo do Server ao qual esse Client está relacionado
        /// </summary>
        readonly private Type ServerType;
        /// <summary>
        /// Tipo do SystemServerBase ao qual o Server está relacionado
        /// </summary>
        readonly private Type? SystemType = null;

        /// <param name="serverType">Tipo do Server ao qual esse Client está relacionado</param>
        /// <param name="systemType">Tipo do SystemServerBase ao qual o Server está relacionado</param>
        public Client(Type serverType, Type systemType)
        {
            ServerType = serverType;
            SystemType = systemType;
        }

        /// <param name="serverType">Tipo do Server ao qual esse Client está relacionado</param>
        public Client(Type serverType)
        {
            ServerType = serverType;
        }

        /// <summary>
        /// Faz uma request definida pelo method para o server
        /// </summary>
        /// <param name="method">Método de request do server</param>
        /// <param name="message">Mensagem a ser enviada na request</param>
        /// <returns>A resposta do server</returns>
        public string MakeRequest(string method, string? message = null)
        {
            int port;

            if (SystemType == null)
                port = RequestConfig.GetRequestPort(method, ServerType);
            else
                port = RequestConfig.GetRequestPort(method, ServerType, SystemType);

            IPEndPoint ipEndPoint = new(IPAddress.Parse("192.168.100.125"), port);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("connecting to " + ipEndPoint);

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

            Console.WriteLine(response);
            client.Close();
            return response;
        }
    }
}