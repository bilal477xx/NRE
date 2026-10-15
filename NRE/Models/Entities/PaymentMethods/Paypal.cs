using System;
using System.ComponentModel;
using NRE.Models.Abstracts;

namespace NRE.Models.Entities.PaymentMethods;

[Description("Paypal")]
public class Paypal : Payment
{
    public string Email { get; set; }

    public Paypal() : this(0, DateTime.Now, string.Empty) { }

    public Paypal(int amount, DateTime date, string email) : base(amount, date)
    {
        Email = email;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Paypal ---",
            GetCommonDetails(),
            $"Email: {Email}");
}
