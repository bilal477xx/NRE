using NRE.Models.Enums;

namespace NRE.Models;

public class Address
{
    public string Street { get; set; }
    public int ZipCode { get; set; }
    public string City { get; set; }
    public Countries Country { get; set; }

    public Address() : this(string.Empty, 0, string.Empty, default) { }

    public Address(string street, int zipCode, string city, Countries country)
    {
        Street = street;
        ZipCode = zipCode;
        City = city;
        Country = country;
    }

    public override string ToString() => $"{Street}, {ZipCode} {City}, {Country}";

    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(Street) && string.IsNullOrWhiteSpace(City))
            return false;
        
        return true;
    }
}
