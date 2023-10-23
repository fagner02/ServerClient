using SD;
class Program
{
    public static async void Main(string[] args)
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
                await client.Connect();
                break;
            case "server":
                Server server = new();
                await Server.Setup();
                break;
        }
    }
}