using System;
using NRE.Helpers;
using NRE.Models.Enums;

namespace NRE.Models.Abstracts.EstateCategories;

public abstract class Institutional : Estate
{
    private int _maxOccupancy;

    public int MaxOccupancy
    {
        get => _maxOccupancy;
        set => _maxOccupancy = Validation.EnsureNotNegative(value, "Max occupancy");
    }

    public bool IsPublicProperty { get; set; }

    protected Institutional() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, false) { }

    protected Institutional(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int maxOccupancy, bool isPublicProperty)
        : base(id, address, legalForm, area, imagePath)
    {
        MaxOccupancy = maxOccupancy;
        IsPublicProperty = isPublicProperty;
    }

    public sealed override string GetCategory() => "Institutional";

    /// <summary>
    /// Common details that are shared between all institutional estates
    /// </summary>
    /// <returns>returns the common details of the estate</returns>
    protected override string GetCommonDetails() =>
        string.Join(Environment.NewLine,
            base.GetCommonDetails(),
            $"Max occupancy: {MaxOccupancy}, public property: {Formatting.YesNo(IsPublicProperty)}");
}
