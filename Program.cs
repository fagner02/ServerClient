using System.Net;
using System.Net.Sockets;
using SD;
class Program
{
    public static void Main(string[] args)
    {

        if (args.Length < 1)
        {
            Console.WriteLine("cmd: client | server");
            var outputStream = new PessoasOutputStream(new Pessoa[] { new Pessoa { Cpf = 100000002, Nome = "Joana", Idade = 1999 } });
            outputStream.SaveToFile();
            outputStream.Print();
            outputStream.SendToServer();
            return;
        }
        switch (args[0])
        {
            case "client":
                Client.Connect();
                break;
            case "server":
                Server.Setup();
                break;
        }
        return;
    }
}