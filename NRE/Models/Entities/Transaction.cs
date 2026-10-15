using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NRE.Models.Abstracts;
using NRE.Models.Entities.People;

namespace NRE.Models.Entities;

public class Transaction : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public Seller? Seller { get; set; }
    public Buyer? Buyer { get; set; }
    public Estate? Estate { get; set; }

    public Payment? Payment
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public Transaction() { }

    public Transaction(Seller seller, Buyer buyer, Estate estate, Payment payment)
    {
        Seller = seller;
        Buyer = buyer;
        Estate = estate;
        Payment = payment;
    }

    public override string ToString() =>
        string.Join(Environment.NewLine,
            $"Transaction: Seller: {Seller?.Name} - Buyer: {Buyer?.Name}",
            $"Estate: {Estate?.ShortDescription}",
            $"Payment details: {Payment}");

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public bool Validate()
    {
        if (Seller == null || Buyer == null || Estate == null || Payment == null) return false;

        return true;
    }
}
