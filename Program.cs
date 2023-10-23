using System.Net;
using System.Net.Sockets;
using SD;
class Program
{
    public static void Main(string[] args)
    {
        Call(args);
    }

    public static void Call(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("cmd: client | server");
            return;
        }
        switch (args[0])
        {
            case "client":
                Client client = new();
                client.Connect();
                break;
            case "server":
                Server.Setup();
                break;
        }
        return;
    }
}