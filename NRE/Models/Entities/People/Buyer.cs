using System;
using System.ComponentModel;
using NRE.Helpers;
using NRE.Models.Abstracts;

namespace NRE.Models.Entities.People;

[Description("Buyer")]
public class Buyer : Person
{
    private int _balance;

    public int Balance
    {
        get => _balance;
        set => _balance = Validation.EnsureNotNegative(value, nameof(Balance));
    }

    public Payment? Payment { get; set; }

    public Buyer() : this(string.Empty, new Address(), 0, null) { }

    public Buyer(string name, Address address, int balance, Payment? payment) : base(name, address)
    {
        Balance = balance;
        Payment = payment;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Buyer ---",
            GetCommonDetails(),
            $"Balance: {Balance}",
            $"Payment: {Payment?.ToString() ?? "None"}");
}
