using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class PessoaServer : Server<Pessoa>
    {
        private int ByteNum;
        private int PeopleNum;

        [Request(Port = 5)]
        public void SendPeopleNum(Socket handler, CancellationToken cancellationToken)
        {
            string response = "";
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return;
                byte[] buffer = new byte[1024];
                if (handler.Available == 0) break;
                int bytes = handler.Receive(buffer);
                response += Encoding.UTF8.GetString(buffer, 0, bytes);
            }
            Console.WriteLine(response);

            int data;
            try { data = int.Parse(response); }
            catch { data = 0; }

            if (cancellationToken.IsCancellationRequested) return;
            PeopleNum = data;
            handler.Send(Encoding.UTF8.GetBytes("Data received"));
            handler.Close();
        }

        [Request(Port = 6)]
        public void SendByteNum(Socket handler, CancellationToken cancellationToken)
        {
            string response = "";
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return;
                byte[] buffer = new byte[1024];
                if (handler.Available == 0) break;
                int bytes = handler.Receive(buffer);
                response += Encoding.UTF8.GetString(buffer, 0, bytes);
            }
            Console.WriteLine(response);

            int data;
            try { data = int.Parse(response); }
            catch { data = 0; }

            if (cancellationToken.IsCancellationRequested) return;
            ByteNum = data;
            handler.Send(Encoding.UTF8.GetBytes("Data received"));
            handler.Close();
        }

        [Request(Port = 7)]
        public void SendPeople([Range(0, 9)] Socket handler, CancellationToken cancellationToken)
        {
            List<Pessoa> pessoas;
            string response = "";

            if (cancellationToken.IsCancellationRequested) return;
            byte[] buffer = new byte[ByteNum];
            int bytes = handler.Receive(buffer);
            response += Encoding.UTF8.GetString(buffer, 0, bytes);

            try { pessoas = JsonSerializer.Deserialize<List<Pessoa>>(response, RequestConfig.JsonOptions)!; }
            catch { return; }

            if (cancellationToken.IsCancellationRequested) return;
            Data.AddRange(pessoas);
            Console.WriteLine(typeof(Pessoa).Name + "s adicionadas: " + PeopleNum.ToString());
            handler.Send(Encoding.UTF8.GetBytes("Data received"));
            handler.Close();
            ByteNum = 0;
            PeopleNum = 0;
        }
    }
}