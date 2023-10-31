using System.Reflection;
using System.Text.Json;

namespace SD.Voting
{
    public class VotingSystem : SystemBase
    {
        [SystemServer(0)]
        public readonly VoteServer votoServer = new();
        [SystemServer(10)]
        public readonly VoterServer eleitorServer = new();
        [SystemServer(20)]
        public readonly AdminServer adminServer = new();
        [SystemServer(30)]
        public readonly CandidateServer candidatoServer = new();
        public static void SendNotification(string msg)
        {
            MulticastSender.Send(msg);
        }
    }
}