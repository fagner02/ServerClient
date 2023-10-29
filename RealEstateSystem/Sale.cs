namespace SD
{
    public class Sale
    {
        public required Guid CustomerId;
        public required Guid SellerId;
        public required PaymentMethod paymentMethod;
    }
}