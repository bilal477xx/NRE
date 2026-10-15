using System;
using NRE.Helpers;
using NRE.Models.Enums;

namespace NRE.Models.Abstracts.EstateCategories;

public abstract class Commercial : Estate
{
    private int _numberOfParkingSpaces;

    public int CommercialZoningCode { get; set; }

    public int NumberOfParkingSpaces
    {
        get => _numberOfParkingSpaces;
        set => _numberOfParkingSpaces = Validation.EnsureNotNegative(value, "Number of parking spaces");
    }

    protected Commercial() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0) { }

    protected Commercial(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int commercialZoningCode, int numberOfParkingSpaces)
        : base(id, address, legalForm, area, imagePath)
    {
        CommercialZoningCode = commercialZoningCode;
        NumberOfParkingSpaces = numberOfParkingSpaces;
    }

    public sealed override string GetCategory() => "Commercial";

    /// <summary>
    /// Common details that are shared between all commercial estates
    /// </summary>
    /// <returns>returns the common details of the estate</returns>
    protected override string GetCommonDetails() =>
        string.Join(Environment.NewLine,
            base.GetCommonDetails(),
            $"Commercial zoning code: {CommercialZoningCode}, number of parking spaces: {NumberOfParkingSpaces}");
}
