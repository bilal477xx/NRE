using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("Rowhouse")]
public class Rowhouse : Residential
{
    public bool HasGarden { get; set; }
    public bool IsEndUnit { get; set; }

    public Rowhouse() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0, 0,
        false, false) { }

    public Rowhouse(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int totalNumberOfRooms, int numberOfBedrooms, int numberOfBathrooms,
        bool hasGarden, bool isEndUnit)
        : base(id, address, legalForm, area, imagePath, totalNumberOfRooms, numberOfBedrooms, numberOfBathrooms)
    {
        HasGarden = hasGarden;
        IsEndUnit = isEndUnit;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Rowhouse ---",
            GetCommonDetails(),
            $"Garden: {Formatting.YesNo(HasGarden)}",
            $"End unit: {Formatting.YesNo(IsEndUnit)}");
}
