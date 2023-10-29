namespace SD
{
    public class Customer
    {
        public required string Name;
        public required string Cpf;
        public List<PaymentMethod> PaymentMethods = new();
    }

    public enum PaymentMethod
    {
        CreditCard,
        Cash,
        Pix
    }


}