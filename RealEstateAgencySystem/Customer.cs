namespace SD
{
    public class Customer
    {
        public required string Name;
        public required string Cpf;
        public required string Id;
        public List<PaymentMethod> PaymentMethods = new();
    }

    public enum PaymentMethod
    {
        CreditCard,
        Cash,
        Pix
    }

    public class CustomerServer : Server<Customer> { }
}