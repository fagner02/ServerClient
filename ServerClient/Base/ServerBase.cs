using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace SD
{
    /// <summary>
    /// A classe base para servers
    /// </summary>
    public class ServerBase
    {
        class ServerParams
        {
            public required Type? SystemType;
            public required MethodInfo Method;
        }
        public int Timeout = -1;
        public bool IsTimedOut;

        /// <summary>
        /// Instancia um endpoint para o método recebido no ServerParams
        /// </summary>
        /// <param name="param">ServerParams contendo os método e o tipo de sistema caso exista </param>
        public void InstanceEndpoint(object? param)
        {
            ServerParams serverParams = (ServerParams)param!;
            string ip = RequestConfig.GetLocalIp().ToString();

            int port = serverParams.SystemType == null ?
                RequestConfig.GetRequestPort(serverParams.Method.Name, GetType()) :
                RequestConfig.GetRequestPort(serverParams.Method.Name, GetType(), serverParams.SystemType);
            IPEndPoint localEndPoint = new(IPAddress.Parse(ip), port);
            string GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
            string BLUE = Console.IsOutputRedirected ? "" : "\x1b[94m";
            string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{BLUE}{GetType().Name + serverParams.Method.Name}{NORMAL} endpoint at {GREEN}{localEndPoint}{NORMAL}");
            Console.ResetColor();

            Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localEndPoint);
            server.Listen();

            CancellationTokenSource cts = new(Timeout);
            HandleConnect(server, serverParams.Method, cts.Token);
            server.Close();
            server.Dispose();
        }

        /// <summary>
        /// Recebe as conexões do cliente criando uma nova thread para cada request do cliente
        /// </summary>
        /// <param name="server"></param>
        /// <param name="method"></param>
        /// <param name="cancellationToken"></param>
        public void HandleConnect(Socket server, MethodInfo method, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    IsTimedOut = true;
                    Console.WriteLine(GetType().Name + " timed out");
                    return;
                }
                try
                {
                    var res = server.AcceptAsync(cancellationToken).AsTask();
                    res.Wait(cancellationToken);

                    if (cancellationToken.IsCancellationRequested) { continue; }
                    if (!res.IsCompletedSuccessfully) continue;

                    Socket handler = res.Result;
                    Thread thread = new(() => method.Invoke(this, new object?[] { handler, cancellationToken }));
                    thread.Start();
                }
                catch { }
            }
        }

        /// <summary>
        /// Instancia um endpoint em uma nova thread para cada método de Request
        /// </summary>
        /// <param name="systemType">Tipo do sistema contendo esse server</param>
        public void Setup(Type? systemType = null)
        {
            RequestConfig.ResolveRequestMethods((method) =>
            {
                try
                {
                    Thread endpoint = new(new ParameterizedThreadStart(InstanceEndpoint));
                    endpoint.Start(new ServerParams() { Method = method, SystemType = systemType });
                }
                catch (Exception e) { Console.WriteLine("erro"); Console.WriteLine(e); return; }
            }, GetType());
        }
    }
}