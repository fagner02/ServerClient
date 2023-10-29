using System.Text.Json;
using SD;
class Program
{
    public static void Main(string[] args)
    {
        // args = new string[] { "server" };
        if (args.Length < 1)
        {
            Console.WriteLine("cmd: client | server | test");
            return;
        }
        switch (args[0])
        {
            case "client":
                // Client.MakeRequest(nameof(VotoServer.WriteRequest), JsonSerializer.Serialize(new Candidato { Name = "Jon", Id = 0 }, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true }));
                Client client = new(typeof(AdminServer), typeof(VotingSystem));
                client.MakeRequestUdp();
                break;
            case "server":
                // VotoServer server = new();
                Udp<Admin> server = new();
                server.Setup();
                break;
            case "test":
                var outputStream = new PessoasOutputStream(new Pessoa[] {
                    new () {Cpf = 100000002, Nome = "Joana", Idade = 19},
                    new () {Cpf = 11100013, Nome = "Chico", Idade = 30},
                    new () {Cpf = 111111111, Nome = "João", Idade = 25},
                    new () {Cpf = 123123123, Nome = "Seu Zé", Idade = 45},
                    new () {Cpf = 999999999, Nome = "Francisca", Idade = 50},
                });
                outputStream.SaveToFile();
                outputStream.Print();
                outputStream.SendToServer();

                var inputStream = new PessoasInputStream();
                inputStream.ReadFromFile();
                inputStream.ReadFromConsole();
                inputStream.ReadFromServer();
                break;
            case "vote":
                SD.VotingSystem sistema = new();
                sistema.Run();
                break;
        }
        return;
    }
}