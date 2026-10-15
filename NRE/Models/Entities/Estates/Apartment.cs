using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("Apartment")]
public class Apartment : Residential
{
    public bool HasBalcony { get; set; }
    public int Floor { get; set; }

    public Apartment() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0, 0,
        false, 0) { }

    public Apartment(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int totalNumberOfRooms, int numberOfBedrooms, int numberOfBathrooms,
        bool hasBalcony, int floor)
        : base(id, address, legalForm, area, imagePath, totalNumberOfRooms, numberOfBedrooms, numberOfBathrooms)
    {
        HasBalcony = hasBalcony;
        Floor = floor;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Apartment ---",
            GetCommonDetails(),
            $"Floor: {Floor}",
            $"Balcony: {Formatting.YesNo(HasBalcony)}");

    public override bool Validate()
    {
        if (!base.Validate())
            return false;
            
        return base.Validate();
    }
    
}
