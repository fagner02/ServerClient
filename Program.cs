using SD;
class Program
{
    public static void Main(string[] args)
    {
        switch (args[0])
        {
            case "client":
                Client client = new();
                client.Connect();
                break;
            case "server":
                Server server = new();
                server.Setup();
                break;
        }
    }
}