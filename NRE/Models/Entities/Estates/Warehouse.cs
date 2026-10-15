using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("Warehouse")]
public class Warehouse : Commercial
{
    private int _numberOfLoadingDocks;
    private double _ceilingHeight;

    public int NumberOfLoadingDocks
    {
        get => _numberOfLoadingDocks;
        set => _numberOfLoadingDocks = Validation.EnsureNotNegative(value, "Number of loading docks");
    }

    public double CeilingHeight
    {
        get => _ceilingHeight;
        set => _ceilingHeight = Validation.EnsureNotNegative(value, "Ceiling height");
    }

    public Warehouse() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0,
        0, 0.0) { }

    public Warehouse(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int commercialZoningCode, int numberOfParkingSpaces,
        int numberOfLoadingDocks, double ceilingHeight)
        : base(id, address, legalForm, area, imagePath, commercialZoningCode, numberOfParkingSpaces)
    {
        NumberOfLoadingDocks = numberOfLoadingDocks;
        CeilingHeight = ceilingHeight;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Warehouse ---",
            GetCommonDetails(),
            $"Loading docks: {NumberOfLoadingDocks}",
            $"Ceiling height: {CeilingHeight:0.0} m");
}
