using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("Shop")]
public class Shop : Commercial
{
    private int _numberOfDisplayWindows;

    public int NumberOfDisplayWindows
    {
        get => _numberOfDisplayWindows;
        set => _numberOfDisplayWindows = Validation.EnsureNotNegative(value, "Number of display windows");
    }

    public bool IsInShoppingCentre { get; set; }

    public Shop() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0,
        0, false) { }

    public Shop(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int commercialZoningCode, int numberOfParkingSpaces,
        int numberOfDisplayWindows, bool isInShoppingCentre)
        : base(id, address, legalForm, area, imagePath, commercialZoningCode, numberOfParkingSpaces)
    {
        NumberOfDisplayWindows = numberOfDisplayWindows;
        IsInShoppingCentre = isInShoppingCentre;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Shop ---",
            GetCommonDetails(),
            $"Display windows: {NumberOfDisplayWindows}",
            $"In shopping centre: {Formatting.YesNo(IsInShoppingCentre)}");
}
