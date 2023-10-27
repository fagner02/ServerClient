using System.Net.Sockets;

namespace SD
{
    public class Building
    {
        public required string Name;
        public string? Description;
        public required string Street;
        public required string Cep;
        public string? Num;
        public string? ReferencePoint;
        public float Price;
        public bool Sold = false;
        public float Dimensions;
    }

    public class House
    {
        public int Bedrooms;
        public int Bathrooms;
    }
    public class Apartment
    {
        public int Bedrooms;
        public int Bathrooms;
        public string? ApartmentNum;
    }
    public class Flat
    {
        public bool LaundryIncluded;
    }
    public class Kitnet
    {
    }

    public class ImmobileServer : Server<Building>
    {
        [Request(Port = 3)]
        public void Sell(Socket handler)
        {
        }
    }
}