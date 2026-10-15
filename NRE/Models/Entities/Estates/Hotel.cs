using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("Hotel")]
public class Hotel : Commercial
{
    private int _numberOfRooms;

    public int NumberOfRooms
    {
        get => _numberOfRooms;
        set => _numberOfRooms = Validation.EnsureNotNegative(value, "Number of rooms");
    }

    public bool HasSwimmingPool { get; set; }

    public Hotel() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0,
        0, false) { }

    public Hotel(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int commercialZoningCode, int numberOfParkingSpaces,
        int numberOfRooms, bool hasSwimmingPool)
        : base(id, address, legalForm, area, imagePath, commercialZoningCode, numberOfParkingSpaces)
    {
        NumberOfRooms = numberOfRooms;
        HasSwimmingPool = hasSwimmingPool;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Hotel ---",
            GetCommonDetails(),
            $"Number of rooms: {NumberOfRooms}",
            $"Swimming pool: {Formatting.YesNo(HasSwimmingPool)}");
}
