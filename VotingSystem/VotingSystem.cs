namespace SD
{
    public class VotingSystem : SystemBase
    {
        [SystemServer(0, 20000, 5000)]
        public readonly VoteServer votoServer = new();
        [SystemServer(10)]
        public readonly VoterServer eleitorServer = new();
        [SystemServer(20)]
        public readonly AdminServer adminServer = new();
        [SystemServer(30)]
        public readonly CandidateServer candidatoServer = new();
        public readonly MulticastClient notificationServer = new();

        public void CreateAdmin()
        {
            clients[typeof(Admin)].MakeRequest(nameof(AdminServer.WriteRequest));
        }
    }
}