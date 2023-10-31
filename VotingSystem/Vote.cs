

using System.Net.Sockets;
using System.Text;

namespace SD
{
    public class Vote
    {
        public required int CandidateId;
    }

    public class VoteServer : Server<Vote>
    {
        bool Closed = false;
        readonly System.Timers.Timer timer = new(50000);

        public VoteServer()
        {
            timer.Start();
            timer.Elapsed += (s, e) =>
            {
                Closed = true;
            };
            timer.AutoReset = false;
        }

        [Request(Port = 4)]
        public void IsTimedOutRequest(Socket handler, CancellationToken cancellationToken)
        {
            Console.WriteLine("Connected");

            if (cancellationToken.IsCancellationRequested) return;
            handler.Send(Encoding.UTF8.GetBytes(Closed.ToString()));
            handler.Close();
            Console.WriteLine("Sent");
        }
    }
}