namespace SD
{
    public class RealEstateAgencySystem : SystemBase
    {
        [SystemServer(0)]
        public readonly BuildingServer buildingServer = new();
        [SystemServer(10)]
        public readonly CustomerServer customerServer = new();
        [SystemServer(20)]
        public readonly SaleServer saleServer = new();
        [SystemServer(30)]
        public readonly SellerServer sellerServer = new();
    }
}