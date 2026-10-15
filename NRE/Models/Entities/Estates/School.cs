using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("School")]
public class School : Institutional
{
    private int _numberOfClassrooms;

    public int NumberOfClassrooms
    {
        get => _numberOfClassrooms;
        set => _numberOfClassrooms = Validation.EnsureNotNegative(value, "Number of classrooms");
    }

    public bool HasSportsHall { get; set; }

    public School() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, false,
        0, false) { }

    public School(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int maxOccupancy, bool isPublicProperty,
        int numberOfClassrooms, bool hasSportsHall)
        : base(id, address, legalForm, area, imagePath, maxOccupancy, isPublicProperty)
    {
        NumberOfClassrooms = numberOfClassrooms;
        HasSportsHall = hasSportsHall;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- School ---",
            GetCommonDetails(),
            $"Number of classrooms: {NumberOfClassrooms}",
            $"Sports hall: {Formatting.YesNo(HasSportsHall)}");
}
