using NRE.Models.Enums;

namespace NRE.Models.Interfaces;

public interface IEstate
{
    int Id { get; set; }
    Address Address { get; set; }
    LegalForm LegalForm { get; set; }
    int Area { get; set; }
    string ImagePath { get; set; }

    /// <summary>
    /// A description of the estate
    /// </summary>
    /// <returns>returns a description of the estate</returns>
    string GetDescription();
    /// <summary>
    /// The category of the estate
    /// </summary>
    /// <returns>returns the category of the estate</returns>
    string GetCategory();
}