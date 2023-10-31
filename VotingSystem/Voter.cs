namespace SD.Voting
{
    public class Voter
    {
        public required string Name;
        public required string Password;
    }

    public class VoterServer : Server<Voter>
    {

    }
}