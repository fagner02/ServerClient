namespace SD
{
    public class VotingSystem
    {
        [ServerPortStart(0)]
        public readonly VoteServer votoServer = new();
        [ServerPortStart(10)]
        public readonly VoterServer eleitorServer = new();
        [ServerPortStart(20)]
        public readonly AdminServer adminServer = new();
        [ServerPortStart(30)]
        public readonly CandidateServer candidatoServer = new();

        public void Run()
        {
            votoServer.Timeout = 20000;
            Task.WaitAll(new Task[] {
                Task.Run(() => {
                    Task.Delay(200000).Wait();
                    votoServer.Setup();
                }),
                Task.Run(() => {
                    Task.Delay(5000).Wait();
                    eleitorServer.Setup();
                }),
                Task.Run(() => adminServer.Setup()),
                Task.Run(() => candidatoServer.Setup()),
            });
        }
    }
}