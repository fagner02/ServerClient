namespace SD
{
    public class RealEstateAgencySystemClient : SystemClient
    {
        public RealEstateAgencySystemClient()
        {
            Initialize(typeof(RealEstateAgencySystem));
        }

        public void AddSeller()
        {
            MakeRequest<Seller>(nameof(SellerServer.WriteRequest), RequestConfig.Serialize(ReadInstance<Seller>()));
        }
        public void AddCustomer()
        {
            MakeRequest<Customer>(nameof(CustomerServer.WriteRequest), RequestConfig.Serialize(ReadInstance<Customer>()));
        }
        public void AddBuilding()
        {
            MakeRequest<Building>(nameof(BuildingServer.WriteRequest), RequestConfig.Serialize(ReadInstance<Building>()));
        }
        public void SellBuilding()
        {
            List<Customer> customers = RequestConfig.Deserialize<List<Customer>>(MakeRequest<Customer>(nameof(CustomerServer.ReadRequest)));
            string name = Console.ReadLine()!;
            Customer? customer = customers.FirstOrDefault(x => x.Name == name);
            if (customer == null) return;

            List<Seller> sellers = RequestConfig.Deserialize<List<Seller>>(MakeRequest<Seller>(nameof(SellerServer.ReadRequest)));
            name = Console.ReadLine()!;
            Seller? seller = sellers.FirstOrDefault(x => x.Name == name);
            if (seller == null) return;

            PaymentMethod paymentMethod = (PaymentMethod)ReadField(typeof(PaymentMethod))!;

            Sale sale = new() { SellerId = seller.Id, CustomerId = customer.Id, paymentMethod = paymentMethod };
            MakeRequest<Sale>(nameof(SaleServer.WriteRequest), RequestConfig.Serialize(sale));
        }
    }
}