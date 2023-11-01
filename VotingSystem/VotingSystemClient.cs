namespace SD.Voting
{
    public class VotingSystemClient : SystemClientBase
    {
        private Role LoggedRole = Role.NotLogged;

        private enum Role
        {
            Voter,
            Admin,
            NotLogged
        }

        public VotingSystemClient()
        {
            Initialize(typeof(VotingSystem));
            MakeRequest<Admin>(nameof(AdminServer.WriteRequest), RequestConfig.Serialize(new Admin() { Name = "admin", Password = "admin" }));
        }

        public void AddAdmin()
        {
            if (LoggedRole != Role.Admin)
            {
                Console.WriteLine("you have to be logged as admin");
                return;
            }
            MakeRequest<Admin>(nameof(AdminServer.ReadRequest), RequestConfig.Serialize(ReadInstance<Admin>()));
        }

        public void AddCandidate()
        {
            if (LoggedRole != Role.Admin)
            {
                Console.WriteLine("you have to be logged as admin");
                // return;
            }
            MakeRequest<Candidate>(nameof(CandidateServer.WriteRequest), RequestConfig.Serialize(ReadInstance<Candidate>()));
        }

        public void AddVoter()
        {
            if (LoggedRole != Role.Admin)
            {
                Console.WriteLine("you have to be logged as admin");
                return;
            }
            MakeRequest<Voter>(nameof(VoterServer.WriteRequest), RequestConfig.Serialize(ReadInstance<Voter>()));
        }

        public void LogInAsAdmin()
        {
            var admin = ReadInstance<Admin>();
            Console.WriteLine("in");
            var admins = RequestConfig.Deserialize<List<Admin>>(MakeRequest<Admin>(nameof(AdminServer.ReadRequest)));
            if (admins.Any(x => x.Name == admin.Name && x.Password == admin.Password))
            {
                LoggedRole = Role.Admin;
                Console.WriteLine("logged as admin");
            }
            else
            {
                Console.WriteLine("invalid credentials");
            }
        }

        public void LogInAsVoter()
        {
            var voter = ReadInstance<Voter>();
            var voters = RequestConfig.Deserialize<List<Voter>>(MakeRequest<Voter>(nameof(VoterServer.ReadRequest)));
            if (voters.Any(x => x.Name == voter.Name && x.Password == voter.Password))
            {
                LoggedRole = Role.Voter;
                Console.WriteLine("logged as voter");
            }
            else
            {
                Console.WriteLine("invalid credentials");
            }
        }

        public void VoteForCandidate()
        {
            if (bool.Parse(MakeRequest<Vote>(nameof(VoteServer.IsTimedOutRequest))))
            {
                Console.WriteLine("election finished, you can't vote anymore");
                return;
            }
            if (LoggedRole != Role.Voter)
            {
                Console.WriteLine("you have to be logged as voter");
                return;
            }
            var candidates = RequestConfig.Deserialize<List<Candidate>>(MakeRequest<Candidate>(nameof(CandidateServer.ReadRequest)));
            if (candidates.Count == 0) { Console.WriteLine("no candidates added"); }
            foreach (Candidate item in candidates)
            {
                Console.WriteLine("candidate: " + item.Name);
                Console.WriteLine("id: " + item.Id + "\n");
            }

            Console.WriteLine("insert candidate id");
            string? input = Console.ReadLine();
            int id;
            while (true)
            {
                try
                {
                    id = int.Parse(input!);
                    if (!candidates.Any(x => x.Id == id)) throw new Exception();
                    MakeRequest<Vote>(nameof(VoteServer.WriteRequest), RequestConfig.Serialize(new Vote() { CandidateId = id }));
                    break;
                }
                catch { Console.WriteLine("insert valid candidate id"); input = Console.ReadLine(); }
            }
        }
    }
}