using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts.EstateCategories;
using NRE.Models.Enums;

namespace NRE.Models.Entities.Estates;

[Description("University")]
public class University : Institutional
{
    private int _numberOfLaboratories;
    private int _numberOfLectureHalls;

    public int NumberOfLaboratories
    {
        get => _numberOfLaboratories;
        set => _numberOfLaboratories = Validation.EnsureNotNegative(value, "Number of laboratories");
    }

    public int NumberOfLectureHalls
    {
        get => _numberOfLectureHalls;
        set => _numberOfLectureHalls = Validation.EnsureNotNegative(value, "Number of lecture halls");
    }

    public University() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty,
        0, false,
        0, 0) { }

    public University(int id, Address address, LegalForm legalForm, int area, string imagePath,
        int maxOccupancy, bool isPublicProperty,
        int numberOfLaboratories, int numberOfLectureHalls)
        : base(id, address, legalForm, area, imagePath, maxOccupancy, isPublicProperty)
    {
        NumberOfLaboratories = numberOfLaboratories;
        NumberOfLectureHalls = numberOfLectureHalls;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- University ---",
            GetCommonDetails(),
            $"Number of laboratories: {NumberOfLaboratories}",
            $"Number of lecture halls: {NumberOfLectureHalls}");
}
