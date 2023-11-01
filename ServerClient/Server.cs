using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class Server<T> : ServerBase
    {
        /// <summary>
        /// Lista de entidades do tipo T salva no server
        /// </summary>
        protected List<T> Data = new();

        /// <summary>
        /// Request que lê o array de entidades do server
        /// </summary>
        /// <param name="handler">O socket da conexão</param>
        /// <param name="cancellationToken"></param>
        [Request(Port = 2)]
        public virtual void ReadRequest(Socket handler, CancellationToken cancellationToken)
        {
            Console.WriteLine("Connected");
            string encodedString = JsonSerializer.Serialize(Data, RequestConfig.JsonOptions);
            if (cancellationToken.IsCancellationRequested) return;
            handler.Send(Encoding.UTF8.GetBytes(encodedString));
            handler.Close();
            Console.WriteLine("Sent");
        }

        /// <summary>
        /// Request que escreve uma ou mais entidades ao array de entidades do server
        /// </summary>
        /// <param name="handler">O socket da conexão</param>
        /// <param name="cancellationToken"></param>
        [Request(Port = 1)]
        public virtual void WriteRequest(Socket handler, CancellationToken cancellationToken)
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
            Console.WriteLine(response);
            List<T>? data;

            try { data = JsonSerializer.Deserialize<List<T>>(response, RequestConfig.JsonOptions); }
            catch { data = new() { JsonSerializer.Deserialize<T>(response, RequestConfig.JsonOptions)! }; }

            if (data == null) return;
            if (cancellationToken.IsCancellationRequested) return;
            Data.AddRange(data);
            Console.WriteLine(typeof(T).Name + " adicionadas: " + data.Count.ToString());
            handler.Send(Encoding.UTF8.GetBytes("Data received"));
            handler.Close();
        }
    }
}