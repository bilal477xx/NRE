using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("Villa")]
public class Villa : Residential
{
    private int _plotArea;

    public bool HasGarden { get; set; }

    public int PlotArea
    {
        get => _plotArea;
        set => _plotArea = Validation.EnsureNotNegative(value, "Plot area");
    }

    public Villa() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0, 0,
        false, 0) { }

    public Villa(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int totalNumberOfRooms, int numberOfBedrooms, int numberOfBathrooms,
        bool hasGarden, int plotArea)
        : base(id, address, legalForm, area, imagePath, totalNumberOfRooms, numberOfBedrooms, numberOfBathrooms)
    {
        HasGarden = hasGarden;
        PlotArea = plotArea;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Villa ---",
            GetCommonDetails(),
            $"Garden: {Formatting.YesNo(HasGarden)}",
            $"Plot area: {PlotArea} m²");
}
