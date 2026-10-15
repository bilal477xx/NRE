using System;
using System.ComponentModel;
using NRE.Models.Abstracts;

namespace NRE.Models.Entities.People;

[Description("Seller")]
public class Seller : Person
{
    public const int MinRating = 0;
    public const int MaxRating = 10;

    private int _rating;

    public int Rating
    {
        get => _rating;
        set
        {
            if (value is < MinRating or > MaxRating)
                throw new ArgumentOutOfRangeException(nameof(Rating), $"Rating must be between {MinRating} and {MaxRating}.");
            _rating = value;
        }
    }

    public Seller() : this(string.Empty, new Address(), 0) { }

    public Seller(string name, Address address, int rating) : base(name, address)
    {
        Rating = rating;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Seller ---",
            GetCommonDetails(),
            $"Rating: {Rating}");
}
