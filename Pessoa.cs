using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class Pessoa
    {
        public string Nome;
        public int Cpf;
        public int Idade;
    }

    public class PessoasOutputStream
    {
        Pessoa[] Pessoas;
        public PessoasOutputStream(Pessoa[] pessoas)
        {
            Pessoas = pessoas;
        }
        public void SaveToFile()
        {
            FileStream file = new("Pessoas.txt", FileMode.OpenOrCreate);
            string encodedString = JsonSerializer.Serialize(Pessoas, new JsonSerializerOptions() { IncludeFields = true });
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
            Client.Connect(encodedString);
        }
    }
}