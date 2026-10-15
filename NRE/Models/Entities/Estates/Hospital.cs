using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("Hospital")]
public class Hospital : Institutional
{
    private int _numberOfBeds;
    private int _numberOfOperatingRooms;

    public int NumberOfBeds
    {
        get => _numberOfBeds;
        set => _numberOfBeds = Validation.EnsureNotNegative(value, "Number of beds");
    }

    public int NumberOfOperatingRooms
    {
        get => _numberOfOperatingRooms;
        set => _numberOfOperatingRooms = Validation.EnsureNotNegative(value, "Number of operating rooms");
    }

    public Hospital() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, false,
        0, 0) { }

    public Hospital(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int maxOccupancy, bool isPublicProperty,
        int numberOfBeds, int numberOfOperatingRooms)
        : base(id, address, legalForm, area, imagePath, maxOccupancy, isPublicProperty)
    {
        NumberOfBeds = numberOfBeds;
        NumberOfOperatingRooms = numberOfOperatingRooms;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Hospital ---",
            GetCommonDetails(),
            $"Number of beds: {NumberOfBeds}",
            $"Number of operating rooms: {NumberOfOperatingRooms}");
}
