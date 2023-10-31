namespace SD.Voting
{
    public class Candidate
    {
        public required string Name;
        public required int Id;
    }

    public class CandidateServer : Server<Candidate>
    {

    }
}