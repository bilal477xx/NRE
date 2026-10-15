using System;
using NRE.Helpers;
using NRE.Models.Enums;

namespace NRE.Models.Abstracts.EstateCategories;

public abstract class Residential : Estate
{
    private int _totalNumberOfRooms;
    private int _numberOfBedrooms;
    private int _numberOfBathrooms;

    public int TotalNumberOfRooms
    {
        get => _totalNumberOfRooms;
        set => _totalNumberOfRooms = Validation.EnsureNotNegative(value, "Number of rooms");
    }

    public int NumberOfBedrooms
    {
        get => _numberOfBedrooms;
        set => _numberOfBedrooms = Validation.EnsureNotNegative(value, "Number of bedrooms");
    }

    public int NumberOfBathrooms
    {
        get => _numberOfBathrooms;
        set => _numberOfBathrooms = Validation.EnsureNotNegative(value, "Number of bathrooms");
    }

    protected Residential() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, 0, 0) { }

    protected Residential(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int totalNumberOfRooms, int numberOfBedrooms, int numberOfBathrooms)
        : base(id, address, legalForm, area, imagePath)
    {
        TotalNumberOfRooms = totalNumberOfRooms;
        NumberOfBedrooms = numberOfBedrooms;
        NumberOfBathrooms = numberOfBathrooms;
    }

    public sealed override string GetCategory() => "Residential";

    /// <summary>
    /// Common details that are shared between all residential estates
    /// </summary>
    /// <returns>returns the common details of the estate</returns>
    protected override string GetCommonDetails() =>
        string.Join(Environment.NewLine,
            base.GetCommonDetails(),
            $"Rooms: {TotalNumberOfRooms} (bedrooms: {NumberOfBedrooms}, bathrooms: {NumberOfBathrooms})");
    
}
