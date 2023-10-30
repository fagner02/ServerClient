namespace SD
{
    public class Seller
    {
        public required string Name;
        public required string Id;
    }

    public class SellerServer : Server<Seller>
    {

    }
}