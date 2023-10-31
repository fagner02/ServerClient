namespace SD
{
    public class Sale
    {
        public required string CustomerId;
        public required string SellerId;
        public required PaymentMethod paymentMethod;
    }

    public class SaleServer : Server<Sale> { }
}