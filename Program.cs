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
            var outputStream = new PessoasOutputStream(new Pessoa[] {
                new Pessoa {Cpf = 100000002, Nome = "Joana", Idade = 19},
                new Pessoa{Cpf = 111000123, Nome = "Chico", Idade = 30},
                new Pessoa {Cpf = 111111111, Nome = "João", Idade = 25},
                new Pessoa{Cpf = 123123123, Nome = "Seu Zé", Idade = 45},
                new Pessoa {Cpf = 999999999, Nome = "Francisca", Idade = 50},
                });
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