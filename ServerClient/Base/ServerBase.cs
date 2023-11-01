using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace SD
{
    public class ServerBase
    {
        class ServerParams
        {
            public required Type? SystemType;
            public required MethodInfo Method;
        }
        public int Timeout = -1;
        public bool IsTimedOut;

        public void InstanceEndpoint(object? param)
        {
            ServerParams serverParams = (ServerParams)param!;
            string ip = RequestConfig.GetLocalIp().ToString();

            int port = serverParams.SystemType == null ?
                RequestConfig.GetRequestPort(serverParams.Method.Name, GetType()) :
                RequestConfig.GetRequestPort(serverParams.Method.Name, GetType(), serverParams.SystemType);
            IPEndPoint localEndPoint = new(IPAddress.Parse(ip), port);
            Console.WriteLine(GetType().Name + serverParams.Method.Name + " endpoint at " + localEndPoint);

            Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localEndPoint);
            server.Listen();

            CancellationTokenSource cts = new(Timeout);
            HandleConnect(server, serverParams.Method, cts.Token);
            server.Close();
            server.Dispose();
        }

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