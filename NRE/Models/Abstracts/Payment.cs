using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using NRE.Models.Entities.PaymentMethods;

namespace NRE.Models.Abstracts;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(BankTransfer), nameof(BankTransfer))]
[JsonDerivedType(typeof(WesternUnion), nameof(WesternUnion))]
[JsonDerivedType(typeof(Paypal), nameof(Paypal))]

[KnownType(typeof(BankTransfer))]
[KnownType(typeof(WesternUnion))]
[KnownType(typeof(Paypal))]
public abstract class Payment
{
    public int Amount { get; set; }
    public DateTime Date { get; set; }

    protected Payment() { }

    protected Payment(int amount, DateTime date)
    {
        Amount = amount;
        Date = date;
    }

    /// <summary>
    /// Common details between all payments details
    /// </summary>
    /// <returns>returns common details between all payments details</returns>
    protected virtual string GetCommonDetails() => $"Amount: {Amount} kr, {Date}";

    /// <summary>
    /// A description of the payment details
    /// </summary>
    /// <returns>returns the payments details</returns>
    public abstract string GetDescription();

    public override string ToString() => GetDescription();
}
