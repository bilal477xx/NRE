using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using NRE.Models.Entities.People;

namespace NRE.Models.Abstracts;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Buyer), nameof(Buyer))]
[JsonDerivedType(typeof(Seller), nameof(Seller))]

[KnownType(typeof(Buyer))]
[KnownType(typeof(Seller))]
//[DataContractAttribute(Name = "Person", Namespace = "NRE")]
public abstract class Person
{
    public string Name { get; set; }
    public Address Address { get; set; }

    protected Person() : this(string.Empty, new Address()) { }
    
    protected Person(string name, Address address)
    {
        Name = name;
        Address = address;
    }

    public abstract string GetDescription();

    /// <summary>
    /// Common details between all 'person' details
    /// </summary>
    /// <returns>returns common details between all 'person' details</returns>
    protected virtual string GetCommonDetails() =>
        string.Join(Environment.NewLine,
            $"Name: {Name}",
            $"Address: {Address}");

    public override string ToString() => GetDescription();

    public virtual bool Validate()
    {
        if (string.IsNullOrWhiteSpace(Name) || !Address.Validate()) return false;
        
        return true;
    }
}
