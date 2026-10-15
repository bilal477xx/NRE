using System;
using System.ComponentModel;
using NRE.Models.Abstracts;

namespace NRE.Models.Entities.PaymentMethods;

[Description("Western Union")]
public class WesternUnion : Payment
{
    public string ReceiverName { get; set; }

    public WesternUnion() : this(0, DateTime.Now, string.Empty) { }

    public WesternUnion(int amount, DateTime date, string receiverName) : base(amount, date)
    {
        ReceiverName = receiverName;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Western Union ---",
            GetCommonDetails(),
            $"Receiver name: {ReceiverName}");
}
