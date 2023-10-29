using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class Vote
    {
        required public Candidate candidato;
    }

    public class VoteServer : Server<Vote>
    {
        [Request(Port = 1)]
        public override void WriteRequest(Socket handler, CancellationToken cancellationToken)
        {
            string response = "";
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return;
                byte[] buffer = new byte[1024];
                if (handler.Available == 0) break;
                int bytes = handler.Receive(buffer);
                Console.WriteLine(bytes);
                response += Encoding.UTF8.GetString(buffer, 0, bytes);
            }

            Candidate candidato = JsonSerializer.Deserialize<Candidate>(response, RequestConfig.JsonOptions)!;
            Data.Add(new Vote() { candidato = candidato });
            handler.Send(Encoding.UTF8.GetBytes("Voto salvo"));
            handler.Close();
        }
    }
}