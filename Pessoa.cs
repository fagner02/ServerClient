using System.Text;
using System.Text.Json;

namespace SD
{
    public class Pessoa
    {
        public required string Nome;
        public int Cpf;
        public int Idade;
    }

    public class PessoasOutputStream
    {
        readonly Pessoa[] Pessoas;
        public PessoasOutputStream(Pessoa[] pessoas)
        {
            Pessoas = pessoas;
        }
        public void SaveToFile()
        {
            FileStream file = new("Pessoas.txt", FileMode.Create);
            string encodedString = JsonSerializer.Serialize(Pessoas, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true });

            using (var writer = new BinaryWriter(file, Encoding.UTF8, false))
            {
                writer.Write(encodedString);
            }
            file.Close();
        }

        public void Print()
        {
            string encodedString = JsonSerializer.Serialize(Pessoas, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true });
            Console.WriteLine(encodedString);
        }

        public void SendToServer()
        {
            string encodedString = JsonSerializer.Serialize(Pessoas, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true });
            Client.Connect(1, () => { }, encodedString);
        }
    }

    public class PessoasInputStream
    {
        Pessoa[] Pessoas = Array.Empty<Pessoa>();

        public void ReadFromFile()
        {
            FileStream file = new("Pessoas.txt", FileMode.Open);
            string res;
            using (BinaryReader reader = new(file))
            {
                res = reader.ReadString();
            }
            Pessoas = JsonSerializer.Deserialize<Pessoa[]>(res, new JsonSerializerOptions() { IncludeFields = true }) ?? Pessoas;
            Console.WriteLine(Pessoas.Length);
        }

        public void ReadFromConsole()
        {
            while (true)
            {
                Console.WriteLine("Enter para continuar a insirir os dados de uma pessoa.\nDigite \"s\" para sair.");
                string? cmd = Console.ReadLine();
                if (cmd == "s") return;

                string? nome;
                int cpf;
                int idade;

                Console.WriteLine("Insira o nome");
                while (true)
                {
                    nome = Console.ReadLine();
                    if (nome == null || nome == "") Console.WriteLine("Insira um nome.");
                    else break;
                }
                Console.WriteLine("Insira o cpf");
                while (true)
                {
                    try
                    {
                        cpf = int.Parse(Console.ReadLine()!);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Cpf inválido. Insira novamente o cpf");
                    }
                }
                Console.WriteLine("Insira a idade");
                while (true)
                {
                    try
                    {
                        idade = int.Parse(Console.ReadLine()!);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Idade inválida. Insira novamente a idade");
                    }
                }
                Pessoas = Pessoas.Append(new Pessoa() { Cpf = cpf, Nome = nome, Idade = idade }).ToArray();
            }
        }

        public void ReadFromServer()
        {
            string encodedString = JsonSerializer.Serialize(Pessoas, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true });
            Client.Connect(2, () => { });
        }
    }
}