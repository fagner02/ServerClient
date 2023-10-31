namespace SD
{
    public class RealEstateAgencySystemClient : SystemClient
    {
        public RealEstateAgencySystemClient()
        {
            Initialize(typeof(RealEstateAgencySystem));
        }

        public void AddSeller() { }
        public void AddCustomer() { }
        public void AddBuilding() { }
        public void SellBuilding() { }
    }
}