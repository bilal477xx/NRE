using System;
using System.ComponentModel;
using NRE.Models.Abstracts;

namespace NRE.Models.Entities.PaymentMethods;

[Description("Bank Transfer")]
public class BankTransfer : Payment
{
    public string BankName { get; set; }

    public BankTransfer() : this(0, DateTime.Now, string.Empty) { }

    public BankTransfer(int amount, DateTime date, string bankName) : base(amount, date)
    {
        BankName = bankName;
    }

    public override string GetDescription() =>
        string.Join(Environment.NewLine,
            "--- Bank Transfer ---",
            GetCommonDetails(),
            $"Bank name: {BankName}");
}
