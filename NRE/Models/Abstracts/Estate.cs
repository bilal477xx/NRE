using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using NRE.Helpers;
using NRE.Models.Entities.Estates;
using NRE.Models.Entities.People;
using NRE.Models.Enums;
using NRE.Models.Interfaces;

namespace NRE.Models.Abstracts;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Apartment), nameof(Apartment))]
[JsonDerivedType(typeof(Villa), nameof(Villa))]
[JsonDerivedType(typeof(Rowhouse), nameof(Rowhouse))]
[JsonDerivedType(typeof(Hotel), nameof(Hotel))]
[JsonDerivedType(typeof(Shop), nameof(Shop))]
[JsonDerivedType(typeof(Warehouse), nameof(Warehouse))]
[JsonDerivedType(typeof(Hospital), nameof(Hospital))]
[JsonDerivedType(typeof(School), nameof(School))]
[JsonDerivedType(typeof(University), nameof(University))]

[KnownType(typeof(Apartment))]
[KnownType(typeof(Villa))]
[KnownType(typeof(Rowhouse))]
[KnownType(typeof(Hotel))]
[KnownType(typeof(Shop))]
[KnownType(typeof(Warehouse))]
[KnownType(typeof(Hospital))]
[KnownType(typeof(School))]
[KnownType(typeof(University))]
public abstract class Estate : IEstate
{
    private int _area;

    public int Id { get; set; }
    public Address Address { get; set; }
    public LegalForm LegalForm { get; set; }
    public string ImagePath { get; set; }
    public Seller? Seller { get; set; }
    public Buyer? Buyer { get; set; }

    public int Area
    {
        get => _area;
        set => _area = Validation.EnsureNotNegative(value, "Area");
    }

    /// <summary>
    /// Short description with basic info about the estate
    /// </summary>
    public string ShortDescription => $"#{Id} {GetType().Name} – {Address.Street}, {Address.City}";

    protected Estate() : this(0, new Address(), LegalForm.Ownership, 0, string.Empty) { }

    protected Estate(int id, Address address, LegalForm legalForm, int area, string imagePath)
    {
        Id = id;
        Address = address;
        LegalForm = legalForm;
        Area = area;
        ImagePath = imagePath;
    }

    public abstract string GetDescription();
    public abstract string GetCategory();

    public virtual bool Validate()
    {
        if (!Address.Validate() && string.IsNullOrEmpty(ImagePath))
            return false;
        
        return true;
    }

    /// <summary>
    /// Common details that are shared between all estate categories
    /// </summary>
    /// <returns>returns the common details of the estate</returns>
    protected virtual string GetCommonDetails() =>
        string.Join(Environment.NewLine,
            $"ID: {Id}",
            $"Category: {GetCategory()}",
            $"Address: {Address}",
            $"Legal form: {LegalForm}",
            $"Area: {Area} m²");

    public override string ToString() => GetDescription();
}
