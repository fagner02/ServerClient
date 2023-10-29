namespace SD
{
    public class Admin
    {
        public required string Name;
        public required string Password;
    }

    public class AdminServer : Server<Admin>
    {

    }
}